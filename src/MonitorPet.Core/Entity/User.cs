using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Entity;

public class User : Entity
{
    public const int MINUTES_BLOCK = 10;
    public const int MAX_COUNT_ACCESS_FAILED = 10;
    public const int MIN_COUNT_ACCESS_FAILED = 0;
    public const int MIN_CHAR_LOGIN_AND_EMAIL = 4;
    public const int MAX_CHAR_LOGIN_AND_EMAIL = 255;
    public const int MIN_CHAR_NAME = 1;
    public const int MAX_CHAR_NAME = 100;
    public const int MIN_CHAR_LAST_NAME = 1;
    public const int MAX_CHAR_LAST_NAME = 255;
    public const string ALLOWED_CHAR_PASSWORD = Internal.Validation.LETTER_NUMBER_SPACE;
    public const string PATTERN_REGEX_PASSWORD = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,100}$";

    private static CommonCoreException CreateInvalidPasswordCoreException =>
        new CommonCoreException($"Senha inválida. Deve conter pelo menos uma letra maiúcula e minúscula, e um número, tendo 8 caracteres.");

    /// <summary>
    /// Identifier
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; private set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Email confirmed
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// Date until lockout
    /// </summary>
    public DateTime? LockOutEnd { get; private set; }

    /// <summary>
    /// Count of fails access
    /// </summary>
    public int AccessFailedCount { get; private set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// NIckName
    /// </summary>
    public string? NickName { get; private set; }

    /// <summary>
    /// Password
    /// </summary>
    public char[] Password { get; private set; }

    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }

    /// <summary>
    /// Private instance
    /// </summary>
    private User(
        int id, 
        string login, 
        string email, 
        bool emailConfirmed,
        DateTime? lockOutEnd,
        int accessFailedCount, 
        string name, 
        string? nickName,
        char[] password,
        string passwordHash,
        string passwordSalt)
    {
        Id = id;
        Login = login;
        Email = email;
        EmailConfirmed = emailConfirmed;
        LockOutEnd = lockOutEnd;
        AccessFailedCount = accessFailedCount;
        Name = name;
        NickName = nickName;
        Password = password;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void AddAcessFailedAccount()
    {
        if (AccessFailedCount < MAX_COUNT_ACCESS_FAILED)
            AccessFailedCount++;
        if (AccessFailedCount >= MAX_COUNT_ACCESS_FAILED)
            LockOutEnd = DateTime.Now.AddMinutes(MINUTES_BLOCK);
    }

    /// <summary>
    /// Try reset account failed
    /// </summary>
    /// <returns>If already reseted, return false.</returns>
    public bool TryResetAcessFailedAccount()
    {
        if (AccessFailedCount == MIN_COUNT_ACCESS_FAILED &&
            LockOutEnd is null)
            return false;

        AccessFailedCount = MIN_COUNT_ACCESS_FAILED;
        LockOutEnd = null;
        return true;
    }
    
    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;
        
        var user = obj as User;
        if (user is null)
            return false;

        if (!this.Login.Equals(user.Login) ||
            !this.Email.Equals(user.Email) ||
            !this.EmailConfirmed.Equals(user.EmailConfirmed) ||
            !this.Password.Equals(user.Password) ||
            !this.LockOutEnd.Equals(user.LockOutEnd) ||
            !this.AccessFailedCount.Equals(user.AccessFailedCount) ||
            !this.Name.Equals(user.Name) ||
            this.NickName != user.NickName)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 45864957;
    }

    /// <summary>
    /// Creates a new instance of <see cref="User"/>
    /// </summary>
    /// <returns>new <see cref="User"/></returns>
    public static User Create(
        string login,
        string email, 
        bool emailConfirmed,
        DateTime? lockOutEnd, 
        int accessFailedCount, 
        string name, 
        string? nickName,
        string password,
        string passwordHash,
        string passwordSalt)
    {
        var id = 0;

        CheckFields(login, email, emailConfirmed, lockOutEnd, accessFailedCount, name, nickName);

        if (!IsValidPassword(password))
            throw CreateInvalidPasswordCoreException;

        return new User(id, login.Trim(), email.Trim(), emailConfirmed, 
            lockOutEnd, accessFailedCount, name.Trim(), nickName?.Trim(), 
            new char[0], passwordHash, passwordSalt);
    }

    /// <summary>
    /// Creates a new instance of <see cref="User"/>
    /// </summary>
    /// <returns>new <see cref="User"/></returns>
    public static User CreateWithOutPassword(
        string login,
        string email, 
        bool emailConfirmed,
        DateTime? lockOutEnd, 
        int accessFailedCount, 
        string name, 
        string? nickName)
    {
        var id = 0;

        CheckFields(login, email, emailConfirmed, lockOutEnd, accessFailedCount, name, nickName);

        return new User(id, login.Trim(), email.Trim(), emailConfirmed, lockOutEnd, 
            accessFailedCount, name.Trim(), nickName?.Trim(), 
            new char[0], string.Empty, string.Empty);
    }

    private static void CheckFields(
        string login,
        string email, 
        bool emailConfirmed, 
        DateTime? lockOutEnd, 
        int accessFailedCount, 
        string name, 
        string? nickName)
    {
        if (accessFailedCount > MAX_COUNT_ACCESS_FAILED 
            || accessFailedCount < MIN_COUNT_ACCESS_FAILED)
            throw new CommonCoreException($"{nameof(accessFailedCount)} must be in range between '{MIN_COUNT_ACCESS_FAILED}' to '{MAX_COUNT_ACCESS_FAILED}'.");
        
        if (string.IsNullOrWhiteSpace(email) ||
            !Internal.Validation.IsValidEmail(email))
            throw new CommonCoreException($"{nameof(email)} is empty or invalid.");

        if (string.IsNullOrWhiteSpace(login))
            throw new CommonCoreException($"{nameof(login)} is empty or invalid.");

        if (string.IsNullOrWhiteSpace(name))
            throw new CommonCoreException($"{nameof(name)} is null or empty.");

        if (email.Length < MIN_CHAR_LOGIN_AND_EMAIL ||
            email.Length > MAX_CHAR_LOGIN_AND_EMAIL)
            throw new CommonCoreException($"{nameof(email)} must have a length between {MIN_CHAR_LOGIN_AND_EMAIL} and {MAX_CHAR_LOGIN_AND_EMAIL}.");

        if (login.Length < MIN_CHAR_LOGIN_AND_EMAIL ||
            login.Length > MAX_CHAR_LOGIN_AND_EMAIL)
            throw new CommonCoreException($"{nameof(login)} must have a length between {MIN_CHAR_LOGIN_AND_EMAIL} and {MAX_CHAR_LOGIN_AND_EMAIL}.");
        
        if (name.Length < MIN_CHAR_NAME ||
            name.Length > MAX_CHAR_NAME)
            throw new CommonCoreException($"{nameof(name)} must have a length between {MIN_CHAR_NAME} and {MAX_CHAR_NAME}.");
        
        if (nickName is not null &&
            (nickName.Length < MIN_CHAR_LAST_NAME ||
            nickName.Length > MAX_CHAR_LAST_NAME))
            throw new CommonCoreException($"{nameof(nickName)} must have a length between {MIN_CHAR_LAST_NAME} and {MAX_CHAR_LAST_NAME}.");
    }
    
    /// <summary>
    /// Check if is valid password, if invalid, return false.
    /// </summary>
    /// <param name="password">password to check</param>
    public static bool IsValidPassword(params char[] password)
    {
        return IsValidPassword(new string(password));
    }

    /// <summary>
    /// Check if is valid password, if invalid, return false.
    /// </summary>
    /// <param name="password">password to check</param>
    public static bool IsValidPassword(string password)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(password, PATTERN_REGEX_PASSWORD))
            return true;

        if (Internal.Validation.ContainsNotAllowedChars(ALLOWED_CHAR_PASSWORD, password))
            throw new CommonCoreException($"Caracteres inválidos na senha, só é permitido '{ALLOWED_CHAR_PASSWORD}'.");

        return false;
    }

    /// <summary>
    /// Check if is valid password, if invalid, throw <see cref="CommonCoreException"/>
    /// </summary>
    /// <param name="password">password to check</param>
    /// <exception cref="CommonCoreException"></exception>
    public static void ThrowIfIsInvalidPassword(params char[] password)
        => ThrowIfIsInvalidPassword(new string(password));
    
    
    /// <summary>
    /// Check if is valid password, if invalid, throw <see cref="CommonCoreException"/>
    /// </summary>
    /// <param name="password">password to check</param>
    /// <exception cref="CommonCoreException"></exception>
    public static void ThrowIfIsInvalidPassword(string password)
    {
        if (!IsValidPassword(password))
            throw CreateInvalidPasswordCoreException;
    }
}
