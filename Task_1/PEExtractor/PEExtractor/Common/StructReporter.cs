﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Common
{
    //TODO : the report method is incorrect with structs of structs..


    public static class StructReporter
    {
        /// <summary>
        /// Report POD structures.
        /// </summary>
        /// <param name="type">structure type</param>
        /// <param name="obj">value</param>
        /// <param name="sw">stream</param>
        /// <param name="tabsOffset">left offset</param>
        /// <param name="fieldsToSkip">parameters which should be skipped</param>
        public static void Report(Type type, object obj, StreamWriter sw, int tabsOffset = 0, SortedSet<string> fieldsToSkip = null)
        {
            if (!type.IsValueType || type.IsPrimitive)
            {
                throw new ArgumentException("Type must me struct", nameof(type));
            }
            foreach (var field in type.GetFields())
            {
                if (fieldsToSkip != null && fieldsToSkip.Contains(field.Name)) continue;
                if (field.FieldType.IsValueType && !field.FieldType.IsPrimitive)
                {
                    sw.WriteLine(TabOffset(tabsOffset) + $"{field.Name}: ");
                    Report(field.FieldType, field.GetValue(obj), sw, tabsOffset + 1);
                    continue;
                }

                var value = field.GetValue(obj);
                string strValue;
                if (value is UInt16 || value is Int16)
                {
                    value = $"0x{value:X4}";
                } else if (value is UInt32 || value is Int32)
                {
                    value = $"0x{value:X8}";
                }
                else if (value is UInt64 || value is Int64)
                {
                    value = $"0x{value:X16}";
                }
                else if (value is byte)
                {
                    value = $"0x{value:X2}";
                }
                else
                {
                    strValue = value.ToString();
                }

                sw.WriteLine(TabOffset(tabsOffset) + $"{field.Name} : {value}");
            }
        }

        /// <summary>
        /// Report POD structures.
        /// </summary>
        /// <param name="t">value</param>
        /// <param name="sw">stream</param>
        /// <param name="tabsOffset">left offset</param>
        /// <param name="fieldsToSkip">parameters which should be skipped</param>
        public static void Report<T>(T t, StreamWriter sw, int tabsOffset = 0, SortedSet<string> fieldsToSkip = null) where T : struct
        {
            Report(typeof(T), t, sw, tabsOffset, fieldsToSkip);
        }


        private static string TabOffset(int count)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++) sb.Append("\t");
            return sb.ToString();
        }
    }
}
