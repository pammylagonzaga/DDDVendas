using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.Pedidos.MarcarPedidoComoEntregue;

public sealed class MarcarPedidoComoEnviadoResultDto
{
    public Guid PedidoId { get; init; }
    public string StatusPedido { get; init; } = string.Empty;
}
