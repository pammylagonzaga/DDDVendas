using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarNomeProduto;

public sealed class AlterarNomeProdutoCommandHandler
{
    private readonly IProdutoRepository _produtoRepository;

    public AlterarNomeProdutoCommandHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<AlterarNomeProdutoResultDto> HandleAsync(
        AlterarNomeProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(
            command.ProdutoId, cancellationToken)
            ?? throw new DomainException("Produto não encontrado.");

        produto.AlterarNome(command.NovoNome);

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AlterarNomeProdutoResultDto
        {
            ProdutoId = produto.Id,
            NovoNome = produto.Nome.Valor
        };
    }

}
