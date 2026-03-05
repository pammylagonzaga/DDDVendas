
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Integration.Cliente;

public sealed class ClientesAcl
{
    public EnderecoEntrega
            TraduzirEndereco(EnderecoDto dto)
    {
        return EnderecoEntrega.Criar(
            dto.Cep,
            dto.Logradouro,
            dto.Numero,
            dto.Bairro,
            dto.Cidade,
            dto.Estado,
            dto.Pais,
            dto.Complemento
            );
    }
}
