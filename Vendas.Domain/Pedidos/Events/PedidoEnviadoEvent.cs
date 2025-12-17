using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Events;
public sealed record PedidoEnviadoEvent (
    Guid PedidoId,
    Guid ClientId,
    EnderecoEntrega EnderecoEntrega) : DomainEventBase;
