using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;

public sealed class MarcarPedidoComoEnviadoCommand
{
    public Guid PedidoId { get; }

    public MarcarPedidoComoEnviadoCommand(Guid pedidoId)
    {
        PedidoId = pedidoId;
    }
}
