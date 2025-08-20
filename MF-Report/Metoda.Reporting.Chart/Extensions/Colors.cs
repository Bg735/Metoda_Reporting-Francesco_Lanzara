using System.Drawing;

namespace Metoda.Reporting.Chart.Extensions;

public static class ColorsExt
{
    public static Color GetForegroundColorForBackground(this Color backgroundColor)
    {
        return backgroundColor.G < 128 ? Color.White : Color.Black;
    }
}