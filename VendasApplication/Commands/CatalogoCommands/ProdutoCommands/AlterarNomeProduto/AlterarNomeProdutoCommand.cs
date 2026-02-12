using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.ValueObjects;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarNomeProduto;

public sealed class AlterarNomeProdutoCommand
{
    public Guid ProdutoId { get; }
    public NomeProduto NovoNome { get; }

    public AlterarNomeProdutoCommand(Guid produtoId, NomeProduto novoNome)
    {
        ProdutoId = produtoId;
        NovoNome = novoNome;
    }
}
