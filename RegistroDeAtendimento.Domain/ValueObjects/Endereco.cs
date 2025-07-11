namespace RegistroDeAtendimento.Domain.ValueObjects;

public class Endereco(string cep, string cidade, string bairro, string logradouro, string? complemento){
    public string Cep{ get; private set; } = cep;
    public string Cidade{ get; private set; } = cidade;
    public string Bairro{ get; private set; } = bairro;
    public string Logradouro { get; private set; } = logradouro;
    public string? Complemento{ get; private set; } = complemento;
    
    public override bool Equals(object? obj) =>
        Equals(obj as Endereco);

    public bool Equals(Endereco? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Cep == other.Cep &&
               Cidade == other.Cidade &&
               Bairro == other.Bairro &&
               Logradouro == other.Logradouro &&
               Complemento == other.Complemento;
    }

    public override int GetHashCode(){
        return HashCode.Combine(Cep, Cidade, Bairro, Logradouro, Complemento);
    }

    public static bool operator ==(Endereco? a, Endereco? b) => Equals(a, b);
    public static bool operator !=(Endereco? a, Endereco? b) => !Equals(a, b);
}