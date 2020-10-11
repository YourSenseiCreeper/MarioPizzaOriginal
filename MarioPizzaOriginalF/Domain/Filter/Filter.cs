using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class Filter<T>
    {
        public List<FilterObject> FilterObjects { get; }
        public Filter(IConsole console, List<FilterObject> filterObjects)
        {
            var filterHelper = new FilterHelper(console);
            _repository = TinyIoCContainer.Current.Resolve<IRepository<T>>();
            FilterObjects = filterObjects;
            _filterTypeAction = new Dictionary<string, Func<string, object[], object>>
            {
                {"int32", filterHelper.FilterInt},
                {"double", filterHelper.FilterDouble},
                {"string", filterHelper.FilterString},
                {"datetime", filterHelper.FilterDateTime},
                {"unitofmeasure", filterHelper.FilterOption<UnitOfMeasure>},
                {"orderpriority", filterHelper.FilterOption<OrderPriority>},
                {"orderstatus", filterHelper.FilterOption<OrderStatus>}
            };
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
                else FilterExecutor(FilterObjects[selection]);
            } while (selection != FilterObjects.Count);
            return true;
        }
        
        /// <summary>
        /// <typeparamref name="T"/> is strictly related to table name!
        /// </summary>
        /// <returns></returns>
        public List<T> Query()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"select * from \"{typeof(T).Name}\"");
            //int activeFilters = FilterObjects.Aggregate(0, (active, next) => next.Value != null ? active++ : active, result => result);
            int activeFil = FilterObjects.Count(filter => filter.Value != null);
            if (activeFil > 0)
            {
                queryBuilder.Append(" WHERE ");
                var conditions = FilterObjects.Where(filter => filter.Value != null).Select(filter => filter.ToQueryString());
                queryBuilder.Append(string.Join(" AND ", conditions));
            }
            var results = _repository.Query(queryBuilder.ToString());
            return results;
        }

        public void Clear() => FilterObjects.ForEach(filter => filter.Value = null);


        private void FilterExecutor(FilterObject filterObject)
        {
            var filterType = filterObject.FilterType.Name.ToLower();
            var args = new[] { filterObject.Value };
            filterObject.Value = _filterTypeAction.ContainsKey(filterType) ? 
                _filterTypeAction[filterType](filterObject.FilterMessage, args) : 
                _filterTypeAction["string"](filterObject.FilterMessage, args);
        }

        private void PrepareMenu()
        {
            var index = 0;
            FilterObjects.ForEach(filter => Console.WriteLine($"{index + 1}. {FilterObjects[index++].ToMenuString()}"));
            Console.WriteLine($"{index++ + 1}. WYŚWIETL WYNIKI");
            Console.WriteLine($"{index + 1}. Wyjdź");
        }

        private readonly IRepository<T> _repository;
        private readonly Dictionary<string, Func<string, object[], object>> _filterTypeAction;
    }
}
