using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.Enums;
using Vendas.Domain.Pedidos.Enums;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtivarProduto;

public sealed class AtivarProdutoCommand
{
    public Guid ProdutoId { get; }

    public AtivarProdutoCommand(Guid produtoId)
    {
        ProdutoId = produtoId;
    }
}
