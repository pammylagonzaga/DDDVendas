using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.CriarCategoria;

public sealed class CriarCategoriaCommandHandler
{
    private readonly ICategoriaRepository _categoriaRepository;
        public CriarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }   

    public async Task<CriarCategoriaResultDto> HandleAsync(
        CriarCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = new Domain.Catalogo.Categoria(command.Nome, command.Descricao);

        await _categoriaRepository.AdicionarAsync(categoria, cancellationToken);

        return new CriarCategoriaResultDto
        {
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            Ativa = categoria.Ativa
        };
    }
}
