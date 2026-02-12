using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.InativarProduto;

public sealed class InativarProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public bool Inativar { get; init; }
}
