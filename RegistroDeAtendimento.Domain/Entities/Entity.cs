namespace RegistroDeAtendimento.Domain.Entities;

public abstract class Entity{
    public Guid Id{ get; private set; } = Guid.CreateVersion7();
}