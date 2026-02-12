using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.InativarCategoria;

public sealed class InativarCategoriaCommandHandler
{
    private readonly ICategoriaRepository _categoriaRepository;
    
        public InativarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
    
        public async Task<InativarCategoriaResultDto> HandleAsync(
            InativarCategoriaCommand command, CancellationToken cancellationToken = default)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(
                command.CategoriaId, cancellationToken)
                ?? throw new DomainException("Categoria não encontrada.");
    
            categoria.Inativar();
    
            await _categoriaRepository.AtualizarAsync(categoria, cancellationToken);
    
            return new InativarCategoriaResultDto
            {
                CategoriaId = categoria.Id,
                Inativa = categoria.Ativa.Equals(false)
            };
    }
}
