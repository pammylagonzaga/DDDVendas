using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Integration.Cliente;

public interface IClientesGateway
{
    Task<EnderecoDto?> ObterEnderecoAsync(
        Guid clienteId,
        Guid enderecoId,
        CancellationToken cancellationToken = default);
}
