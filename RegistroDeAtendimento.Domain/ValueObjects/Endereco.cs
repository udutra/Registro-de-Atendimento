namespace RegistroDeAtendimento.Domain.ValueObjects;

public class Endereco(string cep, string cidade, string bairro, string logradouro, string? complemento){
    public string Cep{ get; } = cep;
    public string Cidade{ get; } = cidade;
    public string Bairro{ get; } = bairro;
    public string Logradouro{ get; } = logradouro;
    public string? Complemento{ get; } = complemento;

    public override bool Equals(object? obj){
        return Equals(obj as Endereco);
    }

    public bool Equals(Endereco? other){
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

    public static bool operator ==(Endereco? a, Endereco? b){
        return Equals(a, b);
    }

    public static bool operator !=(Endereco? a, Endereco? b){
        return !Equals(a, b);
    }
}