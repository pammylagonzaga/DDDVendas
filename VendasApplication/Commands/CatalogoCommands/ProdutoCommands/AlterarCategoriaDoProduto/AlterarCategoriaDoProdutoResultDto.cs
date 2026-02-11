using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarCategoriaDoProduto
{
    public class AlterarCategoriaDoProdutoResultDto
    {
        public Guid ProdutoId { get; init; }
        public Guid CategoriaId { get; init; }
    }
}