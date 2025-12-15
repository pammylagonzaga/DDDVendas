using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;
using Vendas.Domain.Pedidos.Events;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Entities;
 public sealed class Pedido:AggregatRoot
{
    // PROPRIEDADES DO DOMÍNIO
    public Guid ClienteId { get; private set; }
    public EnderecoEntrega EnderecoEntrega { get; private set; }
    public decimal ValorTotal { get; private set; }
    public StatusPedido StatusPedido { get; private set; }
    public string NumeroPedido { get; private set; } = string.Empty;

    //ITENS
   
    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    // PAGAMENTOS
    private readonly List<Pagamento> _pagamentos = new();
    public IReadOnlyCollection<Pagamento> Pagamentos => _pagamentos.AsReadOnly();

    // CONSTRUTORES

    private Pedido(Guid clienteId, EnderecoEntrega enderecoEntrega)
    {
        Guard.AgainstNullOrEmpty(clienteId, nameof(clienteId), "ClienteId inválido."); // NA aula ele utiliza AgainstEmptyGuid
        Guard.AgainstNull(enderecoEntrega, nameof(enderecoEntrega), "O endereço de entrega é obrigatório.");

        ClienteId = clienteId;
        EnderecoEntrega = enderecoEntrega;
        StatusPedido = StatusPedido.Pendente;
        ValorTotal = 0m;

        GerarNumeroPedido();
    }

    public static Pedido Criar(Guid clienteId, EnderecoEntrega enderecoEntrega)
        => new(clienteId, enderecoEntrega);

    public void AdicionarItem(Guid produtoId, string nomeProduto,
                              decimal precoUnitario, int quantidade)
    {
        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.Pendente,
            "Itens só podem ser adicionados enquanto o pedido está pendente.");

        var existente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (existente is not null)
            existente.AdicionarUnidades(quantidade);
        else
            _itens.Add(new ItemPedido(produtoId, nomeProduto, precoUnitario, quantidade));

        RecalcularValorTotal();
        SetDataAtualizacao();
    }

    public void RemoverItem(Guid itemId)
    { 
        Guard.AgainstNullOrEmpty(itemId, nameof(itemId), "ItemId inválido."); // Na aula ele utiliza AgainstEmptyGuid
        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.Pendente,
            "Itens só podem ser removidos em pedidos pendente.");

        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        Guard.AgainstNull(item, nameof(item), "Item não encontradao no pedido.");

        _itens.Remove(item!); // sei que item nao pode ser nulo aqui

        Guard.Against<DomainException>(_itens.Count == 0,
            "O pedido deve conter pelo menos um item.");

        RecalcularValorTotal();
        SetDataAtualizacao();
    }

    public void AtualizarEnderecoEntrega(EnderecoEntrega novoEndereco)
    {         
        Guard.AgainstNull(novoEndereco, nameof(novoEndereco));
        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.Pendente,
            "O endereço de entrega só pode ser alterado enquanto o pedido está pendente.");

        EnderecoEntrega = novoEndereco;
        SetDataAtualizacao();
    }

    public Pagamento IniciarPagamento(MetodoPagamento metodoPagamento)
    {
        Guard.Against<DomainException>(!_itens.Any(),
            "Não é possivel iniciar o pagamento de um pedido sem itens.");

        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente,
            "O pagamento só pode ser iniciado a partir do status Pendente.");

        if (_pagamentos.Any(p => p.StatusPagamento == StatusPagamento.Pendente))
            throw new DomainException("Já existe um pagamento pendente para este pedido");

        var novoPagamento = new Pagamento(Id, metodoPagamento, ValorTotal);
        _pagamentos.Add(novoPagamento);
        SetDataAtualizacao();

        // O proprio pagamento emitirá PagamentoAprovadoEvent ou PagamentoRejeitadoEvent.
        return novoPagamento;
    }

    public void HandlePagamentoAprovado(Guid pagamentoId)
    {
        var pagamento = _pagamentos.FirstOrDefault(p => p.Id == pagamentoId);
        if (pagamento is null) return;

        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.Pendente,
            "O pedido não está no status esperado para confirmação de pagamento");

        StatusPedido = StatusPedido.PagamentoConfirmado;
        SetDataAtualizacao();
    }

    public void HandlerPagamentoRejeitado(Guid pagamentoId)
    {
        var pagamento = _pagamentos.FirstOrDefault(p => p.Id == pagamentoId);
        if (pagamento is null) return;

        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.Pendente,
            "O pedido não está no status esperado para rejeição de pagamento");

        StatusPedido = StatusPedido.Cancelado;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoCanceladoEvent(
            Id,
            ClienteId,
            StatusPedido,
            MotivoCancelamento.ErroPagamento(),
            pagamentoId));
    }

    public void MarcarComoEmSeparacao()
    {
        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.PagamentoConfirmado,
            "O pedido só pode ir para 'Em Separação' após o pagamento ser confirmado.");

        StatusPedido = StatusPedido.EmSeparacao;
        SetDataAtualizacao();
    }

    public void MarcarComoEnviado()
    {
        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.EmSeparacao,
            "O pedido só pode ser marcado como 'Enviado' após estar 'Em Separação'.");

        StatusPedido = StatusPedido.Enviado;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoEnviadoEvent(Id, ClienteId, EnderecoEntrega));
    }

    public void MarcarComoEntregue()
    {
        Guard.Against<DomainException>(
            StatusPedido != StatusPedido.Enviado,
            "O pedido só pode ser marcado como 'Entregue' após estar 'Enviado'.");

        StatusPedido = StatusPedido.Entregue;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoEntregueEvent(Id, ClienteId));
    }

    public void CancelarPedido(MotivoCancelamento? motivo=null)
    {
        Guard.Against<DomainException>(
            StatusPedido >= StatusPedido.EmSeparacao,
            "Não é possivel cancelar um pedido que já está em separação ou posterior.");

        StatusPedido = StatusPedido.Cancelado;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoCanceladoEvent(
            Id,
            ClienteId,
            StatusPedido,
            motivo ?? MotivoCancelamento.Outro(),
            _pagamentos.LastOrDefault()?.Id));
    }



    private void RecalcularValorTotal()
        => ValorTotal = _itens.Sum(i => i.ValorTotal);

    private void GerarNumeroPedido()
    => NumeroPedido = $"PED-{Id.ToString()[..8].ToUpper()}";
}