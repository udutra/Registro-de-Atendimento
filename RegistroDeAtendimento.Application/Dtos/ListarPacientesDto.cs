using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Dtos;

public class ListarPacientesDto{
    public string? Nome { get; set; } = null;
    public string? Cpf { get; set; } = null;
    public int Page{ get; set; } = ConfigurationResponse.DefaultCurrentPage;
    public int PageSize{ get; set; } = ConfigurationResponse.DefaultPageSize;
    public OrderByPacienteEnum? OrderBy { get; set; }  = null;
    public SortDirectionEnum? Sort { get; set; } = null;
    public StatusEnum? Status{ get; set; } = null;
}