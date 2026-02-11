using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;

public sealed class MarcarPedidoComoEntregueResultDto
{
    public Guid PedidoId { get; init; }
    public string StatusPedido { get; init; } = string.Empty;
}
