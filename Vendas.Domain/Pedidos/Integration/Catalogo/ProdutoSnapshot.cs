using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Catalogo;

public sealed class ProdutoSnapshot : ValueObject
{
    public Guid PodutoId { get; }
    public string NomeProduto { get; }
    public decimal PrecoUnitario { get; }

    public ProdutoSnapshot(Guid podutoId, string nomeProduto, decimal precoUnitario)
    {
        Guard.AgainstNullOrEmpty(podutoId, nameof(podutoId));
        Guard.AgainstNullorWhiteSpace(nomeProduto, nameof(nomeProduto));
        Guard.Against<DomainException>(precoUnitario <= 0, "Preço unitário inválido");

        PodutoId = podutoId;
        NomeProduto = nomeProduto;
        PrecoUnitario = precoUnitario;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PodutoId;
        yield return NomeProduto;
        yield return PrecoUnitario;
    }
}
