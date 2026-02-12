using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.InativarCategoria;

public sealed class InativarCategoriaResultDto
{
    public Guid CategoriaId { get; init; }
    public bool Inativa { get; init; }
}
