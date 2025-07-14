namespace RegistroDeAtendimento.Shared.Application.Dtos;

public class AtualizarAtendimentoDto{
    public Guid? PacienteId{ get; set; }
    public DateTime? DataHora{ get; set; }
    public string? Descricao{ get; set; }
}