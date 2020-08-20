using System.Collections.Generic;
using MarioPizzaOriginal.Domain.Enums;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class IngredientFilter : Filter<Ingredient>
    {
        public IngredientFilter(TinyIoCContainer container) : base(container)
        {
            FilterObjects = new List<FilterObject>
            {
                new FilterObject ( "Minimalne Id składnika", "Podaj wartość dla Minimalne Id składnika: ", "IngredientId > {0}", typeof(int), new object[]{ -1 }),
                new FilterObject ( "Maksymalne Id składnika", "Podaj wartość dla Maksymalne Id składnika: ", "IngredientId < {0}", typeof(int)),
                new FilterObject ( "Nazwa składnika zawiera", "Podaj ciąg który znajduje się w Nazwie produktu: ", "IngredientName like '%{0}%'", typeof(string)),
                new FilterObject ( "Jednostka miary", "Wybierz jednostkę miary: ", "UnitOfMeasureType = {0}", typeof(UnitOfMeasure))
            };
        }
    }
}
