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
                case "int32": return ViewHelper.FilterInt(FilterMessage, Value, Args);
                case "double": return ViewHelper.FilterDouble(FilterMessage, Args);
                case "string": return ViewHelper.FilterString(FilterMessage, Args);
                case "datetime": return ViewHelper.FilterDateTime(FilterMessage, Args);
                case "unitofmeasure": return ViewHelper.FilterOption<UnitOfMeasure>(FilterMessage, Args);
                case "orderpriority": return ViewHelper.FilterOption<OrderPriority>(FilterMessage, Args);
                case "orderstatus": return ViewHelper.FilterOption<OrderStatus>(FilterMessage, Args);
                default: return ViewHelper.FilterString(FilterMessage, Args);
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
            else return string.Format(QueryString, Value);
        }
    }
}
