using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.AtivarCategoria;

public sealed class AtivarCategoriaResultDto
{
    public Guid CategoriaId { get; init; }
    public bool Ativa { get; init; }
}
