using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.InativarCategoria;

public sealed class InativarCategoriaCommand
{
    public Guid CategoriaId { get; }
    public InativarCategoriaCommand(Guid categoriaId)
    {
        CategoriaId = categoriaId;
    }
}
