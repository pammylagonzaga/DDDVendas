using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarCategoriaDoProduto;

public sealed class AlterarCategoriaDoProdutoCommandHandler
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public AlterarCategoriaDoProdutoCommandHandler(
        IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<AlterarCategoriaDoProdutoResultDto> HandleAsync(
        AlterarCategoriaDoProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(
            command.NovaCategoriaId, cancellationToken)
            ?? throw new DomainException("Categoria não encontrada.");

        Guard.Against<DomainException>(
            !categoria.Ativa,
            "Não é possível atribuir uma categoria inativa a um produto.");

        var produto = await _produtoRepository.ObterPorIdAsync(
            command.ProdutoId, cancellationToken)
            ?? throw new DomainException("Produto não encontrado.");

        produto.AlterarCategoria(command.NovaCategoriaId);

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AlterarCategoriaDoProdutoResultDto
        {
            ProdutoId = produto.Id,
            CategoriaId = categoria.Id
        };
    }
}
