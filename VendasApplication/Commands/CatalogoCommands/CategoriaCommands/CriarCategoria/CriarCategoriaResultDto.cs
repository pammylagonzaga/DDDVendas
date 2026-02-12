using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.CriarCategoria;

public sealed class CriarCategoriaResultDto
{
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public bool Ativa { get; init; }

}
