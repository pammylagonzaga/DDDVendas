using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.ValueObjects;
public class EnderecoEntrega : ValueObject
{
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Estado { get; private set; }
    public string Cidade { get; private set; }
    public string Pais { get; private set; }
    private EnderecoEntrega(string cep, string logradouro, string complemento, string bairro, string estado, string cidade, string pais)
    {
        Guard.AgainstNullorWhiteSpace(cep, nameof(Cep));
        Guard.AgainstNullorWhiteSpace(logradouro, nameof(Logradouro));
        Guard.AgainstNullorWhiteSpace(bairro, nameof(Bairro));
        Guard.AgainstNullorWhiteSpace(estado, nameof(Estado));
        Guard.AgainstNullorWhiteSpace(cidade, nameof(Cidade));
        Guard.AgainstNullorWhiteSpace(pais, nameof(Pais));

        if (!Regex.IsMatch(cep ?? "", @"^\d{5}-\d{3}$"))
            throw new DomainException("CEP inválido. O formato correto é 00000-000.");

        Cep = cep!;
        Logradouro = logradouro;
        Complemento = complemento ?? string.Empty;
        Bairro = bairro;
        Estado = estado;
        Cidade = cidade;
        Pais = pais;
    }

    public static EnderecoEntrega Criar (string cep, string logradouro, string complemento, string bairro, string estado, string cidade, string pais)
    {
        return new EnderecoEntrega(cep, logradouro, complemento, bairro, estado, cidade, pais);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Logradouro;
        yield return Complemento ?? string.Empty;
        yield return Bairro;
        yield return Estado;
        yield return Cidade;
        yield return Pais;
    }

    public string FormatarEndereco()
    {
               return $"{Logradouro}, {Complemento} - {Bairro}, {Cidade} - {Estado}, {Pais} - CEP: {Cep}";
    }
}
