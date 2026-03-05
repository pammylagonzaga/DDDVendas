using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.PedidosCommands.CriarPedidos;

public sealed class CriarPedidoCommand
{
    public Guid ClienteId { get; }
    public Guid EnderecoId { get; }

    public CriarPedidoCommand(
        Guid clienteId,
        Guid enderecoId)
    {
        ClienteId = clienteId;
        EnderecoId = enderecoId;
    }
}
