using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;

namespace RegistroDeAtendimento.Shared.Application.Dtos;

public class ListarPacientesDto{
    public string? Nome { get; set; } = null;
    public string? Cpf { get; set; } = null;
    public int Page{ get; set; } = ConfigurationResponse.DefaultCurrentPage;
    public int PageSize{ get; set; } = ConfigurationResponse.DefaultPageSize;
    public OrderByPacienteEnum? OrderBy { get; set; }
    public SortDirectionEnum? Sort { get; set; } 
    public StatusEnum? Status{ get; set; }
}