using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Clientes.Entities;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Clientes.Entities;

public class EnderecoTests
{
    private static Endereco CriarEnderecoValido()
    {
        return new Endereco(
            cep: "12345678",
            logradouro: "Rua Exemplo",
            numero: "123",
            bairro: "Centro",
            cidade: "São Paulo",
            estado: "SP",
            pais: "Brasil"
        );
    }

    [Fact]

    public void Deve_Criar_Endereco_Valido()
    {
        // Arrange & Act
        var endereco = CriarEnderecoValido();
        // Assert
        endereco.Cep.Should().Be("12345678");
        endereco.Logradouro.Should().Be("Rua Exemplo");
        endereco.Numero.Should().Be("123");
        endereco.Bairro.Should().Be("Centro");
        endereco.Cidade.Should().Be("São Paulo");
        endereco.Estado.Should().Be("SP");
        endereco.Pais.Should().Be("Brasil");
        endereco.Complemento.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_Lancar_Erro_Quando_CEP_Invalido(string? cepInvalido)
    {
        // Arrange, Act & Assert
        Action act = () => new Endereco(
            cep: cepInvalido!,
            logradouro: "Rua Exemplo",
            numero: "123",
            bairro: "Centro",
            cidade: "São Paulo",
            estado: "SP",
            pais: "Brasil"
        );
        act.Should().Throw<DomainException>()
            .WithMessage("O CEP é obrigatório.");
    }

    [Fact]
    public void Deve_Lancar_Erro_Quando_CEP_Nao_Tiver_8_Digitos()
    {
        // Arrange, Act & Assert
        Action act = () => new Endereco(
            cep: "1234",
            logradouro: "Rua Exemplo",
            numero: "123",
            bairro: "Centro",
            cidade: "São Paulo",
            estado: "SP",
            pais: "Brasil"
        );
        act.Should().Throw<DomainException>()
            .WithMessage("CEP inválido.");
    }

    [Theory]
    [InlineData(null,"123", "Centro", "São Paulo", "SP", "Brasil")]
    [InlineData("Rua Exemplo", null, "Centro", "São Paulo", "SP", "Brasil")]
    [InlineData("Rua Exemplo", "123", null, "São Paulo", "SP", "Brasil")]
    [InlineData("Rua Exemplo", "123", "Centro", null, "SP", "Brasil")]
    [InlineData("Rua Exemplo", "123", "Centro", "São Paulo", null, "Brasil")]
    [InlineData("Rua Exemplo", "123", "Centro", "São Paulo", "SP", null)]
    public void Deve_Lancar_Erro_Quando_Outros_Campos_Obrigatorios_Forem_Invalidos(
        string? logradouro,
        string? numero,
        string? bairro,
        string? cidade,
        string? estado,
        string? pais)
    {
        // Arrange, Act & Assert
        Action act = () => new Endereco(
            cep: "12345678",
            logradouro: logradouro!,
            numero: numero!,
            bairro: bairro!,
            cidade: cidade!,
            estado: estado!,
            pais: pais!
        );
        act.Should().Throw<DomainException>();
    }
    [Fact]
    public void Deve_Atualizar_Endereco_Com_Dados_Validos()
    {
        // Arrange, 
        var endereco = CriarEnderecoValido();

        //Act
        endereco.Atualizar(
            cep: "87654321",
            logradouro: "Rua B",
            numero: "200",
            bairro: "Bairro Novo",
            cidade: "Rio de Janeiro",
            estado: "RJ",
            pais: "Brasil",
            complemento: "Apto 12"
        );

        // Assert
        endereco.Cep.Should().Be("87654321");
        endereco.Logradouro.Should().Be("Rua B");
        endereco.Numero.Should().Be("200");
        endereco.Bairro.Should().Be("Bairro Novo");
        endereco.Cidade.Should().Be("Rio de Janeiro");
        endereco.Estado.Should().Be("RJ");
        endereco.Pais.Should().Be("Brasil");
        endereco.Complemento.Should().Be("Apto 12");

    }
    [Fact]
    public void Deve_Lancar_Erro_Ao_Atualizar_Com_CEP_Invalidos()
    {
        // Arrange
        var endereco = CriarEnderecoValido();
        // Act
        Action act = () => endereco.Atualizar(
            cep: "123",
            logradouro: "Rua Test",
            numero: "10",
            bairro: "Centro",
            cidade: "SP",
            estado: "SP",
            pais: "Brasil"
        );
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CEP inválido.");
    }
    [Fact]
    public void Deve_Lancar_Erro_Ao_Atualizar_Com_Outros_Campo_Obrigatorio_Invalido()
    {
        // Arrange
        var endereco = CriarEnderecoValido();
        // Act
        Action act = () => endereco.Atualizar(
            cep: "12345678",
            logradouro: "",
            numero: "10",
            bairro: "Centro",
            cidade: "SP",
            estado: "SP",
            pais: "Brasil"
        );
        // Assert
        act.Should().Throw<DomainException>();
    }
}
