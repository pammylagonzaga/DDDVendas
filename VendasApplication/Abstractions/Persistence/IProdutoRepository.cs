using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.Entities;
using Vendas.Domain.Pedidos.Entities;

namespace Vendas.Application.Abstractions.Persistence;

public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(
        Guid produtoId,
        CancellationToken cancellationToken = default);
    Task AdicionarAsync(
        Produto produto,
        CancellationToken cancellationToken = default);

    Task AtualizarAsync(
        Produto produto,
        CancellationToken cancellationToken = default);
}