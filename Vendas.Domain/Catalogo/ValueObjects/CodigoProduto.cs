using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Catalogo.ValueObjects;

public sealed class CodigoProduto : ValueObject
{
    public string Valor { get; }
    public CodigoProduto(string valor)
    {
        Guard.AgainstNullorWhiteSpace(valor, nameof(valor), 
            "O código do produto é obrigatório.");

        Guard.Against<DomainException>(valor.Length < 3,
            "O código do produto deve ter no mínimo 3 caracteres.");

        Valor = valor.Trim().ToUpper();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Valor;
    }
}
