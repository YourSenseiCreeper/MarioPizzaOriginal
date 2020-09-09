using MarioPizzaOriginal.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class FilterObject
    {
        public string Message { get; set; }
        public string FilterMessage { get; set; }
        public string QueryString { get; set; }
        public object Value { get; set; }
        public Type FilterType { get; set; }
        public object[] Args { get; set; }

        public FilterObject(string filterMenuMessage, string filterMessage, string queryString,
            Type filterType, object[] args = null)
        {
            Message = filterMenuMessage;
            FilterMessage = filterMessage;
            QueryString = queryString;
            FilterType = filterType;
            Args = args;
        }

        public void Filter() => Value = FunctionSelector();

        public string ToMenuString()
        {
            if (Value == null) return Message;
            return $"{Message} ({Convert.ChangeType(Value, FilterType)})";
        }

        /// <summary>
        /// Null value check is above in implementation
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            var parameter = string.Empty;
            if (typeof(Enum).IsAssignableFrom(FilterType))
            {
                var enumValue = (int) Enum.Parse(FilterType, Value.ToString());
                parameter = enumValue.ToString();
            }
            else if (FilterType == typeof(double)) parameter = Value.ToString().Replace(",", ".");
            else if (FilterType == typeof(DateTime)) parameter = ((DateTime)Value).ToString("s");
            else if (FilterType == typeof(int)) parameter = Value.ToString();
            else parameter = (string)Value;
            return string.Format(QueryString, parameter);
        }


        private object FunctionSelector()
        {
            var filterType = FilterType.Name.ToLower();
            var args = new[] { Value };
            if (filterTypeActionMapper.ContainsKey(filterType))
            {
                return filterTypeActionMapper[filterType](FilterMessage, args);
            }
            return filterTypeActionMapper["string"](FilterMessage, args);
        }

        private readonly Dictionary<string, Func<string, object[], object>> filterTypeActionMapper =
            new Dictionary<string, Func<string, object[], object>>
            {
                {"int32", FilterHelper.FilterInt},
                {"double", FilterHelper.FilterDouble},
                {"string", FilterHelper.FilterString},
                {"datetime", FilterHelper.FilterDateTime},
                {"unitofmeasure", FilterHelper.FilterOption<UnitOfMeasure>},
                {"orderpriority", FilterHelper.FilterOption<OrderPriority>},
                {"orderstatus", FilterHelper.FilterOption<OrderStatus>}
            };
    }
}
