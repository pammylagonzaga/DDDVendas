using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.AtivarCategoria;

public sealed class AtivarCategoriaCommandHandler
{
    private readonly ICategoriaRepository _categoriaRepository;

    public AtivarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<AtivarCategoriaResultDto> HandleAsync(
        AtivarCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(
            command.CategoriaId, cancellationToken)
            ?? throw new Exception("Categoria não encontrada.");

        categoria.Ativar();

        await _categoriaRepository.AtualizarAsync(categoria, cancellationToken);
        return new AtivarCategoriaResultDto
        {
            CategoriaId = categoria.Id,
            Ativa = categoria.Ativa
        };
    }
}
