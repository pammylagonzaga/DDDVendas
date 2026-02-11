using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.PedidosCommands.CancelarPedidoMotivo;

public sealed class CancelarPedidoCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public CancelarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CancelarPedidoResultDto> HandleAsync(
                        CancelarPedidoCommand command,
                        CancellationToken cancellationToken = default)
    {
        //1. Obter o pedido do repositorio
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken)
                       ?? throw new DomainException("Pedido não encontrado.");

        var motivo = new MotivoCancelamento(command.CodigoMotivo);

        //2. Cancelar o pedido atraves do dominio
        pedido.CancelarPedido(motivo);

        //3. Persistir a alteracao do pedido
        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        //4. Retornar o resultado do caso de uso
        return new CancelarPedidoResultDto
        {
            PedidoId = pedido.Id,
            Status = pedido.StatusPedido.ToString(),
        };

    }
}

