﻿using MonitorPet.Application.Model.PesoHistorico;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Security;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Application.UoW;

namespace MonitorPet.Application.Services.Implementation;

public class ConsumptionService : IConsumptionService
{
	private readonly ContextClaim _contextClaim;
	private readonly IUsuarioDosadorRepository _usuarioDosadorRepository;
	private readonly IWeightHistoryRepository _weightHistoryRepository;
	private readonly IUnitOfWork _uoW;

	public ConsumptionService(
        ContextClaim contextClaim,
        IUsuarioDosadorRepository usuarioDosadorRepository, 
		IWeightHistoryRepository weightHistoryRepository,
		IUnitOfWork uoW)
	{
		_contextClaim = contextClaim;
        _usuarioDosadorRepository = usuarioDosadorRepository;
		_weightHistoryRepository = weightHistoryRepository;
		_uoW = uoW;
	}

	public async Task<ConsumptionIntervalModel> GetDaily(Guid idDosador, DateTimeOffset start)
	{
		var context = await _contextClaim.GetRequiredCurrentClaim();

		if (start.Date > DateTime.UtcNow)
			throw new Core.Exceptions.CommonCoreException("Data de início inválida.");

		using var con = await _uoW.OpenConnectionAsync();

		await ThrowIfCannotAccessDosador(context.IdUser, idDosador);

        var end = start.AddHours(24) > DateTimeOffset.UtcNow ? DateTimeOffset.UtcNow : start.AddHours(24);

        var consumptions = await GetByInterval(idDosador, start, end, TimeSpan.FromHours(2));

        return new ConsumptionIntervalModel
        {
            Consumptions = consumptions.ToList(),
            End = end,
            GeneratedAt = DateTimeOffset.Now,
            Start = start
        };
    }

	public async Task<ConsumptionIntervalModel> GetWeekly(Guid idDosador, DateTimeOffset start)
    {
        var context = await _contextClaim.GetRequiredCurrentClaim();

        if (start.Date > DateTime.UtcNow)
            throw new Core.Exceptions.CommonCoreException("Data de início inválida.");

        using var con = await _uoW.OpenConnectionAsync();

        await ThrowIfCannotAccessDosador(context.IdUser, idDosador);

        var end = start.AddDays(7) > DateTimeOffset.Now ? DateTimeOffset.Now : start.AddDays(7);

        var consumptions = await GetByInterval(idDosador, start, end, TimeSpan.FromDays(1));

        return new ConsumptionIntervalModel
        {
            Consumptions = consumptions.ToList(),
            End = end,
            GeneratedAt = DateTimeOffset.Now,
            Start = start
        };
    }

    /// <summary>
    /// Needs a connection
    /// </summary>
    private async Task ThrowIfCannotAccessDosador(int idUser, Guid idDosador)
    {
        var dosadorFound =
            await _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador);

        if (dosadorFound is null)
            throw new Core.Exceptions.ForbiddenCoreException("Usuário não tem acesso à dosador.");
    }

	/// <summary>
	/// Needs a connection
	/// </summary>
	private async Task<IEnumerable<ConsumptionModel>> GetByInterval(
        Guid idDosador, DateTimeOffset start, DateTimeOffset end, TimeSpan? interval = null, CancellationToken cancellationToken = default)
    {
        if (interval is null)
            interval = TimeSpan.FromHours(1);

        List<ConsumptionModel> consumptions = new();
        WeightHistoryModel? lastChecked = null;
        
        for (;start < end; start = start.Add(interval.Value))
        {
            ConsumptionModel consumption = new (){ Start = start, End = start.Add(interval.Value) };

            await foreach (var currentWeight in _weightHistoryRepository
                .GetByDosadorAndInterval(idDosador, start, start.Add(interval.Value)).WithCancellation(cancellationToken))
            {
                try
                {
                    if (lastChecked is null)
                        continue;

                    if (currentWeight.Weight < lastChecked.Weight)
                    {
                        consumption.QttConsumption += (lastChecked.Weight - currentWeight.Weight);
                        continue;
                    }
                }
                finally
                {
                    lastChecked = currentWeight;
                }
            }

            consumptions.Add(consumption);
        }

        return consumptions;
    }
}
