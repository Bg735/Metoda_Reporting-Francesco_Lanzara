using NPOI.SS.UserModel;
using System;

namespace Metoda.Reporting.Excel.Helpers;

public static class Utils
{
    public static CellType GetCellType(Type type)
    {
        var result = CellType.Unknown;

        if (type == typeof(string))
        {
            result = CellType.String;
        }

        else if (type == typeof(bool))
        {
            result = CellType.Boolean;
        }
        else if (type == typeof(int) ||
               type == typeof(uint) ||
               type == typeof(long) ||
               type == typeof(ulong) ||
               type == typeof(short) ||
               type == typeof(ushort) ||
               type == typeof(float) ||
               type == typeof(double) ||
               type == typeof(decimal))
        {
            result = CellType.Numeric;
        }
        else if (type == typeof(int?) ||
              type == typeof(uint?) ||
              type == typeof(long?) ||
              type == typeof(ulong?) ||
              type == typeof(short?) ||
              type == typeof(ushort?) ||
              type == typeof(float?) ||
              type == typeof(double?) ||
              type == typeof(decimal?))
        {
            result = CellType.Numeric;
        }

        return result;
    }
 
    public static CellType GetCellType(object value)
    {
        if (value == null)
            return CellType.Unknown;

        return GetCellType(value.GetType());
    }
}