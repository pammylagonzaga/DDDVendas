using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.PedidosCommands.IniciarPagamento;

public sealed class IniciarPagamentoCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;

    public IniciarPagamentoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<IniciarPagamentoResultDto> HandlerAsync(
        IniciarPagamentoCommand command, CancellationToken cancellationToken= default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId,
            cancellationToken) ?? throw new DomainException("Pedido não encontrado.");

        var pagamento = pedido.IniciarPagamento(command.MetodoPagamento);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new IniciarPagamentoResultDto
        {
            PedidoId = pedido.Id,
            PagamentoId = pagamento.Id,
            StatusPedido = pedido.StatusPedido.ToString(),
            StatusPagamento = pagamento.StatusPagamento.ToString()
        };
    }
}
