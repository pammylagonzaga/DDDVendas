using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Clientes;

public sealed class Endereco : Entity
{
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Pais { get; private set; }
    public string Complemento { get; private set; }

    public Endereco (
        string cep,
        string logradouro,
        string numero, 
        string bairro, 
        string cidade, 
        string estado, 
        string pais, 
        string complemento = "")
    {
        Validar(cep, logradouro, numero, bairro, cidade, estado, pais);

        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
        Complemento = complemento;
    }
    internal void Atualizar(
       string cep,
       string logradouro,
       string numero,
       string bairro,
       string cidade,
       string estado,
       string pais,
       string complemento = "")
    {
        Validar(cep, logradouro, numero, bairro, cidade, estado, pais);

        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
        Complemento = complemento;
    }
    private static void Validar (
        string cep, 
        string logradouro, 
        string numero, 
        string bairro, 
        string cidade, 
        string estado, 
        string pais)
    {
        Guard.AgainstNullorWhiteSpace(cep, nameof(cep),"O CEP é obrigatório.");
        Guard.Against<DomainException>(!Regex.IsMatch(cep, @"^\d{8}$"), "CEP inválido.");
        Guard.AgainstNullorWhiteSpace(logradouro, nameof(logradouro), "O logadouro é obrigatório.");
        Guard.Against<DomainException>(logradouro.Length < 3, "Logadouro muito curto.");
        Guard.AgainstNullorWhiteSpace(numero, nameof(numero), "O número é obrigatório.");
        Guard.Against<DomainException>(numero.Length == 0, "Número inválido.");
        Guard.AgainstNullorWhiteSpace(bairro, nameof(bairro), "O bairro é obrigatório.");
        Guard.AgainstNullorWhiteSpace(cidade, nameof(cidade), "A cidade é obrigatória.");
        Guard.AgainstNullorWhiteSpace(estado, nameof(estado), "O estado é obrigatório.");
        Guard.AgainstNullorWhiteSpace(pais, nameof(pais), "O país é obrigatório.");
    }
}

