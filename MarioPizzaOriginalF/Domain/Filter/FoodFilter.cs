using System.Collections.Generic;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class FoodFilter : Filter<Food>
    {
        public FoodFilter(TinyIoCContainer container) : base(container)
        {
            FilterObjects = new List<FilterObject>
            {
                new FilterObject ( "Minimalne Id produktu", "Podaj wartość dla Minimalne Id produktu: ", "FoodId >= {0}", typeof(int), new object[]{ -1 }),
                new FilterObject ( "Maksymalne Id produktu", "Podaj wartość dla Maksymalne Id produktu: ", "FoodId <= {0}", typeof(int)),
                new FilterObject ( "Nazwa produktu zawiera", "Podaj ciąg który znajduje się w Nazwie produktu: ", "FoodName like '%{0}%'", typeof(string)),
                new FilterObject ( "Minmalna cena netto", "Podaj minimalną cenę netto produktu: ", "NettPrice >= {0}", typeof(double)),
                new FilterObject ( "Maksymalna cena netto", "Podaj maksymalną cenę netto produktu: ", "NettPrice <= {0}", typeof(double)),
                new FilterObject ( "Minmalna cena", "Podaj minimalną cenę produktu: ", "Price >= {0}", typeof(double)),
                new FilterObject ( "Maksymalna cena", "Podaj maksymalną cenę produktu: ", "Price <= {0}", typeof(double)),
                new FilterObject ( "Minmalna waga", "Podaj minimalną wagę produktu: ", "Weight > {0}", typeof(double)),
                new FilterObject ( "Maksymalna waga", "Podaj maksymalną wagę produktu: ", "Weight < {0}", typeof(double)),
                new FilterObject ( "Minimalny czas produkcji", "Podaj minimalny czas produkcji w sekundach bez przecinka: ", "ProductionTime > {0}", typeof(int)),
                new FilterObject ( "Maksymalny czas produkcji", "Podaj maksymalny czas produkcji w sekundach bez przecinka: ", "ProductionTime < {0}", typeof(int)),
            };
        }
    }
}
