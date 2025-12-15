using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Pedidos.Events;

public record PagamentoRejeitadoEvent (Guid PagamentoId,
                                        Guid PedidoId,
                                        decimal Valor,
                                        DateTime DataPagamento,
                                        string? CodigoTransacao) : DomainEventBase;