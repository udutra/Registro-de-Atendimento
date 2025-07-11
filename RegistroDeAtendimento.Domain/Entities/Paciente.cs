using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace RegistroDeAtendimento.Domain.Entities;

public class Paciente(string nome, DateTime dataNascimento, string cpf, SexoEnum sexo, Endereco endereco, StatusEnum status) : Entity{
    public string Nome{ get; private set; } = nome;
    public DateTime DataNascimento{ get; private set; } = dataNascimento;
    public string Cpf{ get; private set; } = cpf;
    public SexoEnum Sexo{ get; private set; } = sexo;
    public Endereco Endereco{ get; private set; } = endereco;
    public StatusEnum Status{ get; private set; } = status;
}