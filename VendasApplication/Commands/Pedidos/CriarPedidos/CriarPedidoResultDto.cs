using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.Pedidos.CriarPedidos;

public sealed class CriarPedidoResultDto
{
    public Guid PedidoId { get; }
    public string NumeroPedido { get; }
    public DateTime DataCriacao { get; }
    public decimal ValorTotal { get; }
    public string StatusPedido { get; }
    public CriarPedidoResultDto(
        Guid pedidoId,
        string numeroPedido,
        DateTime dataCriacao,
        decimal valorTotal,
        string statusPedido)
    {
        PedidoId = pedidoId;
        NumeroPedido = numeroPedido;
        DataCriacao = dataCriacao;
        ValorTotal = valorTotal;
        StatusPedido = statusPedido;
    }
}
