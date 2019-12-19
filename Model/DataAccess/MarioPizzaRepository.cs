using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public class MarioPizzaRepository : IMarioPizzaNewRepository
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderElementRepository _orderElementRepository;
        private readonly IOrderSubElementRepository _orderSubElementRepository;

        public IFoodRepository FoodRepository => _foodRepository;
        public IIngredientRepository IngredientRepository => _ingredientRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IOrderElementRepository OrderElementRepository => _orderElementRepository;
        public IOrderSubElementRepository OrderSubElementRepository => _orderSubElementRepository;

        public MarioPizzaRepository()
        {
            _foodRepository = new FoodRepository();
            _ingredientRepository = new IngredientRepository();
            _orderElementRepository = new OrderElementRepository(_orderSubElementRepository);
            _orderSubElementRepository = new OrderSubElementRepository();
            _orderRepository = new OrderRepository(_orderElementRepository, _orderSubElementRepository);
        }
    }
}
