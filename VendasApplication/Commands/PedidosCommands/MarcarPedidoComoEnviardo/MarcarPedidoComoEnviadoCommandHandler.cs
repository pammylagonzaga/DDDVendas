using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;

public sealed class MarcarPedidoComoEnviadoCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public MarcarPedidoComoEnviadoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<MarcarPedidoComoEnviadoResultDto> HandleAsync(
                        MarcarPedidoComoEnviadoCommand command,
                        CancellationToken cancellationToken = default)
    {
        //1. Obter o pedido do repositorio
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken)
                       ?? throw new DomainException("Pedido não encontrado.");

        //2. Marcar o pedido como entregue atraves do dominio
        pedido.MarcarComoEnviado();

        //3. Persistir a alteracao do pedido
        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        //4. Retornar o resultado do caso de uso
        return new MarcarPedidoComoEnviadoResultDto
        {
            PedidoId = pedido.Id,
            StatusPedido = pedido.StatusPedido.ToString(),

        };
    }
}
