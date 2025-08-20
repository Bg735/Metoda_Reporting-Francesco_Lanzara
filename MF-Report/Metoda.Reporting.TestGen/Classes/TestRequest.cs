using Metoda.Reporting.TestGen.Enums;

namespace Metoda.Reporting.TestGen.Classes
{
    public class TestRequest
    {
        public string CodModello { get; set; }
        public FileExtensions Estensione { get; set; } = FileExtensions.Pdf;
    }
}
