using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.Entities;
using Vendas.Domain.Pedidos.Events;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Tests.Pedidos.Entities;

public class PedidoTests
{
    private static EnderecoEntrega CriarEnderecoValido()
        => EnderecoEntrega.Criar("12345-000", "Rua A", "Ap 1", "Centro", "SP", "São Paulo", "Brasil");

    private static readonly Guid ClienteIdValido = Guid.NewGuid();
    private static readonly Guid ProdutoIdValido = Guid.NewGuid();

    private static void SetStatusPedido(Pedido pedido, StatusPedido status)
    {
        typeof(Pedido).GetProperty(nameof(Pedido.StatusPedido),
            BindingFlags.Public | BindingFlags.Instance)!
                .SetValue(pedido, status);
    }

    //Criação de pedido

    [Fact(DisplayName = "Deve criar pedido válido com status Pendente")]

    public void Deve_Criar_Pedido_valido()
    {
        // Arrange & Act
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());

        // Assert
        pedido.Should().NotBeNull();
        pedido.ClienteId.Should().Be(ClienteIdValido);
        pedido.EnderecoEntrega.Should().NotBeNull();
        pedido.StatusPedido.Should().Be(StatusPedido.Pendente);
        pedido.ValorTotal.Should().Be(0);
        pedido.Itens.Should().BeEmpty();
        pedido.Pagamentos.Should().BeEmpty();
        pedido.Id.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Não deve criar pedido com ClienteId inválido")]

    public void Nao_Deve_Criar_Pedido_Com_ClienteId_Invalido()
    {
        // Act
        Action act = () => Pedido.Criar(Guid.Empty, CriarEnderecoValido());
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*ClienteId inválido.");
    }

    [Fact(DisplayName = "Não deve criar pedido sem endereço de entrega.")]
    public void Nao_Deve_Criar_Pedido_Sem_Endereco_Entrega()
    {
        // Act
        Action act = () => Pedido.Criar(ClienteIdValido, null!);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O endereço de entrega é obrigatório.");
    }

    [Fact(DisplayName = "Deve adicionar item ao pedido e recalcular valor total")]
    public void Deve_Adicionar_Item_Ao_Pedido_E_Recalcular_Valor_Total()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        // Act
        pedido.AdicionarItem(ProdutoIdValido, "Mouse", 100m, 2);
        // Assert
        pedido.Itens.Should().HaveCount(1);
        pedido.ValorTotal.Should().Be(200m);
        pedido.Itens.First().ValorTotal.Should().Be(200m);
    }

    [Fact(DisplayName = "Deve somar quantidade de item existente ao adicionar mesmo produto")]
    public void Deve_Somar_Quantidade_De_Item_Existente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        var produtoId= ProdutoIdValido;

        // Act
        pedido.AdicionarItem(ProdutoIdValido, "Teclado", 200m, 1);
        pedido.AdicionarItem(ProdutoIdValido, "Teclado", 200m, 2);
        // Assert
        pedido.Itens.Should().HaveCount(1);
        var item = pedido.Itens.First();
        item.Quantidade.Should().Be(3);
        item.ValorTotal.Should().Be(600m);
        pedido.ValorTotal.Should().Be(600m);
    }

    [Theory(DisplayName = "Não deve permitir adicionar itens quando o pedido não estiver Pendente")]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]

    public void Nao_Deve_Adicionar_Item_Quando_Pedido_Nao_Estiver_Pendente(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, status);
        // Act
        Action act = () => pedido.AdicionarItem(Guid.NewGuid(), "Outro", 100m, 1);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Itens só podem ser adicionados enquanto o pedido está pendente.");
    }

    [Fact (DisplayName = "Deve remover item e recalcular valor total")]

    public void Deve_Remover_Item_E_Recalcular_Valor()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Mouse", 100m, 2);

        // Act
        Action act = () => pedido.RemoverItem(pedido.Itens.First().Id);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pedido deve conter pelo menos um item.");
    }
    [Fact(DisplayName = "Deve remover item e recaucular valor total quando houver mais de um item.")]
    public void Deve_Remover_Item_Quando_Houver_Mais_De_Um()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        var produto1 = Guid.NewGuid();
        var produto2 = Guid.NewGuid();


        pedido.AdicionarItem(produto1, "Mouse", 100m, 1);
        pedido.AdicionarItem(produto2, "Teclado", 200m, 1);
        // Act
        var itemId = pedido.Itens.First(i => i.ProdutoId == produto1).Id;
        pedido.RemoverItem(itemId);
        // Assert
        pedido.Itens.Should().HaveCount(1);
        pedido.ValorTotal.Should().Be(200m);
    }

    [Fact(DisplayName = "Deve ignorar remoção de item inexistente")]

    public void Deve_Ignorar_Remocao_De_Item_Inexistente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Mouse", 100m, 2);
        // Act
        Action act = () => pedido.RemoverItem(Guid.NewGuid());
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Item não encontradao no pedido.");
    }

    [Theory(DisplayName = "Não deve permitir remover itens quando o pedido não estiver Pendente")]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]

    public void Nao_Deve_Remover_Item_Quando_Nao_Pendente(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 10m, 1);
        SetStatusPedido(pedido, status);
        // Act
        Action act = () => pedido.RemoverItem(ProdutoIdValido);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Itens só podem ser removidos em pedidos pendente.");
    }

    //endereço
    public void Deve_Atualizar_Endereco_Quando_Pendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        //var novo Endereço = new EnderecoEntrega(...);
        var novoEndereco = EnderecoEntrega.Criar("54321-000", "Rua B", "Casa", "Bairro", "RJ", "Rio de Janeiro", "Brasil");
        // Act
        pedido.AtualizarEnderecoEntrega(novoEndereco);
        // Assert
        pedido.EnderecoEntrega.Should().Be(novoEndereco);
    }

    [Theory(DisplayName = "Não deve atualizar endereço de entrega quando não Pendente")]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]

    public void Nao_Deve_Atualizar_Endereco_Quando_Nao_Pendente(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        //var novo Endereço = new EnderecoEntrega(...);
        var novoEndereco = EnderecoEntrega.Criar("54321-000", "Rua B", "Casa", "Bairro", "RJ", "Rio de Janeiro", "Brasil");
        SetStatusPedido(pedido, status);
        // Act
        Action act = () => pedido.AtualizarEnderecoEntrega(novoEndereco);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O endereço de entrega só pode ser alterado enquanto o pedido está pendente.");
    }

    //pagamentos
    [Fact(DisplayName = "Deve iniciar pagamento e manter status Pendentes")]

    public void Deve_Iniciar_Pagamento()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 100m, 2);
        // Act
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);
        // Assert
        pagamento.Should().NotBeNull();
        pagamento.Valor.Should().Be(200m);
        pedido.Pagamentos.Should().Contain(pagamento);
        pedido.StatusPedido.Should().Be(StatusPedido.Pendente);
    }

    [Fact(DisplayName = "Não deve iniciar pagamento sem itens no pedido")]

    public void Nao_Deve_Iniciar_Pagamento_Sem_Itens()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        // Act
        Action act = () => pedido.IniciarPagamento(MetodoPagamento.Pix);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Não é possivel iniciar o pagamento de um pedido sem itens.");
    }

    [Fact (DisplayName = "Deve iniciar pagamento se já houver um pagamento Pendente")]

    public void Deve_Iniciar_Pagamento_Se_Ja_Houver_Pagamento_Pendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 150m, 1);
        //Simula a criacao de um pagamento pendente
        pedido.IniciarPagamento(MetodoPagamento.Pix);
        // Act
        Action act = () => pedido.IniciarPagamento(MetodoPagamento.CartaoDebito);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Já existe um pagamento pendente para este pedido");
    }

    [Fact(DisplayName = "Deve alterar status para PagamentoConfirmado ao HandlePagamentoAprovado")]

    public void Deve_Alterar_Status_ao_HandlePagamentoAprovado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 100m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.Pix);
        // Act
        pedido.HandlePagamentoAprovado(pagamento.Id);
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.PagamentoConfirmado);
    }

    [Fact(DisplayName = "Deve manter status Pendente ao HandlePagamentoRecusado")]
    public void Deve_Manter_Status_Pendente_ao_HandlePagamentoRecusado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 100m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.Pix);
        // Act
        pedido.HandlerPagamentoRejeitado(pagamento.Id);
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Cancelado);
        pedido.DomainEvents.Should().ContainSingle(e => e is PedidoCanceladoEvent);
    }

    [Fact(DisplayName = "Não deve HandlePagamentoAprovado se status não for Pendente")]

    public void Nao_Deve_HandlePagamentoAprovado_Se_Nao_Pendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 100m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.Pix);
        // Simula alteração de status incorreto
        SetStatusPedido(pedido, StatusPedido.EmSeparacao);
        // Act
        Action act = () => pedido.HandlePagamentoAprovado(pagamento.Id);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pedido não está no status esperado para confirmação de pagamento");
    }

    //transição de estado   
    [Fact(DisplayName = "Deve permitir marcar como EmSeparacao após PagamentoConfirmado")]

    public void Deve_Marcar_Como_EmSeparacao()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido,"Produto", 100m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);
        pedido.HandlePagamentoAprovado(pagamento.Id); // status pagamento confirmado
        // Act
        pedido.MarcarComoEmSeparacao();
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.EmSeparacao);
    }

    [Fact(DisplayName = "Não deve marcar como EmSeparacao se não estiver em PagamentoConfirmado")]

    public void Nao_Deve_Marcar_Como_EmSeparacao_Se_Nao_PagamentoConfirmado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        // Act
        Action act = () => pedido.MarcarComoEmSeparacao();
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pedido só pode ir para 'Em Separação' após o pagamento ser confirmado.");
    }

    [Fact(DisplayName = "Deve permitir marcar como Enviado")]
    public void Deve_Marcar_Como_Enviado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.EmSeparacao);
        // Act
        pedido.MarcarComoEnviado();
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Enviado);
    }

    [Fact(DisplayName = "Não deve marcar como Enviado se não estiver EmSeparacao")]
    public void Nao_Deve_Marcar_Como_Enviado_Se_Nao_EmSeparacao()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.PagamentoConfirmado);
        // Act
        Action act = () => pedido.MarcarComoEnviado();
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pedido só pode ser marcado como 'Enviado' após estar 'Em Separação'.");
    }

    [Fact(DisplayName = "Deve permitir marcar como Entregue")]
    public void Deve_Marcar_Como_Entregue()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.Enviado);
        // Act
        pedido.MarcarComoEntregue();
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Entregue);
    }

    [Fact(DisplayName = "Não deve marcar como Entregue se não estiver Enviado")]
    public void Nao_Deve_Marcar_Como_Entregue_Se_Nao_Enviado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.EmSeparacao);
        // Act
        Action act = () => pedido.MarcarComoEntregue();
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pedido só pode ser marcado como 'Entregue' após estar 'Enviado'.");
    }

    //cancelamento
    [Fact(DisplayName = "Deve cancelar pedido Pendente")]
    public void Deve_Cancelar_Pedido_Pendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 50m, 1);
        // Act
        pedido.CancelarPedido();
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Cancelado);
    }

    [Fact(DisplayName = "Deve cancelar pedido PagamentoConfirmado")]
    public void Deve_Cancelar_Pedido_PagamentoConfirmado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto", 50m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.Pix);
        pedido.HandlePagamentoAprovado(pagamento.Id);
        // Act
        pedido.CancelarPedido();
        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Cancelado);
    }

    [Theory(DisplayName = "Não deve permitir cancelar pedido após EmSeparacao")]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]

    public void Nao_Deve_Cancelar_Apos_EmSeparacao(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, status);
        // Act
        Action act = () => pedido.CancelarPedido();
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Não é possivel cancelar um pedido que já está em separação ou posterior.");
    }
}