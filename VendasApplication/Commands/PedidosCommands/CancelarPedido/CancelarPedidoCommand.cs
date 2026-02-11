using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.PedidosCommands.CancelarPedidoMotivo;

public sealed class CancelarPedidoCommand
{
    public Guid PedidoId { get; }
    public string CodigoMotivo { get; }

    public CancelarPedidoCommand(Guid pedidoId, string codigoMotivo)
    {
        PedidoId = pedidoId;
        CodigoMotivo = codigoMotivo;
    }
}
