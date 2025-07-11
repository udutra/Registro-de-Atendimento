using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Domain.Entities;

public class Atendimento : Entity {
    public Paciente Paciente { get; private set; }
    public Guid PacienteId { get; set; }
    public DateTime DataHora { get; private set; }
    public string Descricao { get; private set; }
    public StatusEnum Status{ get; private set; }


    public Atendimento(){ }

    public Atendimento(Paciente paciente, DateTime dataHora, string descricao, StatusEnum status){
        Paciente = paciente;
        DataHora = dataHora;
        Descricao = descricao;
        Status = status;
    }
} 