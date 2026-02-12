using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.InativarProduto;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtivarProduto;

public sealed class InativarProdutoCommandHandler
{
    private readonly IProdutoRepository _produtoRepository;

    public InativarProdutoCommandHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<InativarProdutoResultDto> HandlerAsync(
        InativarProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken)
            ?? throw new DomainException("Produto não encontrado.");

        produto.Inativar();

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new InativarProdutoResultDto
        {
            ProdutoId = produto.Id,
            Inativar = produto.Status == Domain.Catalogo.Enums.StatusProduto.Inativo
        };
    }
}
