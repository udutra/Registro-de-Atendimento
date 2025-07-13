using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Dtos;

public class CriarAtendimentoDto{
    public Guid PacienteId{ get; set; }
    public DateTime DataHora{ get; set; }
    public string Descricao{ get; set; }
    public StatusEnum Status{ get; set; }
}