using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.Pedidos.AtualizarEnderecoEntrega;

public sealed class AtualizarEnderecoEntregaCommand
{
    public Guid PedidoId { get; }
    public EnderecoEntrega NovoEnderecoEntrega { get; }

    public AtualizarEnderecoEntregaCommand(
        Guid pedidoId,
        EnderecoEntrega novoEnderecoEntrega)
    {
        PedidoId = pedidoId;
        NovoEnderecoEntrega = novoEnderecoEntrega;
    }
}
