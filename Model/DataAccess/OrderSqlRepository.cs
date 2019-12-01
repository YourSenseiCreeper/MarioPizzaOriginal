using MarioPizzaOriginal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public class OrderSqlRepository
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;

        public OrderSqlRepository(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }
    }
}
