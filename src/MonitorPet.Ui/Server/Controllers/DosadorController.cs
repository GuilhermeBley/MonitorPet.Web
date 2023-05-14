using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Services.Implementation;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Ui.Server.Security;
using MonitorPet.Ui.Shared.Model.Dosador;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MonitorPet.Ui.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DosadorController : ControllerBase
{
    private readonly IDosadorService _dosadorService;
    private readonly IMapper _map;
    private readonly Application.Security.ContextClaim _contextClaim;

    public DosadorController(IDosadorService dosadorService, IMapper map, IApiToken tokenService,
        Application.Security.ContextClaim contextClaim)
    {
        _dosadorService = dosadorService;
        _map = map;
        _contextClaim = contextClaim;
    }

    [HttpGet("All")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<DosadorJoinUsuarioDosadorViewModel>>> GetDosadores()
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        var userDosadores =
            (await _dosadorService.GetDosadoresByIdUser(ctx.RequiredIdUser))
                .Select(d => _map.Map<DosadorJoinUsuarioDosadorViewModel>(d));

        if (!userDosadores.Any())
            return NoContent();

        return Ok(userDosadores);
    }

    [HttpGet("Info/All/{page}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<JoinUsuarioDosadorInfoViewModel>>> GetDosadoresInfo(
        [Range(0, int.MaxValue)]int page)
    {
        const int PAGE_SIZE = 10;

        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        var userDosadores =
            (await _dosadorService.GetDosadoresInfoByIdUser(ctx.RequiredIdUser)
            .Skip(page * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToListAsync())
            .Select(d => _map.Map<JoinUsuarioDosadorInfoViewModel>(d));

        if (!userDosadores.Any())
            return NoContent();

        return Ok(userDosadores);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<DosadorJoinUsuarioDosadorViewModel?>> GetDosadoresByIdOrDefault([FromQuery] string id)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        var userDosador =
            (await _dosadorService.GetDosadoresByIdUser(ctx.RequiredIdUser))
                .Select(d => _map.Map<DosadorJoinUsuarioDosadorViewModel>(d))
                .FirstOrDefault(d => d.IdDosador.ToString() == id);

        if (userDosador is null)
            return NoContent();

        return Ok(userDosador);
    }

    [HttpPost("Add")]
    [Authorize]
    public async Task<ActionResult<DosadorJoinUsuarioDosadorViewModel>> AddDosador([FromBody] AddDosadorViewModel addDosadorModel)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        var dosadorAdded = 
            await _dosadorService.AddDosadorToUser(ctx.RequiredIdUser, addDosadorModel.Token);

        return Created($"/api/dosador?id={dosadorAdded.IdDosador}", dosadorAdded);
    }

    [HttpDelete("Remove")]
    [Authorize]
    public async Task<ActionResult<DosadorJoinUsuarioDosadorViewModel>> RemoveDosador([FromQuery] Guid idDosador)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();
        
        var dosadorRemoved =
            await _dosadorService.RemoveDosadorFromUser(ctx.RequiredIdUser, idDosador);
        
        return Ok(dosadorRemoved);
    }

    [HttpPut()]
    [Authorize]
    public async Task<ActionResult<DosadorJoinUsuarioDosadorViewModel>> UpdateDosador([FromQuery] Guid idDosador, 
            [FromBody] PutDosadorNameViewModel putDosadorViewModel)
    {
        var putDosador = _map.Map<UpdateDosadorModel>(putDosadorViewModel);

        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        var dosadorUpdated = 
            await _dosadorService.UpdateDosador(ctx.RequiredIdUser, idDosador, putDosador);

        if (dosadorUpdated is null)
            return NotFound();
        
        return Ok(dosadorUpdated);
    }
}
