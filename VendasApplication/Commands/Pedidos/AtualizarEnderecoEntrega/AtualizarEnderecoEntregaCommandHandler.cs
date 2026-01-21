using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Pedidos.AtualizarEnderecoEntrega;

public sealed class AtualizarEnderecoEntregaCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public AtualizarEnderecoEntregaCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<AtualizarEnderecoEntregaResultDto> HandleAsync(
        AtualizarEnderecoEntregaCommand command, CancellationToken cancellationToken= default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new InvalidOperationException("Pedido não encontrado.");

        pedido.AtualizarEnderecoEntrega(command.NovoEnderecoEntrega);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new AtualizarEnderecoEntregaResultDto(
            pedido.Id,
            pedido.EnderecoEntrega.ToString(),
            pedido.StatusPedido.ToString());
    }
    // no retorno o EnderecoEntrega é convertido para string para evitar expor detalhes internos do Value Object, não dá erro!
}
