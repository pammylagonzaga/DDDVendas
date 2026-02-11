using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.CriarProduto;

public sealed class CriarProdutoCommand
{
    public string Nome { get; }
    public string Codigo { get; }
    public decimal Preco { get; }
    public Guid CategoriaId { get; }
    public int EstoqueInicial { get; }
    public string? Descrição { get; }

    public CriarProdutoCommand(string nome, string codigo, decimal preco, Guid categoriaId,
        int estoqueInicial = 0, string? descrição = null)
    {
        Nome = nome;
        Codigo = codigo;
        Preco = preco;
        CategoriaId = categoriaId;
        EstoqueInicial = estoqueInicial;
        Descrição = descrição;
    }
}
