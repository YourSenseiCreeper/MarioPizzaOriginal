using MarioPizzaOriginal.Domain.Enums;
using System;

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

        private object FunctionSelector()
        {
            switch (FilterType.Name.ToLower())
            {
                case "int32": return FilterHelper.FilterInt(FilterMessage, Value, Args);
                case "double": return FilterHelper.FilterDouble(FilterMessage, Args);
                case "string": return FilterHelper.FilterString(FilterMessage, Args);
                case "datetime": return FilterHelper.FilterDateTime(FilterMessage, Args);
                case "unitofmeasure": return FilterHelper.FilterOption<UnitOfMeasure>(FilterMessage, Args);
                case "orderpriority": return FilterHelper.FilterOption<OrderPriority>(FilterMessage, Args);
                case "orderstatus": return FilterHelper.FilterOption<OrderStatus>(FilterMessage, Args);
                default: return FilterHelper.FilterString(FilterMessage, Args);
            }
        }
        /// <summary>
        /// Null value check is above in implementation
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            if (typeof(Enum).IsAssignableFrom(FilterType))
            {
                var enumValue = Enum.Parse(FilterType, Value.ToString());
                return string.Format(QueryString, (int) enumValue);
            }
            else if (FilterType == typeof(double)) return string.Format(QueryString, Value.ToString().Replace(",", "."));
            else return string.Format(QueryString, Value);
        }
    }
}
