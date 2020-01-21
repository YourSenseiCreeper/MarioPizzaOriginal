using Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarioPizzaWeb
{
    public class DataAccess
    {
        private static IMarioPizzaRepository repository;

        public DataAccess()
        {
            repository = new MarioPizzaRepository();
        }
        public static IFoodRepository FoodRepository => repository.FoodRepository;
        public static IOrderRepository OrderRepository => repository.OrderRepository;
        public static IOrderElementRepository OrderElementRepository => repository.OrderElementRepository;

    }

}
