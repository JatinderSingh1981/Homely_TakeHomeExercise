using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.API
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var returnValue = enumValue.GetType()?
                            .GetMember(enumValue.ToString())?
                            .First()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name;

            if (returnValue == null)
            {
                returnValue = enumValue.ToString();
            }
            return returnValue;
        }
    }
}
