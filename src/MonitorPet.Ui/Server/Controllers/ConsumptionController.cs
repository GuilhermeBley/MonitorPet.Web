using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Ui.Shared.Model.PesoHistorico;

namespace MonitorPet.Ui.Server.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConsumptionController : Controller
{
    private readonly IConsumptionService _consumptionService;
    private readonly IMapper _map;

    public ConsumptionController(IConsumptionService consumptionService, IMapper map)
    {
        _consumptionService = consumptionService;
        _map = map;
    }

    [HttpGet("day/{idDosador}/{start}")]
    public async Task<ActionResult<ConsumptionIntervalViewModel>> GetConsumptionDay(Guid idDosador, DateTimeOffset start)
    {
        var consumptions = await _consumptionService.GetDaily(idDosador, start);

        return Ok(_map.Map<ConsumptionIntervalViewModel>(consumptions));
    }

    [HttpGet("week/{idDosador}/{start}")]
    public async Task<ActionResult<ConsumptionIntervalViewModel>> GetConsumptionWeek(Guid idDosador, DateTimeOffset start)
    {
        var consumptions = await _consumptionService.GetWeekly(idDosador, start);

        return Ok(_map.Map<ConsumptionIntervalViewModel>(consumptions));
    }
}
