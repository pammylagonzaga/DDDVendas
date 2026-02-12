using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.Enums;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtivarProduto;

public sealed class AtivarProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public bool Ativa { get; init; }
}
