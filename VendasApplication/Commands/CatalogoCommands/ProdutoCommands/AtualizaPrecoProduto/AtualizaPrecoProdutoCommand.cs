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
    public PrecoProduto NovoPreco { get; }
    public decimal PrecoAntigo { get; }

    public AtualizaPrecoProdutoCommand(Guid produtoId, PrecoProduto novoPreco, decimal precoAntigo)
    {
        ProdutoId = produtoId;
        NovoPreco = novoPreco;
        PrecoAntigo = precoAntigo;
    }
}
