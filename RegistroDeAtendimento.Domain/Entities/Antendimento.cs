using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Domain.Entities;

public class Antendimento(Paciente paciente, DateTime dataHora, string descricao, 
    StatusEnum status) : Entity{
    public Paciente Paciente { get; private set; } = paciente;
    public DateTime DataHora { get; private set; } = dataHora;
    public string Descricao { get; private set; } = descricao;
    public StatusEnum Status{ get; private set; } = status;
} 