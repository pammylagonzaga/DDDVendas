using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.Pedidos.AdicionarItemAoPedido;

public sealed class AdicionarItemAoPedidoResultDto
    {
    public Guid PedidoId { get; }
    public decimal ValorTotal { get; }
    public string StatusPedido { get; }
    public AdicionarItemAoPedidoResultDto(
        Guid pedidoId,
        decimal valorTotal,
        string statusPedido)
    {
        PedidoId = pedidoId;
        ValorTotal = valorTotal;
        StatusPedido = statusPedido;
    }
}
