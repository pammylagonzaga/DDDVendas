using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Vendas.Application.Commands.Pedidos.AtualizarEnderecoEntrega;

public sealed class AtualizarEnderecoEntregaResultDto
{
    public Guid PedidoId { get; }
    public string EnderecoEntrega { get; }
    public string Status { get; }

    public AtualizarEnderecoEntregaResultDto(
        Guid pedidoId,
        string enderecoEntrega,
        string status)
    {
        PedidoId = pedidoId;
        EnderecoEntrega = enderecoEntrega;
        Status = status;
    }
}