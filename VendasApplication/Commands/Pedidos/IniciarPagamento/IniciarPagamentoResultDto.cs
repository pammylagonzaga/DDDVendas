using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.Pedidos.IniciarPagamento;

public sealed class IniciarPagamentoResultDto
{

    public Guid PedidoId { get; init; }
    public Guid PagamentoId { get; init; }
    public string StatusPedido { get; init; } = string.Empty;
    public string StatusPagamento { get; init; } = string.Empty;

}
