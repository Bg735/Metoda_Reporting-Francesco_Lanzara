using System.Reflection.Metadata;
using System.Text.Json;

namespace Global
{
    public static class Utils
    {
        public static class Domain
        {
            public static readonly string Host;
            public const string schema = "http";
            public const int port = 8080;

            static Domain()
            {
                //Con questa configurazione, è possibile reperire da fonti esterne il dominio
                Host = "localhost";
            }

            public static string UrlOf(string path) => $"{Root}/{path}";
            public static string PathOf(string subpath) => $"{Host}/{subpath}";

            public static string Root => $"{schema}://{Host}:{port}";
        }

    }
}