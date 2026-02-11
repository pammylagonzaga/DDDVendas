using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Application.Commands.PedidosCommands.CancelarPedidoMotivo;

public sealed class CancelarPedidoResultDto
{
    public Guid PedidoId { get; init; }
    public string Status { get; init; } = string.Empty;

}

