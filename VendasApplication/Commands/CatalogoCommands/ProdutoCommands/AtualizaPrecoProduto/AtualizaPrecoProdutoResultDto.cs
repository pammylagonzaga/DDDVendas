using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.ValueObjects;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtualizaPrecoProduto;

public sealed class AtualizaPrecoProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public decimal NovoPreco { get; init; }
    public decimal PrecoAntigo { get; init; }
}
