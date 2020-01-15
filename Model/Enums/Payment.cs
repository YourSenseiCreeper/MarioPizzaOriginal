using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enums
{
    [EnumAsInt]
    public enum Payment
    {
        AT_PLACE,FOREWARD,DELIVERY,NONE
    }
}
