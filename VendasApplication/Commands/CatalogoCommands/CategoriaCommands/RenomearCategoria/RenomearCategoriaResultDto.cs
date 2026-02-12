using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.RenomearCategoria;

public sealed class RenomearCategoriaResultDto
{
    public Guid CategoriaId { get; init; }
    public string NovoNome { get; init; } = null!;
}
