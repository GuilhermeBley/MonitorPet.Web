﻿using MonitorPet.Application.Model.PesoHistorico;

namespace MonitorPet.Application.Repositories;

internal interface IWeightHistoryRepository
{
    Task<WeightHistoryModel?> GetByIdOrDefault(long id);
    IAsyncEnumerable<WeightHistoryModel> GetByDosador(Guid idDosador);
    IAsyncEnumerable<WeightHistoryModel> GetByDosadorAndInterval(Guid idDosador, DateTimeOffset start, DateTimeOffset? end = null);
}
