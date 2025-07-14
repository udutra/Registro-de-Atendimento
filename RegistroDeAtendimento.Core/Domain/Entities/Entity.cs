using RegistroDeAtendimento.Core.Domain.Enums;

namespace RegistroDeAtendimento.Core.Domain.Entities;

public abstract class Entity{
    public Guid Id{ get; private set; } = Guid.CreateVersion7();
    public StatusEnum Status{ get; private set; }
    
    public void AlterarStatus(StatusEnum status){
        Status = status;
    }
    
}