using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Amphibian.Utility
{
    static class EnumHelper
    {
        public static List<T> GetValues<T> ()
        {
            Type currentEnum = typeof(T);
            List<T> resultSet = new List<T>();
            if (currentEnum.IsEnum) {
                FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                    resultSet.Add((T)field.GetValue(null));
            }

            return resultSet;
        }
    }
}
