using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.CriarProduto;

public sealed class CriarProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public string Nome { get; init; } = string.Empty;
    public decimal Preco { get; init; }
    public string Status { get; init; } = string.Empty;

}