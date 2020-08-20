using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarioPizzaOriginal.Domain.DataAccess;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class Filter<T>
    {
        public List<FilterObject> FilterObjects { get; set; }
        public Filter(TinyIoCContainer container)
        {
            _repository = container.Resolve<IRepository<T>>();
        }

        public bool FilterMenu()
        {
            var selection = 0;
            do
            {
                Console.Clear();
                PrepareMenu();
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var option))
                {
                    Console.WriteLine($"{input} nie jest liczbą!");
                    continue;
                }
                selection = option - 1;
                if (selection > FilterObjects.Count + 1 || selection < 0)
                {
                    Console.WriteLine($"Nie ma filtra nr {selection}!)");
                }
                else if (selection == FilterObjects.Count + 1) return false;
                else if (selection == FilterObjects.Count) return true;
                else FilterObjects[selection].Filter();
            } while (selection != FilterObjects.Count);
            return true;
        }
        
        public List<T> Query()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"select * from \"{typeof(T).Name}\"");
            //int activeFilters = FilterObjects.Aggregate(0, (active, next) => next.Value != null ? active++ : active, result => result);
            int activeFil = 0;
            foreach(var filter in FilterObjects)
            {
                if (filter.Value != null) activeFil++;
            }
            if (activeFil > 0)
            {
                queryBuilder.Append(" WHERE ");
                var conditions = FilterObjects.Where(filter => filter.Value != null).Select(filter => filter.ToQueryString());
                queryBuilder.Append(string.Join(" AND ", conditions));
            }
            var results = _repository.Query(queryBuilder.ToString());
            return results;
        }
        private void PrepareMenu()
        {
            var index = 0;
            FilterObjects.ForEach(filter => Console.WriteLine($"{index + 1}. {FilterObjects[index++].ToMenuString()}"));
            Console.WriteLine($"{index++ + 1}. WYŚWIETL WYNIKI");
            Console.WriteLine($"{index + 1}. Wyjdź");
        }

        private readonly IRepository<T> _repository;
    }
}
