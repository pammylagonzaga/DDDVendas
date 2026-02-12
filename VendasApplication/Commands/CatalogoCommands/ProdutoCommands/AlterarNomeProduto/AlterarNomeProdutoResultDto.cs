using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.ValueObjects;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarNomeProduto;

public sealed class AlterarNomeProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public string NovoNome { get; init; } = null!;
}
