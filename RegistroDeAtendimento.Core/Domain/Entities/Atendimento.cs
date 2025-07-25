using System.Text.Json.Serialization;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Core.Domain.Exceptions;

namespace RegistroDeAtendimento.Core.Domain.Entities;

public class Atendimento : Entity{
    [JsonIgnore]
    public Paciente Paciente{ get; private set; }
    public Guid PacienteId{ get; set; }
    public DateTime DataHora{ get; private set; }
    public string Descricao{ get; private set; }
    
    public Atendimento(){ }

    public Atendimento(Paciente paciente, DateTime dataHora, string descricao, StatusEnum status){
        if (dataHora > DateTime.Now)
            throw new DomainException("A data e hora do atendimento não pode estar no futuro.");

        Paciente = paciente;
        DataHora = dataHora;
        Descricao = descricao;
        AlterarStatus(status);
    }

    public void AtualizarDados(Paciente paciente, DateTime dataHora, string descricao){
        if (dataHora > DateTime.Now)
            throw new DomainException("A data e hora do atendimento não pode estar no futuro.");

        Paciente = paciente;
        DataHora = dataHora;
        Descricao = descricao;
    }
}