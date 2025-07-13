using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Dtos;

public class ListarAtendimentosDto{
    public int Page { get; set; } = ConfigurationResponse.DefaultCurrentPage;
    public int PageSize { get; set; } = ConfigurationResponse.DefaultPageSize;
    public OrderByAtendimentoEnum? OrderBy { get; set; } = null;
    public SortDirectionEnum? Sort { get; set; } = null;
    public Guid? PacienteId { get; set; } = null;
    public string? PacienteNome { get; set; } = null;
    public string? PacienteCpf { get; set; } = null;
    public StatusEnum? Status { get; set; } = null;
    public DateTime? DataInicio { get; set; } = null;
    public DateTime? DataFim { get; set; } = null;
}