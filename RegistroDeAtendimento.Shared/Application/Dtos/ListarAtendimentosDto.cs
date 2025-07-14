using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;

namespace RegistroDeAtendimento.Shared.Application.Dtos;

public class ListarAtendimentosDto{
    public int Page { get; set; } = ConfigurationResponse.DefaultCurrentPage;
    public int PageSize { get; set; } = ConfigurationResponse.DefaultPageSize;
    public OrderByAtendimentoEnum? OrderBy { get; set; } = null;
    public SortDirectionEnum? Sort { get; set; } = null;
    public Guid? Id { get; set; } = null;
    public Guid? PacienteId { get; set; } = null;
    public string? PacienteNome { get; set; } = null;
    public string? PacienteCpf { get; set; } = null;
    public StatusEnum? Status { get; set; } = null;
    public DateTime? DataInicio { get; set; } = null;
    public DateTime? DataFim { get; set; } = null;
}