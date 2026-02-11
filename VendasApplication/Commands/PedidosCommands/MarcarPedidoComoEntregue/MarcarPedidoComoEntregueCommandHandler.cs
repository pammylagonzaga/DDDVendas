using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;

public sealed class MarcarPedidoComoEntregueCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public MarcarPedidoComoEntregueCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<MarcarPedidoComoEntregueResultDto> HandleAsync(
                        MarcarPedidoComoEntregueCommand command,
                        CancellationToken cancellationToken = default)
    {
        //1. Obter o pedido do repositorio
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken)
                       ?? throw new DomainException("Pedido não encontrado.");

        //2. Marcar o pedido como entregue atraves do dominio
        pedido.MarcarComoEntregue();

        //3. Persistir a alteracao do pedido
        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        //4. Retornar o resultado do caso de uso
        return new MarcarPedidoComoEntregueResultDto
        {
            PedidoId = pedido.Id,
            StatusPedido = pedido.StatusPedido.ToString(),

        };
    }
}
