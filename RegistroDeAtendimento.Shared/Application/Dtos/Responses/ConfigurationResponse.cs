using RegistroDeAtendimento.Core.Domain.Enums;

namespace RegistroDeAtendimento.Shared.Application.Dtos.Responses;

public static class ConfigurationResponse{
    public const int DefaultCurrentPage = 1;
    public const int DefaultPageSize = 50;
    public const int DefaultStatusCode = 200;
    public const OrderByPacienteEnum DefaultOrderBy = OrderByPacienteEnum.Id;
    public const SortDirectionEnum DefaultDirection = SortDirectionEnum.Asc;
    public const StatusEnum DefaultStatus = StatusEnum.Ativo;
}