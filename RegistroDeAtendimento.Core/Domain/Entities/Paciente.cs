using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Core.Domain.ValueObjects;

namespace RegistroDeAtendimento.Core.Domain.Entities;

public class Paciente : Entity{
    public string Nome{ get; private set; }
    public DateOnly DataNascimento{ get; private set; }
    public string Cpf{ get; private set; }
    public SexoEnum Sexo{ get; private set; }
    public Endereco Endereco{ get; private set; }
    public List<Atendimento> Atendimentos{ get; private set; }
    
    public Paciente(){ }

    public Paciente(string nome, DateOnly dataNascimento, string cpf, SexoEnum sexo, Endereco endereco,
        StatusEnum status){
        Nome = nome;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Sexo = sexo;
        Endereco = endereco;
        AlterarStatus(status);
        Atendimentos =[];
    }

    public void AtualizarDados(string nome, DateOnly dataNascimento, string cpf, SexoEnum sexo, Endereco endereco){
        Nome = nome;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Sexo = sexo;
        Endereco = endereco;
    }
}