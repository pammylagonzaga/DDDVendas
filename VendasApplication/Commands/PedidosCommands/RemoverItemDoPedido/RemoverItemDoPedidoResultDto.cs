using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.PedidosCommands.RemoverItemDoPedido;
public sealed class RemoverItemDoPedidoResultDto
{
    public Guid PedidoId { get; }
    public decimal ValorTotal { get; }
    public string Status { get; }

    public RemoverItemDoPedidoResultDto(
        Guid pedidoId,
        decimal valorTotal,
        string status)
    {
        PedidoId = pedidoId;
        ValorTotal = valorTotal;
        Status = status;
    }
}
