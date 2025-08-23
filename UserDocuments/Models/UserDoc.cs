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

    public sealed class DocumentContent
    {
        public string Title { get; }
        public string ApiName { get; }
        public string FileName { get; }

        private DocumentContent(string title, string apiName, string fileName)
        {
            Title = title;
            ApiName = apiName;
            FileName = fileName;
        }

        public static readonly DocumentContent MonthlyReport =
            new("Report Analitico per Controparte", "MonthlyReport", "REPORT_ANALITICO_PER_CONTROPARTE");

        public static readonly DocumentContent ReportingUnlikelyToPayBySubject =
            new("Segnalazione Inadempienze Probabili per Soggetto", "ReportingUnlikelyToPayBySubject", "SEGNALAZIONE_INADEMPIENZE_PROBABILI_PER_SOGGETTO");

        public static readonly DocumentContent GuaranteesConnectedWithOperationsOfACommercialNature =
            new("Garanzie Connesse con Operazioni di Natura Commerciale", "GuaranteesConnectedWithOperationsOfACommercialNature", "GARANZIE_CONNESSE_CON_OPERAZIONI_DI_NATURA_COMMERCIALE");

        public static readonly DocumentContent SufferingsReportedWithOtherPhenomena =
            new("Sofferenze Segnalate con Altri Cubi", "SufferingsReportedWithOtherPhenomena", "SOFFERENZE_SEGNALATE_CON_ALTRI_CUBI");

        public static readonly DocumentContent GrantedMajorUsedOrNonUsed =
            new("Accordato Maggiore di Utilizzato o Senza Utilizzato", "GrantedMajorUsedOrNonUsed", "ACCORDATO_MAGGIORE_DI_UTILIZZATO_O_SENZA_UTILIZZATO");

        public static readonly DocumentContent OtherCreditsButImpaired =
            new("Altri Crediti ma Deteriorati", "OtherCreditsButImpaired", "ALTRI_CREDITI_MA_DETERIORATI");

        public static readonly DocumentContent AbsenceRegisteredConnected =
            new("Assenza Censito Collegato", "AbsenceRegisteredConnected", "ASSENZA_CENSITO_COLLEGATO");

        public static readonly DocumentContent SummaryOfPerformanceStatement =
            new("Riepilogo Prospetto Andamentale", "SummaryOfPerformanceStatement", "RIEPILOGO_PROSPETTO_ANDAMENTALE");

        public static readonly DocumentContent InconsistencyBetweenOriginalAndResidualDuration =
            new("Incongruenza tra durata originaria e residua", "InconsistencyBetweenOriginalAndResidualDuration", "INCONGRUENZA_TRA_DURATA_ORIGINARIA_E_RESIDUA");

        public static readonly DocumentContent OperationalOverrunsAnalitics =
            new("Sconfinamenti Operativi analitico", "OperationalOverruns/Analitics", "SCONFINAMENTI_OPERATIVI_ANALITICO");

        public static readonly DocumentContent OperationalOverrunsSintetics =
            new("Sconfinamenti Operativi sintetico", "OperationalOverruns/Sintetics", "SCONFINAMENTI_OPERATIVI_SINTETICO");

        public static readonly DocumentContent PresenceOfLeadPoolAndNotTotalPoolOrViceVersa =
            new("Presenza di pool capofila e non pool totale o viceversa", "PresenceOfLeadPoolAndNotTotalPoolOrViceVersa", "PRESENZA_DI_POOL_CAPOFILA_E_NON_POOL_TOTALE_O_VICEVERSA");

        public static readonly DocumentContent TypeOfActivityIncompatibleWithRisksAtMaturity =
            new("Tipo attività incompatibile con rischi a scadenza", "TypeOfActivityIncompatibleWithRisksAtMaturity", "TIPO_ATTIVITA’_INCOMPATIBILE_CON_RISCHI_A_SCADENZA");
     
        public static readonly DocumentContent Trepassing =
            new("Sconfinamenti", "Trepassing", "SCONFINAMENTI");

        public static IEnumerable<DocumentContent> All =>
        [
            MonthlyReport,
            ReportingUnlikelyToPayBySubject,
            GuaranteesConnectedWithOperationsOfACommercialNature,
            SufferingsReportedWithOtherPhenomena,
            GrantedMajorUsedOrNonUsed,
            OtherCreditsButImpaired,
            AbsenceRegisteredConnected,
            SummaryOfPerformanceStatement,
            InconsistencyBetweenOriginalAndResidualDuration,
            OperationalOverrunsAnalitics,
            OperationalOverrunsSintetics,
            PresenceOfLeadPoolAndNotTotalPoolOrViceVersa,
            TypeOfActivityIncompatibleWithRisksAtMaturity,
            Trepassing
        ];

        public static bool TryFromFileName(string? fileName, out DocumentContent? result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            int pos = fileName.IndexOf('|');
            var name= fileName[..pos];

            result = All.FirstOrDefault(t =>
                name.StartsWith(t.FileName, StringComparison.OrdinalIgnoreCase));

            return result is not null;
        }
    }
}
