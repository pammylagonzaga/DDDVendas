using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtivarProduto;

public sealed class AtivarProdutoCommandHandler
{
        private readonly IProdutoRepository _produtoRepository;
    
        public AtivarProdutoCommandHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
    
        public async Task<AtivarProdutoResultDto> HandlerAsync(
            AtivarProdutoCommand command, CancellationToken cancellationToken = default)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken)
                ?? throw new DomainException("Produto não encontrado.");
    
            produto.Ativar();
    
            await _produtoRepository.AtualizarAsync(produto, cancellationToken);
    
            return new AtivarProdutoResultDto
            {
                ProdutoId = produto.Id,
                Status = produto.Status.ToString()
            };
    }
}
