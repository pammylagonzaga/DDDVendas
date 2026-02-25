using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Cliente;

public sealed class ClientesAcl
{
    //injetando interface
    private readonly IClientesGateway _gateway;

    public ClientesAcl(IClientesGateway gateway)
    {
        Guard.AgainstNull(gateway, nameof(gateway));
        _gateway = gateway;
    }
    //fazendo implementação do metodo
    public async Task<EnderecoEntregaSnapshot> ObterEnderecoEntregaSnapshotAsync(
        Guid clienteId, Guid enderecoId, CancellationToken cancellationToken = default)
    {
        var dto = await _gateway.ObterEnderecoAsync(clienteId,enderecoId,cancellationToken);

        if (dto is null)
            throw new DomainException("Endereço não encontrado no contexto Cliente.");

        //fazendo conversao do dto para snapshot
        return new EnderecoEntregaSnapshot(
            dto.Cep,dto.Logradouro, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado,dto.Pais, dto.Complemento);
    }
}
