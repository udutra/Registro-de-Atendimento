using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Dtos.Responses;

public class AtendimentoResponseDto{
    public Guid Id { get; set; }
    public Guid PacienteId { get; set; }
    public string PacienteNome { get; set; } = string.Empty;
    public string PacienteCpf { get; set; } = string.Empty;
    public DateTime DataHora { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public StatusEnum Status { get; set; }
}