using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Interfaces;

namespace Vendas.Domain.Common.Base;

public abstract record class DomainEventBase : IDomainEvent
{
    public DateTime DateOcccured { get; protected set; } = DateTime.UtcNow;
}
