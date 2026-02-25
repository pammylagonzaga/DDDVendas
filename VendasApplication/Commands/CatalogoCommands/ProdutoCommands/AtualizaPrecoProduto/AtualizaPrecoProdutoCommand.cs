using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.ValueObjects;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtualizaPrecoProduto;

public sealed class AtualizaPrecoProdutoCommand
{
    public Guid ProdutoId { get; }
    public decimal NovoPreco { get; }

    public AtualizaPrecoProdutoCommand(Guid produtoId, decimal novoPreco)
    {
        ProdutoId = produtoId;
        NovoPreco = novoPreco;
    }
}
