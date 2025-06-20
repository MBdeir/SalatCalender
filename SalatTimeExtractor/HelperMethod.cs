using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalatTimeExtractor;

public static class HelperMethods
{
    public static DateTime? String2DateTime(string time)
    {
        if (DateTime.TryParse(time, out var datetime))
            return datetime;
        return null;
    }
}
