using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Events;

public interface IDomainEvent
{
    DateTime DateOcccured { get; }

}
