using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Tests.Pedidos.ValueObjects;

public class EnderecoEntregaTests
{
    //Teste de criação bem sucedida
    //--------------------------------------------------------
    [Fact(DisplayName = "Deve criar EnderecoEntrega com sucesso quando todos os dados são válidos")]
    public void Criar_DeveRetornarEnderecoValido_QuandoDadosForemValidos()
    {
        //Arrange
        var cep = "01234-567";
        var logradouro = "Rua das Flores";
        var complemento = "Apto 101";
        var bairro = "Jardim Primavera";
        var estado = "SP";
        var cidade = "São Paulo";
        var pais = "Brasil";

        //Act
        var endereco = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, estado, cidade, pais);

        //Assert
        endereco.Should().NotBeNull();
        endereco.Cep.Should().Be(cep);
        endereco.Logradouro.Should().Be(logradouro);
        endereco.Complemento.Should().Be(complemento);
        endereco.Bairro.Should().Be(bairro);
        endereco.Estado.Should().Be(estado);
        endereco.Cidade.Should().Be(cidade);
        endereco.Pais.Should().Be(pais);
        endereco.FormatarEndereco().Should().Contain("Rua das Flores");
    }

    //Teste de falha na criação - formatação de cep
    //--------------------------------------------------------

    [Theory(DisplayName = "Deve lançar DomainException quando o CEP estiver em formato inválido")]
    [InlineData("1234567")]       // Sem hífen
    [InlineData("12-345678")]     // Formato Incorreto
    [InlineData("1234-5678")]     // Muito longo
    [InlineData("12a34-567")]     // Caracteres inválidos
    [InlineData("12345_678")]     // Caractere especial inválido
    public void Criar_DeveLancarDomainException_QuandoCepForInvalido(string cepInvalido)
    {
        //Arrange
        var logradouro = "Rua das Flores";
        var complemento = "Apto 101";
        var bairro = "Jardim Primavera";
        var estado = "SP";
        var cidade = "São Paulo";
        var pais = "Brasil";

        //Act
        Action act = () => EnderecoEntrega.Criar(cepInvalido, logradouro, complemento, bairro, estado, cidade, pais);

        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CEP inválido. O formato correto é 00000-000.");
    }

    [Fact(DisplayName = "Dois EnderecosEntrega com mesmo dados devem ser iguais (Value Object)")]

    public void EnderecoEntregaDevemSerIguais_QuandoPossuemMesmoValores()
    {

        //Arrange
        var endereco1 = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa", "Centro", "SP", "São Paulo", "Brasil");
        var endereco2 = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa", "Centro", "SP", "São Paulo", "Brasil");

        //Assert
        endereco1.Should().Be(endereco2);
        (endereco1 == endereco2).Should().BeTrue();

    }
    [Fact(DisplayName = "EnderecosEntrega devem ser diferentes quando algum campo for diferente")]

    public void EnderecosDevemSerDiferentes_QuandoAlgumCampoForDiferente()
    {
        //Arrange
        var endereco1 = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa", "Centro", "SP", "São Paulo", "Brasil");
        var endereco2 = EnderecoEntrega.Criar("12345-678", "Rua Y", "Casa", "Centro", "SP", "São Paulo", "Brasil");
        //Assert
        endereco1.Should().NotBe(endereco2);
    }

    [Fact(DisplayName = "EnderecoEntrega deve ser imutável após a criação")]

    public void EnderecoEntregaDeveSerImutavel_AposCriacao()
    {
        //Arrange
        var endereco = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa", "Centro", "SP", "São Paulo", "Brasil");
        //Act 
        Action act = () =>
        {
            //Tentativa hipotetica (isso deve falhar em tempo de compilação)
            // endereco.Cep = "98765-432"; // Descomente esta linha para testar a imutabilidade
        };

        //Assert
        endereco.GetType().GetProperties()
            .All(p => p.SetMethod == null || p.SetMethod.IsPrivate)
            .Should().BeTrue("As propriedades do VO devem ser imutáveis");
    }

    [Theory(DisplayName = "Deve lançar DomainException quando campos obrigatórios forem nulos ou vazios")]
    [InlineData(null, "Logadouro", "Bairro", "Estado", "Cidade", "Pais")]
    [InlineData("12345-678", null, "Bairro", "Estado", "Cidade", "Pais")]
    [InlineData("12345-678", "Logadouro", "Bairro", "Estado", "Cidade", null)]
    public void Criar_DeveLancarDomainException_QuandoCamposObrigatoriosForemNulosOuVazios(
        string cep, string logradouro, string bairro, string estado, string cidade, string pais)
    {
        //Act
        Action act = () => EnderecoEntrega.Criar(cep, logradouro, "Complemento", bairro, estado, cidade, pais);

        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*não pode ser nulo ou vazio*");
    }
}
