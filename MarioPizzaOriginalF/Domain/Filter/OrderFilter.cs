using System.Collections.Generic;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class OrderFilter : Filter<Order>
    {
        public OrderFilter(TinyIoCContainer container) : base(container)
        {
            FilterObjects = new List<FilterObject>
            {

            };
        }
    }
}
