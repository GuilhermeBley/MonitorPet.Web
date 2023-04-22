using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Ui.Shared.Model.Agendamento;

namespace MonitorPet.Ui.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgendamentoController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;
    private readonly IMapper _map;

    public AgendamentoController(IAgendamentoService agendamentoService, IMapper map)
    {
        _agendamentoService = agendamentoService;
        _map = map;
    }

    [HttpGet("dosador/{idDosador}")]
    public async Task<ActionResult<AgendamentoViewModel>> GetAgendamentos(Guid idDosador)
    {
        var agendamentos = await _agendamentoService.GetByDosador(idDosador.ToString());

        if (!agendamentos.Any())
            return NoContent();

        return Ok(
            agendamentos.Select(a => _map.Map<AgendamentoViewModel>(a)));
    }

    [HttpPost]
    public async Task<ActionResult<AgendamentoViewModel>> PostNewAgendamento([FromBody] CreateAgendamentoViewModel createAgendamentoViewModel)
    {
        var agendamentoMapped = _map.Map<CreateAgendamentoModel>(createAgendamentoViewModel);

        var agendamentoAdded = await _agendamentoService.Create(agendamentoMapped);

        return Created(
            $"api/agendamento/{agendamentoAdded.Id}",
            _map.Map<AgendamentoViewModel>(agendamentoAdded)
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AgendamentoViewModel>> PostNewAgendamento(int id)
    {
        var agendamentoDeleted = await _agendamentoService.DeleteById(id);

        return Ok(
            _map.Map<AgendamentoViewModel>(agendamentoDeleted)
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AgendamentoViewModel>> PutAgendamento(int id, [FromBody] UpdateAgendamentoModel updateAgendamentoModel)
    {
        var agendamentoMapped = _map.Map<UpdateAgendamentoModel>(updateAgendamentoModel);

        var agendamentoUpdated = await _agendamentoService.UpdateById(id, agendamentoMapped);

        return Ok(
            _map.Map<AgendamentoViewModel>(agendamentoUpdated)
        );
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<AgendamentoViewModel>> PatchAgendamento(int id, [FromBody] PatchAgendamentoViewModel patchAgendamentoModel)
    {
        var agendamentoToPatch = await _agendamentoService.GetById(id);
        var agendamentoToUpdate = new UpdateAgendamentoModel
        {
            Ativado = patchAgendamentoModel.Ativado is not null ? patchAgendamentoModel.Ativado.Value : agendamentoToPatch.Ativado,
            HoraAgendada = patchAgendamentoModel.HoraAgendada is not null ? patchAgendamentoModel.HoraAgendada.Value : agendamentoToPatch.HoraAgendada,
            QtdeLiberadaGr = patchAgendamentoModel.QtdeLiberadaGr is not null ? patchAgendamentoModel.QtdeLiberadaGr.Value : agendamentoToPatch.QtdeLiberadaGr,
            DiaSemana = patchAgendamentoModel.DiaSemana is not null ? patchAgendamentoModel.DiaSemana.Value : agendamentoToPatch.DiaSemana
        };

        var agendamentoMapped = _map.Map<UpdateAgendamentoModel>(agendamentoToUpdate);

        var agendamentoUpdated = await _agendamentoService.UpdateById(id, agendamentoMapped);

        return Ok(
            _map.Map<AgendamentoViewModel>(agendamentoUpdated)
        );
    }
}