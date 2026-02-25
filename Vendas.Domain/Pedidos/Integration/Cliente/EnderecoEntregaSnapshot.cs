using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Cliente;

public class EnderecoEntregaSnapshot : ValueObject
{
    public string Cep { get; }
    public string Logradouro { get; }
    public string Numero { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string Pais { get; }
    public string Complemento { get; }

    public EnderecoEntregaSnapshot(string cep, string logradouro, string numero, string bairro, string cidade, string estado, string pais, string complemento)
    {
        Guard.AgainstNullorWhiteSpace(cep, nameof(cep), "CEP é obrigatório.");
        Guard.AgainstNullorWhiteSpace(logradouro, nameof(logradouro), "Logradouro é obrigatório.");
        Guard.AgainstNullorWhiteSpace(numero, nameof(numero), "Número é obrigatório.");
        Guard.AgainstNullorWhiteSpace(bairro, nameof(bairro), "Bairro é obrigatório.");
        Guard.AgainstNullorWhiteSpace(cidade, nameof(cidade), "Cidade é obrigatória.");
        Guard.AgainstNullorWhiteSpace(estado, nameof(estado), "Estado é obrigatório.");
        Guard.AgainstNullorWhiteSpace(pais, nameof(pais), "País é obrigatório.");

        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
        Complemento = complemento ?? string.Empty;

    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Logradouro;
        yield return Numero;
        yield return Bairro;
        yield return Cidade;
        yield return Estado;
        yield return Pais;
        yield return Complemento;
    }
}
