using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Events;

public abstract record class DomainEventBase : IDomainEvent
{
    public DateTime DateOcccured { get; protected set; } = DateTime.UtcNow;
}
