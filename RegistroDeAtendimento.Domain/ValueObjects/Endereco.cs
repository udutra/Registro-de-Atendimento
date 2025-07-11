namespace RegistroDeAtendimento.Domain.ValueObjects;

public class Endereco(string cep, string cidade, string bairro, string logradouro, 
    string? complemento){
    public string Cep{ get; private set; } = cep;
    public string Cidade{ get; private set; } = cidade;
    public string Bairro{ get; private set; } = bairro;
    public string Logradouro { get; private set; } = logradouro;
    public string? Complemento{ get; private set; } = complemento;
}