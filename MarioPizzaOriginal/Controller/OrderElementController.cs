using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Controller
{
    public class OrderElementController
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;
        public OrderElementController(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        public MarioResult GetAllElements()
        {

        }

        public MarioResult AddElement()
        {

        }

        public MarioResult ChangeAmount()
        {

        }

        public MarioResult DeleteElement()
        {

        }

    }
}
