using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.Enums;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.InativarProduto;

public sealed class InativarProdutoCommand
{
    public Guid ProdutoId { get; }

    public InativarProdutoCommand(Guid produtoId)
    {
        ProdutoId = produtoId;
    }
}
