using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Common.Base;

public abstract class AggregatRoot : Entity
{
    protected AggregatRoot() : base()
    {
    }

    protected AggregatRoot(Guid id) : base(id)
    {
    }
}
