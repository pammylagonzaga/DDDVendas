using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalogo;
using Vendas.Domain.Catalogo.ValueObjects;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.CriarProduto;

public sealed class CriarProdutoCommandHandler
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRespository;

    public CriarProdutoCommandHandler(
        IProdutoRepository produtoRepository, 
        ICategoriaRepository categoriaRespository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRespository = categoriaRespository;
    }

    public async Task<CriarProdutoResultDto> HandleAsync(
        CriarProdutoCommand command, CancellationToken cancellationToken = default)
    {
        //1° Validação cruzada entre 
        var categoria = await _categoriaRespository.ObterPorIdAsync(
            command.CategoriaId, cancellationToken)
            ??throw new DomainException("Categoria não encontrada.");

        Guard.Against<DomainException>(
            !categoria.Ativa,
            "Não é possível criar um produto em uma categoria inativa.");

        //2° Tradução de tipos primitivos para value object
        var nome = new NomeProduto(command.Nome);
        var codigo = new CodigoProduto(command.Codigo);
        var preco = new PrecoProduto(command.Preco);

        //3° Criação do Agreate Root Produto
        var produto = new Produto(
            nome,
            codigo,
            preco,
            command.CategoriaId,
            command.EstoqueInicial,
            command.Descrição
            );

        //4° Persistência do Produto
        await _produtoRepository.AdicionarAsync(produto, cancellationToken);

        return new CriarProdutoResultDto
        {
            ProdutoId = produto.Id,
            Nome = produto.Nome.Valor,
            Preco = produto.Preco.Valor,
            Status = produto.Status.ToString(),
        };
    }
}
