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
        public static string MapEnum(Enum enumValue)
        {
            return enumValue.GetDisplayName();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            var returnValue = enumValue != null ? (enumValue.GetType()?
                            .GetMember(enumValue.ToString())?
                            .First()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name) : "";

            if (returnValue == null)
            {
                returnValue = enumValue.ToString();
            }
            return returnValue;
        }


        public static StringBuilder GenerateKey(this StringBuilder stringBuilder, object obj)
        {
            StringBuilder builder = new StringBuilder();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(obj) != null)
                {
                    if (builder.Length > 1)
                        builder.Append("_");
                    builder.Append(property.Name);
                    builder.Append(property.GetValue(obj));
                }
            }
            return builder;
        }
    }
}
