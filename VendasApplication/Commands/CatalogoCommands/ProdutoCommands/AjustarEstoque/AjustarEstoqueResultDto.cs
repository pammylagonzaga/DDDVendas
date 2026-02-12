using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AjustarEstoque;

public sealed class AjustarEstoqueResultDto
{
    public Guid ProdutoId { get; init; }
    public int EstoqueAtualizado { get; init; }
    public string MotivoAjuste { get; init; } = string.Empty;
}
