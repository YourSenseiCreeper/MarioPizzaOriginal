using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Controller
{
    public class PizzaController : FoodSizeSauceController
    {
        private IMarioPizzaRepository _marioPizzaRepository;
        public PizzaController(IMarioPizzaRepository marioPizzaRepository) : base(marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        
    }
}
