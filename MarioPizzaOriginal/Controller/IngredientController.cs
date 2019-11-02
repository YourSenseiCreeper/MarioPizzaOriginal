using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarioPizzaOriginal.Controller
{
    public class IngredientController
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;
        public IngredientController(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        public MarioResult AddIngredient()
        {
            Console.WriteLine("Podaj nazwę nowego składnika:");
            var ingredientName = Console.ReadLine();
            UnitOfMeasure unitofmeasure = UnitOfMeasure.NONE;
            while(unitofmeasure == UnitOfMeasure.NONE)
            {
                Console.WriteLine("Wybierz jednostkę miary spośród podanych:");
                List<UnitOfMeasure> uoms = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().ToList();
                uoms.ForEach(uom => Console.WriteLine($"{(int)uom}. {uom}"));
                var userUOM = Console.ReadLine();
                try
                {
                    unitofmeasure = (UnitOfMeasure) Enum.Parse(typeof(UnitOfMeasure), userUOM);
                }
                catch(ArgumentException) { Console.WriteLine($"{userUOM} nie istnieje!");  }
            }
            Console.WriteLine("Podaj ilość nowego składnika: ");
            var amoutofUOM = Convert.ToDouble(Console.ReadLine());
            _marioPizzaRepository.AddIngredient(ingredientName, unitofmeasure, amoutofUOM);
            var message = $"Dodano nowy składnik: {ingredientName}";
            Console.WriteLine(message);
            return new MarioResult { Success = true, Message = message };
        }

        public void AllIngredients()
        {
            Console.WriteLine("Lista dostępnych składników:");
            Console.Write("lp.".PadLeft(5));
            Console.Write("Nazwa składnika".PadLeft(30));
            Console.Write("Jednostka miary".PadLeft(30));
            Console.Write("Ilość składnika".PadLeft(30));

            _marioPizzaRepository.GetIngredientList().ForEach(x => {
                Console.Write(x.IngredientId.ToString().PadLeft(5));
                Console.Write(x.IngredientName.PadLeft(30));
                Console.Write(x.UnitOfMeasureType.ToString().PadLeft(30));
                Console.Write(x.AmoutOfUOM.ToString().PadLeft(30));
                });
        }

        public MarioResult DeleteIngredient()
        {
            Console.WriteLine("Podaj nazwę lub numer składnika który chcesz usunąć");
            var ingredient = Console.ReadLine();
            bool success;
            if (Int32.TryParse(ingredient, out int ingredientId))
            {
                success = _marioPizzaRepository.DeleteIngredient(ingredientId);
            }
            else
            {
                success = _marioPizzaRepository.DeleteIngredient(ingredient);
            }
            var message = success ? $"Usunięto składnik {ingredient}" : "Podany składnik nie istnieje!";
            Console.WriteLine(message);
            return new MarioResult { Success = success, Message = message };
        }

        private void DescribeIngredient(Ingredient ingredient)
        {
            List<string> text = new List<string> { $"Nazwa składnika: {ingredient.IngredientName}",
            $"Numer porządkowy: {ingredient.IngredientId}", $"Jednostka miary: {ingredient.UnitOfMeasureType.ToString()}",
            $"Ilość jednostki miary: {ingredient.AmoutOfUOM}"};
            text.ForEach(x => Console.WriteLine(x));
        }

        public MarioResult GetIngredient(string ingredientName)
        {
            var ingredient = _marioPizzaRepository.GetIngredient(ingredientName);
            var message = $"Składnik {ingredientName} nie istnieje!";
            if(ingredient != null)
            {
                message = "";
                DescribeIngredient(ingredient);
            }
            return new MarioResult { Success = (ingredient != null), Message = message };
        }

        public MarioResult GetIngredient(int ingredientId)
        {
            var ingredient = _marioPizzaRepository.GetIngredient(ingredientId);
            var message = $"Składnik o numerze {ingredientId} nie istnieje!";
            if (ingredient != null)
            {
                message = "";
                DescribeIngredient(ingredient);
            }
            return new MarioResult { Success = (ingredient != null), Message = message };
        }
    }
}
