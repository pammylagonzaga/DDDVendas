using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Integration.Catalogo;

public sealed class ProdutoDto
{
    public Guid Id { get; }
    public string Nome { get; }
    public decimal Preco { get; }

    public ProdutoDto(Guid id, string nome, decimal preco)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }

}
