using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.PedidosCommands.RemoverItemDoPedido;

public sealed class RemoverItemDoPedidoCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public RemoverItemDoPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task <RemoverItemDoPedidoResultDto> HandleAsync(
        RemoverItemDoPedidoCommand command,
        CancellationToken cancellationToken= default)
{
    var pedido = await _pedidoRepository
        .ObterPorIdAsync(command.PedidoId, cancellationToken);

    if (pedido is null)
        throw new InvalidOperationException("Pedido não encontrado.");
            
    pedido.RemoverItem(command.ItemId);


    await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

    return new RemoverItemDoPedidoResultDto(
        pedido.Id,
        pedido.ValorTotal,
        pedido.StatusPedido.ToString());
}
}