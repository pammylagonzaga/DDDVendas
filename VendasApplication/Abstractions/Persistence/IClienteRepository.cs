using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.Entities;
using Vendas.Domain.Clientes.Entities;

namespace Vendas.Application.Abstractions.Persistence;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(
    Guid clienteId,
    CancellationToken cancellationToken = default);
    Task AdicionarAsync(
        Cliente cliente,
        CancellationToken cancellationToken = default);

    Task AtualizarAsync(
        Cliente cliente,
        CancellationToken cancellationToken = default);
}
