using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarCategoriaDoProduto;

public sealed class AlterarCategoriaDoProdutoCommand
{
    public Guid ProdutoId { get; }
    public Guid NovaCategoriaId { get; }

    public AlterarCategoriaDoProdutoCommand(Guid produtoId, Guid novaCategoriaId)
    {
        ProdutoId = produtoId;
        NovaCategoriaId = novaCategoriaId;
    }
}
