using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Catalogo;

public sealed class CatalogoAcl
{
    private readonly ICatalogoGateway _gateway;

    public CatalogoAcl(ICatalogoGateway gateway)
    {
        Guard.AgainstNull(gateway, nameof(gateway));
        _gateway = gateway;
    }

    public async Task<ProdutoSnapshot> ObterProdutoSnapshotAsync(
        Guid produtoId, CancellationToken cancellationToken = default)
    {
        var dto = await _gateway.ObterProdutoPorIdAsync(produtoId, cancellationToken);

        if (dto == null)
            throw new DomainException("Produto não encontrado no catálogo.");

        return new ProdutoSnapshot(dto.Id, dto.Nome, dto.Preco);
    }

    public async Task ValidarEstoqueAsync(Guid produtoId, int quantidade, CancellationToken ct = default)
    {
        var possuiEstoque = await _gateway.PossuiEstoqueDisponivelAsync(produtoId, quantidade, ct);

        if (!possuiEstoque)
            throw new DomainException("Estoque insuficiente para o produto.");
    }

}
