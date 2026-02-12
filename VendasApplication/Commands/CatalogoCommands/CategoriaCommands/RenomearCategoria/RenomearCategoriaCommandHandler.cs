using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.RenomearCategoria;

public sealed class RenomearCategoriaCommandHandler
{
    private readonly ICategoriaRepository _categoriaRepository;

    public RenomearCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task <RenomearCategoriaResultDto> HandleAsync(
        RenomearCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.CategoriaId, cancellationToken)
            ?? throw new Exception("Categoria não encontrada.");

        categoria.AlterarNome(command.NovoNome);

        await _categoriaRepository.AtualizarAsync(categoria, cancellationToken);

        return new RenomearCategoriaResultDto
        {
            CategoriaId = categoria.Id,
            NovoNome = categoria.Nome
        };
    }
}
