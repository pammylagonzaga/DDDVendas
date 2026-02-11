using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo;
using Vendas.Domain.Catalogo.Events;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Catalogos;

public class CategoriaTests
{
    [Fact]

    public void CriarCategoria_DeveCriarAtivarComNomeValido()
    {         // Arrange
        var nome = "Eletrônicos";
        // Act
        var categoria = new Categoria(nome);
        // Assert
        categoria.Nome.Should().Be(nome);
        categoria.Ativa.Should().BeTrue();
        categoria.DataCriacao.Should().NotBe(default);
        categoria.Descricao.Should().BeNull();
        categoria.DomainEvents.Should().BeEmpty(); // Nenhum evento deve ser disparado no construtor
    }

    [Fact]
    public void CriarCategoria_ComNomeInvalido_DeveLancarDomainException()
    {
        // Arrange
        Action act = () => new Categoria("ab");
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome deve ter no mínimo 3 caracteres.");
    }

    [Fact]

    public void CriarCategoria_ComNomeVazio_DeveLancarDomainException()
    {
        // Arrange
        Action act = () => new Categoria("   ");
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome é obrigatório.");
    }

    [Fact]

    public void AlterarNome_DeveAtualizarNomeEDataAtualizacao()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        // Act
        categoria.AlterarNome("Periféricos");
        // Assert
        categoria.Nome.Should().Be("Periféricos");
        categoria.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]

    public void AlterarNome_ComNomeInvalido_DeveLancarDomainException()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");

        Action act = () => categoria.AlterarNome("ab");
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome deve ter no mínimo 3 caracteres.");
    }

    [Fact]

    public void AlterarDescricao_DeveAtualizarDescricaoEDataAtualizacao()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        // Act
        categoria.AlterarDescricao("Categoria de dispositivos eletrônicos");
        // Assert
        categoria.Descricao.Should().Be("Categoria de dispositivos eletrônicos");
        categoria.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]

    public void Ativar_DeveGerarEventoCategoriaAtivada()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar(); // Primeiro inativa para depois ativar
        categoria.ClearDomainEvents(); // Limpa eventos anteriores
        // Act
        categoria.Ativar();
        var events = categoria.DomainEvents;
        // Assert
        events.Should().ContainSingle().Which.Should().BeOfType<CategoriaAtivadaEvent>();

        categoria.Ativa.Should().BeTrue();
    }

    [Fact]

    public void Ativar_QuandoJaAtiva_DeveLancarDomainException()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        // Act
        Action act = () => categoria.Ativar();
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Categoria já está ativa.");
    }

    [Fact]

    public void Inativar_DeveGerarEventoCategoriaInativada()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        // Act
        categoria.Inativar(); // Primeiro inativa para depois ativar
        var events = categoria.DomainEvents;
        // Assert
        events.Should().ContainSingle()
            .Which.Should().BeOfType<CategoriaInativadaEvents>();
        categoria.Ativa.Should().BeFalse();
    }

    [Fact]

    public void Inativar_QuandoJaInativa_DeveLancarDomainException()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();
        // Act
        Action act = () => categoria.Inativar();
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Categoria já está inativda.");
    }

    [Fact]

    public void DomainEvents_DeveSerPossivelLimparEventos()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();

        categoria.DomainEvents.Should().HaveCount(1);
        // Act
        categoria.ClearDomainEvents();
        // Assert
        categoria.DomainEvents.Should().BeEmpty();
    }
}
