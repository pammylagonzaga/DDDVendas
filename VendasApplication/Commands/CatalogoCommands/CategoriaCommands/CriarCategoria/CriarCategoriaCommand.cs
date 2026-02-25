using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.CriarCategoria;

public class CriarCategoriaCommand
{
    public string Nome { get; }
    public string Descricao { get; }
    public CriarCategoriaCommand(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }

}
