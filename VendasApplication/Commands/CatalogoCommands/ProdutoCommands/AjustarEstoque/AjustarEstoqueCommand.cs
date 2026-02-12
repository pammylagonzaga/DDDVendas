using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AjustarEstoque;

public sealed class AjustarEstoqueCommand
{
    public Guid ProdutoId { get; }
    public int QuantidadeAjustada { get; }
    public string Motivo { get; }
    public AjustarEstoqueCommand(Guid produtoId, int quantidadeAjustada, string motivo)
    {
        ProdutoId = produtoId;
        QuantidadeAjustada = quantidadeAjustada;
        Motivo = motivo;
    }
}
