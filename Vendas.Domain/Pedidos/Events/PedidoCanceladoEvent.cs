using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Events;
public sealed record PedidoCanceladoEvent(
    Guid PedidoId,
    Guid ClientId,
    StatusPedido StatusAnterior,
    MotivoCancelamento Motivo,
    Guid? PagamentoId) : DomainEventBase;
