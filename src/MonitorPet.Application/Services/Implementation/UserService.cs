using System.Security.Claims;
using MonitorPet.Application.Email;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Security;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Application.UoW;
using MonitorPet.Core.Entity;
using MonitorPet.Core.Security;
using static MonitorPet.Application.Security.Internal.InternalHash;

namespace MonitorPet.Application.Services.Implementation;

public class UserService : IUserService
{
    private readonly ContextClaim _contextClaim;
    private readonly IDosadorRepository _dosadorRepository;
    private readonly IUserEmail _userEmail;
    private readonly IUserRepository _userRepository;
    private readonly IUsuarioDosadorRepository _usuarioDosadorRepository;
    private readonly IUnitOfWork _uoW;

    public UserService(
        ContextClaim contextClaim,
        IDosadorRepository dosadorRepository,
        IUserEmail userEmail,
        IUserRepository userRepository,
        IUsuarioDosadorRepository usuarioDosadorRepository,
        IUnitOfWork uoW)
    {
        _contextClaim = contextClaim;
        _dosadorRepository = dosadorRepository;
        _userEmail = userEmail;
        _userRepository = userRepository;
        _usuarioDosadorRepository = usuarioDosadorRepository;
        _uoW = uoW;
    }

    public async Task<ResultUserLoginModel> ConfirmEmail(string email)
    {
        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredEmail != email)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado para confirmação de e-mail.");

        if (!currentClaims.ContainsRole(RoleClaim.RoleConfirmEmail.Value))
            throw new Core.Exceptions.ForbiddenCoreException("Não autorizado para confirmar e-mail.");

        using var transaction = await _uoW.BeginTransactionAsync();

        var userFound =
            await _userRepository.GetByEmailOrDefault(email)
            ?? throw new Core.Exceptions.NotFoundCoreException("Email não encontrado.");

        var userEmailConfirmed = await TryUpdateConfirmEmail(userFound);

        await transaction.SaveChangesAsync();

        return CreateResultLogin(userEmailConfirmed);
    }

    public async Task Create(CreateUserModel createUserModel)
    {
        var userEntityToCreate = CreateUserWithHashedPassword(createUserModel);

        using var transaction = await _uoW.BeginTransactionAsync();

        await ThrowIfIsInvalidTokenAccessDosador(createUserModel.TokenAccessDosador);

        var constainsEmailRegistered =
            (await _userRepository.GetByEmailOrDefault(userEntityToCreate.Email)) is not null;
        if (constainsEmailRegistered)
            throw new Core.Exceptions.ConflictCoreException("Email já registrado.");

        var userCreated =
            await _userRepository.Create(userEntityToCreate);

        await RegisterAccessTokenDosador(userCreated.Id, createUserModel.TokenAccessDosador);

        await _userEmail.SendEmailConfirmPassword(
            userCreated, CreateClaimConfirmEmail(userCreated));

        await transaction.SaveChangesAsync();
    }

    public async Task<QueryUserModel> GetByIdUser(int idUser)
    {
        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado.");

        using var connection = await _uoW.OpenConnectionAsync();

        var userFound =
            await _userRepository.GetByIdOrDefault(idUser)
            ?? throw new Core.Exceptions.NotFoundCoreException("Usuário não encontrado.");

        return Map.UserMap.MapQueryUserModel(userFound);
    }

    public async Task<ResultUserLoginModel> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
            throw new Core.Exceptions.UnauthorizedCoreException();

        using var connection = await _uoW.BeginTransactionAsync();

        var userByLogin = await _userRepository.GetByEmailOrDefault(email)
            ?? throw new Core.Exceptions.UnauthorizedCoreException("Login ou senha inválidos.");

        if (userByLogin.AccessFailedCount >= Core.Entity.User.MAX_COUNT_ACCESS_FAILED &&
            (userByLogin.LockOutEnd is not null &&
            userByLogin.LockOutEnd.Value < DateTime.Now))
            throw new Exceptions.BlockedUserCoreException(userByLogin.LockOutEnd.Value);

        var userToUpdateAccessAccountFailed = Map.UserMap.MapUserModelWithOutPassword(userByLogin);

        if (!Security.Internal.InternalHash.IsValidPassword(password, userByLogin.PasswordHash, userByLogin.PasswordSalt))
        {
            userToUpdateAccessAccountFailed.AddAcessFailedAccount();
            await _userRepository.UpdateAccessAccountFailed(
                userByLogin.Id, userToUpdateAccessAccountFailed);
            await _uoW.SaveChangesAsync();
            throw new Core.Exceptions.UnauthorizedCoreException("Login ou senha inválidos.");
        }

        ThorwIfUserContainsEmailNotConfirmed(userByLogin);

        if (userToUpdateAccessAccountFailed.TryResetAcessFailedAccount())
        {
            await _userRepository.UpdateAccessAccountFailed(
                userByLogin.Id, userToUpdateAccessAccountFailed);
            await _uoW.SaveChangesAsync();
        }

        return CreateResultLogin(userByLogin);
    }

    public async Task SendAgainEmailConfirmAccount(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Core.Exceptions.CommonCoreException("E-mail inválido.");

        using var connection = await _uoW.OpenConnectionAsync();
        var userToConfirmEmail = await _userRepository.GetByEmailOrDefault(email)
            ?? throw new Core.Exceptions.NotFoundCoreException("Usuário não encontrado");

        if (userToConfirmEmail.EmailConfirmed)
            return;

        await _userEmail.SendEmailConfirmPassword(
            userToConfirmEmail, CreateClaimConfirmEmail(userToConfirmEmail));
    }

    public async Task SendEmailChangePassword(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Core.Exceptions.CommonCoreException("E-mail inválido.");

        using var connection = await _uoW.OpenConnectionAsync();
        var userToConfirmEmail = await _userRepository.GetByEmailOrDefault(email);

        if (userToConfirmEmail is null)
            return;

        await _userEmail.SendEmailChangePassword(
            userToConfirmEmail, CreateClaimChangePassword(userToConfirmEmail));
    }

    public async Task<ResultUserLoginModel> ConfirmChangePassoword(string email, string newPassword)
    {
        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredEmail != email)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado para confirmação de e-mail.");

        if (!currentClaims.ContainsRole(RoleClaim.RoleChangePassword.Value))
            throw new Core.Exceptions.ForbiddenCoreException("Não autorizado para confirmar e-mail.");

        using var transaction = await _uoW.BeginTransactionAsync();

        var userFound =
            await _userRepository.GetByEmailOrDefault(email)
            ?? throw new Core.Exceptions.NotFoundCoreException("Email não encontrado.");

        userFound.Password = newPassword;

        var userPasswordChange = CreateUserWithHashedPassword(
            new CreateUserModel
            {
                Email = userFound.Email,
                Password = newPassword,
                Name = userFound.Name,
                NickName = userFound.NickName
            }
        );

        userPasswordChange.ConfirmEmail();

        var userUpdated =
            await _userRepository.UpdatePasswordByIdOrDefault(userFound.Id, userPasswordChange)
            ?? throw new Core.Exceptions.CommonCoreException("Falha em atualizar a senha.");

        await transaction.SaveChangesAsync();

        return CreateResultLogin(userUpdated);
    }

    public async Task<QueryUserModel> Update(int idUser, UpdateUserModel updateUserModel)
    {
        var userClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (userClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException();

        var userEntityToUpdate = CreateUpdateUser(updateUserModel);

        using var transaction = await _uoW.BeginTransactionAsync();

        if (userClaims.RequiredEmail != updateUserModel.Email)
        {
            await ThrowIfAlreadyExistsEmail(updateUserModel.Email);
        }

        var userUpdated =
            await _userRepository.UpdateByIdOrDefault(idUser, userEntityToUpdate)
            ?? throw new Core.Exceptions.NotFoundCoreException("Usuário não encontrado.");

        await transaction.SaveChangesAsync();

        return Map.UserMap.MapQueryUserModel(userUpdated);
    }

    public async Task<QueryUserModel> ChangePassword(int idUser, string oldPassword, string newPassword)
    {
        var userClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (userClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException();

        Core.Entity.User.ThrowIfIsInvalidPassword(newPassword);

        using var transaction = await _uoW.BeginTransactionAsync();

        var userToChangePassword =
            await _userRepository.GetByIdOrDefault(idUser)
            ?? throw new Core.Exceptions.NotFoundCoreException("Usuário não encontrado.");

        if (userToChangePassword.Password != oldPassword)
            throw new Core.Exceptions.CommonCoreException("Senha antiga não confere.");

        var userEntity = CreateUpdateUserWithPassword(userToChangePassword, newPassword);

        var userUpdated =
            await _userRepository.UpdatePasswordByIdOrDefault(idUser, userEntity)
            ?? throw new Core.Exceptions.NotFoundCoreException("Usuário não encontrado.");

        await transaction.SaveChangesAsync();

        return Map.UserMap.MapQueryUserModel(userUpdated);
    }

    /// <summary>
    /// Try update <see cref="UserModel.EmailConfirmed"/>, needs a connection
    /// </summary>
    private async Task<UserModel> TryUpdateConfirmEmail(UserModel userToTryUpdate)
    {
        if (userToTryUpdate.EmailConfirmed)
            return userToTryUpdate;

        userToTryUpdate.EmailConfirmed = true;

        var userEntity = Map.UserMap.MapUserModelWithOutPassword(userToTryUpdate);

        await _userRepository.UpdateByIdOrDefault(userToTryUpdate.Id, userEntity);

        return userToTryUpdate;
    }

    private static User CreateUserWithHashedPassword(CreateUserModel createUserModel)
    {
        var hashedPassword = Security.Internal.InternalHash.CreateHashedPassword(createUserModel.Password);
        return User.Create(
            createUserModel.Email,
            createUserModel.Email,
            false,
            null,
            User.MIN_COUNT_ACCESS_FAILED,
            createUserModel.Name,
            createUserModel.NickName,
            createUserModel.Password,
            hashedPassword.HashBase64,
            hashedPassword.Salt
        );
    }

    private static User CreateUpdateUser(UpdateUserModel updateUserModel)
        => User.CreateWithOutPassword(
            updateUserModel.Email,
            updateUserModel.Email,
            true,
            null,
            User.MIN_COUNT_ACCESS_FAILED,
            updateUserModel.Name,
            updateUserModel.NickName
        );



    private static User CreateUpdateUserWithPassword(UserModel userModel, string newPassword)
    {
        var hashedPassword = Security.Internal.InternalHash.CreateHashedPassword(newPassword);

        return
            User.Create(
                userModel.Email,
                userModel.Email,
                true,
                null,
                User.MIN_COUNT_ACCESS_FAILED,
                userModel.Name,
                userModel.NickName,
                userModel.Password,
                hashedPassword.HashBase64,
                hashedPassword.Salt
            );
    }

    /// <summary>
    /// Check token to access dosador, needs a connection
    /// </summary>
    /// <exception cref="Core.Exceptions.CommonCoreException"></exception>
    private async Task ThrowIfIsInvalidTokenAccessDosador([System.Diagnostics.CodeAnalysis.NotNull] string? tokenAccessDosador)
    {
        if (string.IsNullOrEmpty(tokenAccessDosador) ||
            !Guid.TryParse(tokenAccessDosador, out Guid token))
            throw new Core.Exceptions.CommonCoreException("Token de acesso à dosador inválido.");

        var containsDosador = (await _dosadorRepository.GetByIdOrDefault(token)) is not null;

        if (!containsDosador)
            throw new Core.Exceptions.CommonCoreException("Token de acesso à dosador inválido.");
    }

    /// <summary>
    /// Join dosador and user with <paramref name="tokenAccessDosador"/>
    /// </summary>
    /// <exception cref="Core.Exceptions.CommonCoreException"></exception>
    private async Task<DosadorJoinUsuarioDosadorModel> RegisterAccessTokenDosador(int idUser, string tokenAccessDosador)
    {
        if (!Guid.TryParse(tokenAccessDosador, out Guid idDosador))
            throw new Core.Exceptions.CommonCoreException("Token acesso à dosador é inválido.");

        var dosadorExists = _dosadorRepository.GetByIdOrDefault(idDosador)
            ?? throw new Core.Exceptions.NotFoundCoreException("Dosador não econtrado.");

        var entityUserDosador = UsuarioDosador.Create(idDosador, idUser);

        var usuarioDosadorCreated = await _usuarioDosadorRepository.Create(entityUserDosador);

        return await _usuarioDosadorRepository.GetByIdUserAndIdDosador(
            idUser, usuarioDosadorCreated.IdDosador)
            ?? throw new Core.Exceptions.NotFoundCoreException("Dosador não econtrado.");
    }

    private async Task ThrowIfAlreadyExistsEmail(string email)
    {
        var constainsEmailRegistered =
                (await _userRepository.GetByEmailOrDefault(email)) is not null;
        if (constainsEmailRegistered)
            throw new Core.Exceptions.ConflictCoreException("Email já registrado.");
    }

    private static ResultUserLoginModel CreateResultLogin(UserModel user)
        => new ResultUserLoginModel
        {
            Claims = CreateClaimUser(user),
            UserModel = Map.UserMap.MapQueryUserModel(user)
        };

    private static Claim[] CreateClaimConfirmEmail(UserModel userCreated)
        => new Claim[] {
            RoleClaim.CreateUserClaimEmail(userCreated.Email),
            RoleClaim.RoleConfirmEmail
        };

    private static Claim[] CreateClaimChangePassword(UserModel userToChangePassword)
        => new Claim[] {
            RoleClaim.CreateUserClaimEmail(userToChangePassword.Email),
            RoleClaim.RoleChangePassword
        };

    private static Claim[] CreateClaimUser(UserModel user)
        => new Claim[] {
            RoleClaim.CreateUserClaimEmail(user.Email),
            RoleClaim.CreateUserClaimId(user.Id.ToString()),
            RoleClaim.CreateUserClaimName(user.Name)
        };

    /// <summary>
    /// If contains id dosador, remove old for <paramref name="currentClaims"/> and add new
    /// </summary>
    private static Claim[] AddOrUpdateClaimIdDosador(string idDosador, IEnumerable<Claim> currentClaims)
        => currentClaims.Where(claim => claim.Type != RoleClaim.DEFAULT_ID_DOSADOR)
            .Concat(new Claim[] { RoleClaim.CreateIdDosador(idDosador) })
            .ToArray();

    private static void ThorwIfUserContainsEmailNotConfirmed(UserModel userModel)
    {
        if (!userModel.EmailConfirmed)
            throw new Exceptions.UserEmailNotConfirmedCoreException(userModel.Email);
    }
}