using FluentAssertions;
using RegistroDeAtendimento.Domain.Entities;

namespace Domain.Tests.Entities;

public class EntityTests{
    [Fact]
    public void Nova_Entidade_Deve_Ter_Id_GuidVersion7_Valido(){
        var entidade = new EntidadeFake();
        entidade.Id.Should().NotBe(Guid.Empty);
        entidade.Id.ToString()[14].Should().Be('7');
    }

    private class EntidadeFake : Entity{ }
}