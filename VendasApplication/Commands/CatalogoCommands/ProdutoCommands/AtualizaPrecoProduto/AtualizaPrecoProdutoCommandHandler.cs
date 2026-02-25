using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalogo.ValueObjects;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtualizaPrecoProduto;

public sealed class AtualizaPrecoProdutoCommandHandler
{
    private readonly IProdutoRepository _produtoRepository;

    public AtualizaPrecoProdutoCommandHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<AtualizaPrecoProdutoResultDto> HandleAsync(
        AtualizaPrecoProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(
            command.ProdutoId, cancellationToken)
            ?? throw new Exception("Produto não encontrado.");

        produto.AlterarPreco(new PrecoProduto(command.NovoPreco));

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AtualizaPrecoProdutoResultDto
        {
            ProdutoId = produto.Id,
            NovoPreco = produto.Preco.Valor
        };
    }
}
