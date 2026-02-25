using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Integration.Catalogo;

public interface ICatalogoGateway
{
    Task<ProdutoDto?> ObterProdutoPorIdAsync(Guid produtoId, CancellationToken ct = default);

    Task <bool> PossuiEstoqueDisponivelAsync(Guid produtoId, int quantidade, CancellationToken ct = default);
}
