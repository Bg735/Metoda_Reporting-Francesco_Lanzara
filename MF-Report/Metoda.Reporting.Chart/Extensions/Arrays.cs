using System;

namespace Metoda.Reporting.Chart.Extensions;
public static class ArraysExt
{
    public static T[,] ConvertOneDimArrayToTwoDim<T>(this T[] srcData, int numColumns)
    {
         int numRows = (int)Math.Ceiling((double)srcData.Length / numColumns);

        T[,] heatmapData = new T[numRows, numColumns];

        int dataIndex = 0;
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (dataIndex < srcData.Length)
                {
                    heatmapData[row, col] = srcData[dataIndex];
                    dataIndex++;
                }
                else
                {
                    heatmapData[row, col] = default;
                }
            }
        }
        return heatmapData;
    }
}
