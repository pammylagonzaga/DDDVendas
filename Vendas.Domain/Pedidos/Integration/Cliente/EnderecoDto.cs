using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Integration.Cliente;

public sealed class EnderecoDto
{
    public Guid Id { get; }
    public string Cep { get; } 
    public string Logradouro { get; } 
    public string Numero { get; } 
    public string Bairro { get; } 
    public string Cidade { get; } 
    public string Estado { get; } 
    public string Pais { get; }
    public string Complemento { get;  } 

    public EnderecoDto(Guid id, string cep, string logradouro, string numero, string bairro, string cidade, string estado, string pais, string complemento)
    {
        Id = id;
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
        Complemento = complemento?? string.Empty;
    }
}
