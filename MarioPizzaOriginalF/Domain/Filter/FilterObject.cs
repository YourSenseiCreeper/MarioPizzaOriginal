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
                case "int32": return ViewHelper.FilterInt(FilterMessage, Args);
                case "double": return ViewHelper.FilterDouble(FilterMessage, Args);
                case "string": return ViewHelper.FilterString(FilterMessage, Args);
                case "datetime": return ViewHelper.FilterDateTime(FilterMessage, Args);
                default: return ViewHelper.FilterOption(FilterType, FilterMessage, Args);
            }
        }
        /// <summary>
        /// Null value check is above in implementation
        /// </summary>
        /// <returns></returns>
        public string ToQueryString() => string.Format(QueryString,Value);
    }
}
