using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.RenomearCategoria;

public sealed class RenomearCategoriaCommand
{
    public Guid CategoriaId { get; }
    public string NovoNome { get; }
    public RenomearCategoriaCommand(Guid categoriaId, string novoNome)
    {
        CategoriaId = categoriaId;
        NovoNome = novoNome;
    }
}
