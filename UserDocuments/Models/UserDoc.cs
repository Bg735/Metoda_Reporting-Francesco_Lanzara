using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace UserDocuments.Models
{

    public class UserDoc
    {
        public string Id { get; set; } = default!;
        public Guid UserId { get; set; }
        public string FileNameOriginal { get; set; } = default!;
        public string StoragePath { get; set; } = default!;
        public long Size { get; set; }
        public string MimeType { get; set; } = "application/octet-stream";
        public DateTime CreatedAtUtc { get; set; }
    }

    public class UserDocsDbContext : DbContext
    {
        public DbSet<UserDoc> UserDocs => Set<UserDoc>();

        public UserDocsDbContext(DbContextOptions<UserDocsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<UserDoc>().HasKey(x => x.Id);
            b.Entity<UserDoc>().HasIndex(x => new { x.UserId, x.CreatedAtUtc });
            b.Entity<UserDoc>().Property(x => x.StoragePath).IsRequired();
            b.Entity<UserDoc>().Property(x => x.FileNameOriginal).IsRequired();
        }
    }

    // Central catalog for report types used across the solution
    public sealed record DocumentType(string Title, string ApiName)
    {
        //public string FileName { get; init; } = DocumentTypes.TitleToFileName(Title);
        public string FileName
        {
            get
            {
                var value = DocumentTypes.TitleToFileName(Title);
                Console.WriteLine(value);
                return value;
            }
        }
    }

    public static class DocumentTypes
    {
        public static readonly DocumentType[] All = new[]
        {
            new DocumentType("Report Analitico per Controparte", "MonthlyReport"),
            new DocumentType("Segnalazione Inadempienze Probabili per Soggetto", "ReportingUnlikelyToPayBySubject"),
            new DocumentType("Garanzie Connesse con Operazioni di Natura Commerciale", "GuaranteesConnectedWithOperationsOfACommercialNature"),
            new DocumentType("Sofferenze Segnalate con Altri Cubi", "SufferingsReportedWithOtherPhenomena"),
            new DocumentType("Accordato Maggiore di Utilizzato o Senza Utilizzato", "GrantedMajorUsedOrNonUsed"),
            new DocumentType("Altri Crediti ma Deteriorati", "OtherCreditsButImpaired"),
            new DocumentType("Sconfinamenti Operativi", "Trespassing"),
            new DocumentType("Assenza Censito Collegato", "AbsenceRegisteredConnected"),
            new DocumentType("Accordato Diverso da Utilizzato per Operazioni in Pool", "AgreedOtherThanUsedForPooledTransactions"),
            new DocumentType("Riepilogo Prospetto Andamentale", "SummaryOfPerformanceStatement"),
            new DocumentType("Incongruenza tra durata originaria e residua", "InconsistencyBetweenOriginalAndResidualDuration"),
            new DocumentType("Sconfinamenti operativi - analitico", "OperationalOverruns/Analitics"),
            new DocumentType("Sconfinamenti operativi - sintetico", "OperationalOverruns/Sintetics"),
            new DocumentType("Presenza di pool capofila e non pool totale o viceversa", "PresenceOfLeadPoolAndNotTotalPoolOrViceVersa"),
            new DocumentType("Tipo attività incompatibile con rischi a scadenza", "TypeOfActivityIncompatibleWithRisksAtMaturity"),
        };

        // Simple matcher: strip extension and check StartsWith against the precomputed FileName
        public static bool TryFromFileName(string? fileName, out DocumentType? type)
        {
            type = null;
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            var name = fileName;
            var dot = name.LastIndexOf('.');
            if (dot > 0) name = name[..dot];

            type = All.FirstOrDefault(t => name.StartsWith(t.FileName, StringComparison.OrdinalIgnoreCase));
            return type != null;
        }

        public static string TitleToFileName(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return string.Empty;
            var upper = RemoveDiacritics(title).ToUpperInvariant();
            var sb = new StringBuilder(upper.Length);
            var lastUnderscore = false;
            foreach (var ch in upper)
            {
                if (char.IsWhiteSpace(ch))
                {
                    if (!lastUnderscore)
                    {
                        sb.Append('_');
                        lastUnderscore = true;
                    }
                }
                else
                {
                    sb.Append(ch);
                    lastUnderscore = false;
                }
            }
            return sb.ToString().Trim('_');
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var norm = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(norm.Length + 8);

            bool lastBaseWasVowel = false;
            bool apostropheAddedForCurrentBase = false;

            foreach (var ch in norm)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat == UnicodeCategory.NonSpacingMark)
                {
                    if (lastBaseWasVowel && !apostropheAddedForCurrentBase && IsAccentMark(ch))
                    {
                        sb.Append('’'); // Trasforma vocali accentate in A’, E’, I’, O’, U’
                        apostropheAddedForCurrentBase = true;
                    }
                    continue; // scarta il diacritico
                }

                // nuovo carattere base
                lastBaseWasVowel = IsLatinVowel(ch);
                apostropheAddedForCurrentBase = false;
                sb.Append(ch);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);

            static bool IsLatinVowel(char c)
            {
                c = char.ToUpperInvariant(c);
                return c is 'A' or 'E' or 'I' or 'O' or 'U';
            }

            static bool IsAccentMark(char c)
            {
                // Considera gli accenti tipici italiani: grave (̀ U+0300) e acuto (́ U+0301)
                return c == '\u0300' || c == '\u0301';
            }
        }
    }

}
