using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Pedidos;


namespace Vendas.Application.Commands.PedidosCommands.CriarPedidos;

public sealed class CriarPedidoCommandHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public CriarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CriarPedidoResultDto> HandleAsync(
        CriarPedidoCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1 . Criar o agregado Pedido atraves do dominio
        var pedido = Pedido.Criar(
            command.ClienteId,
            command.EnderecoEntrega);

        //2. Persistir o pedido
        await _pedidoRepository.AdicionarAsync(pedido, cancellationToken);

        //3. Retornar o resultado do caso de uso
        return new CriarPedidoResultDto(
            pedido.Id,
            pedido.NumeroPedido,
            pedido.DataCriacao,
            pedido.ValorTotal,
            pedido.StatusPedido.ToString()
            );
    }
}
