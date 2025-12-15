using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Events;

public sealed record class PedidoEntregueEvent (
    Guid PedidoId,
    Guid ClientId) : DomainEventBase;
