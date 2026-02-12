using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.AtivarCategoria;

public sealed class AtivarCategoriaCommand
{
    public Guid CategoriaId { get; }
    public AtivarCategoriaCommand(Guid categoriaId)

    {
        CategoriaId = categoriaId;
    }
}
