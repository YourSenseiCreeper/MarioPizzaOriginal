using System;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class FilterObject
    {
        public string MenuMessage { get; set; }
        public string FilterMessage { get; set; }
        public string QueryString { get; set; }
        public object Value { get; set; }
        public Type FilterType { get; set; }
        public object[] Args { get; set; }

        public FilterObject(string menuMessage, string filterMessage, string queryString,
            Type filterType, object[] args = null)
        {
            MenuMessage = menuMessage;
            FilterMessage = filterMessage;
            QueryString = queryString;
            FilterType = filterType;
            Args = args;
        }

        public string ToMenuString()
        {
            if (Value == null) return MenuMessage;
            return $"{MenuMessage} ({Convert.ChangeType(Value, FilterType)})";
        }

        /// <summary>
        /// Null value check is above in implementation
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            string parameter;
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
    }
}
