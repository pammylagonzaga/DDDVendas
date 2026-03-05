using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Pedidos;
using Vendas.Domain.Pedidos.Integration.Cliente;


namespace Vendas.Application.Commands.PedidosCommands.CriarPedidos;

public sealed class CriarPedidoCommandHandler
{
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClientesGateway _clientesGateway;
        private readonly ClientesAcl _clientesAcl;

    public CriarPedidoCommandHandler(
        IPedidoRepository pedidoRepository,
        IClientesGateway clientesGateway,
        ClientesAcl clientesAcl)
    {
        _pedidoRepository = pedidoRepository;
        _clientesGateway = clientesGateway;
        _clientesAcl = clientesAcl;
    }

    public async Task<CriarPedidoResultDto> HandleAsync(CriarPedidoCommand command,
        CancellationToken cancellationToken= default)
    {
        //Buscar Endereço diretamente no BC Cliente (Upstreanm)
        var enderecoDto = await _clientesGateway.ObterEnderecoAsync(command.ClienteId, command.EnderecoId, cancellationToken);
    
        if (enderecoDto is null)
            throw new InvalidOperationException("Endereço não encontrado.");

        //ACL traduz para modelo interno Snapshot do cliente
        var enderecoEntrega = _clientesAcl.TraduzirEndereco(enderecoDto);

        //Criar Aggragate
        var pedido = Pedido.Criar(command.ClienteId,enderecoEntrega);

        //Persistir o Pedido
        await _pedidoRepository.AdicionarAsync(pedido, cancellationToken);

        //Retornar resultado (Id do Pedido criado)
        return new CriarPedidoResultDto(
            pedido.Id,
            pedido.NumeroPedido,
            pedido.DataCriacao,
            pedido.ValorTotal,
            pedido.StatusPedido.ToString()
            );

    }
}
