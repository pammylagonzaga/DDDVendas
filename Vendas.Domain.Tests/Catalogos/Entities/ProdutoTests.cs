using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Catalogo.Entities;
using Vendas.Domain.Catalogo.Enums;
using Vendas.Domain.Catalogo.Events;
using Vendas.Domain.Catalogo.ValueObjects;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Catalogos.Entities;

public class ProdutoTests
{
    //Metodo auxiliar para criar Produto Valido

    private Produto CriarProduto(
        string nome = "Parafusadeira 2000",
        string codigo = "PAR-001",
        decimal preco = 2500m,
        int estoque = 10,
        string? descricao = null)
    {
        return new Produto(
            new NomeProduto(nome),
            new CodigoProduto(codigo),
            new PrecoProduto(preco),
            Guid.NewGuid(),
            estoque,
            descricao
            );
    }

    [Fact]

    public void CriarProduto_DeveNascerAtivo()
    {
        // Arrange & Act
        var produto = CriarProduto();
        // Assert
        produto.Status.Should().Be(StatusProduto.Ativo);
    }

    [Fact]

    public void CriarProduto_ComEstoqueNegativo_DeveLancarExcecao()
    {
        // Arrange
        Action act = () => CriarProduto(estoque: -1);
        // Act & Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O estoque inicial não pode ser negativo.");
    }

    [Fact]

    public void AlterarNome_DeveAtualizarNome()
    {
        // Arrange
        var produto = CriarProduto();
        // Act
        produto.AlterarNome(new NomeProduto("Câmera Mirrorless"));
        // Assert
        produto.Nome.Valor.Should().Be("Câmera Mirrorless");
    }

    [Fact]

    public void AlterarPreco_DeveAtualizarPrecoEGerarEvento()
    {
        // Arrange
        var produto = CriarProduto();
        produto.ClearDomainEvents();

        var novoPreco = new PrecoProduto(3000m);
        // Act
        produto.AlterarPreco(novoPreco);
        // Assert
        produto.Preco.Valor.Should().Be(3000m);

        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<PrecoProdutoAlteradoEvent>();

        var evento = (PrecoProdutoAlteradoEvent)produto.DomainEvents.Single();
        evento.PrecoAntigo.Should().Be(2500m);
        evento.PrecoNovo.Should().Be(3000m);
    }

    [Fact]

    public void AjustarEstoque_DeveAlterarEstoqueEGerarEvento()
    {
        var produto = CriarProduto();
        produto.ClearDomainEvents();

        produto.AjustarEstoque(5, "Reabastecimento");

        produto.Estoque.Should().Be(15);

        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<EstoqueAjustadoEvent>();
    }

    [Fact]

    public void AjustarEstoque_ResultadoNegativo_DeveLancarExcecao()
    {
        var produto = CriarProduto(estoque: 5);

        Action act = () => produto.AjustarEstoque(-10, "Erro de reabastecimento");

        act.Should().Throw<DomainException>()
            .WithMessage("O ajuste de estoque não pode resultar em estoque negativo.");
    }

    [Fact]

    public void Inativar_DeveMudarStatusEGerarEvento()
    {
        var produto = CriarProduto();
        produto.ClearDomainEvents();

        produto.Inativar();

        produto.Status.Should().Be(StatusProduto.Inativo);

        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ProdutoInativadoEvent>();
    }

    [Fact]

    public void Ativar_DeveMudarStatusEGerarEvento()
    {
        var produto = CriarProduto();

        produto.Inativar();
        produto.ClearDomainEvents();

        produto.Ativar();

        produto.Status.Should().Be(StatusProduto.Ativo);

        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ProdutoAtivadoEvent>();
    }

    [Fact]

    public void Inativar_QuandoJaInativo_DeveLancarExcecao()
    {
        var produto = CriarProduto();
        produto.Inativar();

        Action act = () => produto.Inativar();

        act.Should().Throw<DomainException>()
            .WithMessage("O produto já está inativo.");
    }

    [Fact]

    public void Ativar_QuandoJaAtivo_DeveLancarExcecao()
    {
        var produto = CriarProduto();

        Action act = () => produto.Ativar();

        act.Should().Throw<DomainException>()
            .WithMessage("O produto já está ativo.");
    }

    [Fact]

    public void AlterarDescricao_DeveAtualizarDescricao()
    {
        var produto = CriarProduto();

        produto.AlterarDescricao("Descrição nova");

        produto.Descricao.Should().Be("Descrição nova");
    }

    [Fact]

    public void AdicionarImagem_DeveAdicionarImagemEGerarEvento()
    {
        var produto = CriarProduto();
        produto.ClearDomainEvents();

        var imagem = new ImagemProduto("http://imagem.com/produto1.jpg",1);

        produto.AdicionarImagem(imagem);

        produto.Imagens.Should().HaveCount(1);

        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ImagemAdicionadaEvent>();
    }

    [Fact]

    public void AdicionarImagem_ComOrdemDuplicada_DeveLancarExcecao()
    {
        var produto = CriarProduto();
        
        produto.AdicionarImagem(new ImagemProduto("http://imagem.com/produto1.jpg", 1));

        Action action = () => produto.AdicionarImagem(new ImagemProduto("http://imagem.com/produto2.jpg", 1));

        action.Should().Throw<DomainException>()
            .WithMessage("Já existe uma imagem com esta ordem.");
    }

}
