using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout.Element;
using Metoda.Reporting.Chart.Plots.Bar;
using Metoda.Reporting.Chart.Plots.BarSeries;
using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Chart.Plots.Base64Image;
using Metoda.Reporting.Chart.Plots.Heatmap;
using Metoda.Reporting.Chart.Plots.Pie;
using Metoda.Reporting.Chart.Plots.Scatter;
using Metoda.Reporting.Chart.Plots.Signal;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Common.Reports.Contracts;
using Metoda.Reporting.Common.Res;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.Reports;
using Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;
using Metoda.Reporting.Models.Reports.AgreedOtherThanUsedForPooledTransactions;
using Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;
using Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;
using Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;
using Metoda.Reporting.Models.Reports.MonthlyReport;
using Metoda.Reporting.Models.Reports.OperationalOverruns;
using Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;
using Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;
using Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;
using Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;
using Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;
using Metoda.Reporting.Models.Reports.Trespassing;
using Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;
using Metoda.Reporting.Pdf.Builders;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.Reports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metoda.Reporting.TestGen
{
    public partial class MainForm : Form
    {
        readonly string dataFolder = Path.Combine(AppContext.BaseDirectory, "data");

        public MainForm()
        {
            InitializeComponent();

            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
        }

        private void BtnOpenDestFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", dataFolder);
        }

        private async Task ReportGeneratorAsync(
         IReport report,
         Button reportRunBtn,
         string destFilePath,
         int initPdfProgress)
        {
            try
            {
                reportRunBtn.Enabled = false;

                progressBarPdf.Value = 0;
                lblProgress.Text = $"Progress: {progressBarPdf.Value}%";

                progressBarPdf.Value = initPdfProgress;
                lblProgress.Text = $"Progress: {progressBarPdf.Value}%";

                await report.ToFileAsync($"{destFilePath}")
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            throw task.Exception?.InnerExceptions[0];
                        }

                        if (task.IsCompleted)
                        {
                            report.Progress?.Report(1f);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                MessageBox.Show("Report generation has been completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarPdf.Value = 0;
                lblProgress.Text = $"Progress: {progressBarPdf.Value}%";
                reportRunBtn.Enabled = true;
            }
        }

        private void ReportGenerator(
        IReport report,
        Button reportRunBtn,
        string destFilePath,
        int initPdfProgress)
        {
            try
            {
                reportRunBtn.Enabled = false;

                progressBarPdf.Value = 5;
                lblProgress.Text = $"Progress: {progressBarPdf.Value}%";

                progressBarPdf.Value = initPdfProgress;
                lblProgress.Text = $"Progress: {progressBarPdf.Value}%";

                report.ToFile($"{destFilePath}");

                report.Progress?.Report(1f);

                MessageBox.Show("Pdf generation has been completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarPdf.Value = 0;
                lblProgress.Text = $"Progress: {progressBarPdf.Value}%";
                reportRunBtn.Enabled = true;
            }
        }

        private IReportProgress GetReportProgress(int initProgressValue)
        {
            return new ReportProgress(value =>
            {
                float percentage = value * 100;
                progressBarPdf.Value = (int)percentage;
                lblProgress.Text = $"Progress: {percentage: 0.00}%";
            }, initProgressValue / 100f);
        }

        /// <summary>
        /// ACCORDATO DIVERSO DA UTILIZZATO PER OPERAZIONI IN POOL
        /// </summary>
        private void BtnAgreedOtherThanUsedForPooledTransactions_Click(object sender, EventArgs e)
        {
            // Init Pdf report 
            string filePath = $"{dataFolder}/ACCORDATO_DIVERSO_DA_UTILIZZATO_PER_OPERAZIONI_IN_POOL_{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new AgreedOtherThanUsedForPooledTransactionsPdfReportBuilder(progress: GetReportProgress(initPdfProgress));

            AgreedOtherThanUsedForPooledTransactionsFakeData.FillBuilderByData(builderPdf);

            var reportPdf = builderPdf.Build();

            // Generate the report
            ReportGenerator(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init Excel report 
            filePath = $"{dataFolder}/ACCORDATO_DIVERSO_DA_UTILIZZATO_PER_OPERAZIONI_IN_POOL_{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.xlsx";
            var initExcelProgress = 0;

            var builderExcel = new AgreedOtherThanUsedForPooledTransactionsExcelReportBuilder(progress: GetReportProgress(initExcelProgress));

            AgreedOtherThanUsedForPooledTransactionsFakeData.FillBuilderByData(builderExcel);

            var reportExcel = builderExcel.Build();

            // Generate the report
            ReportGenerator(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// ACCORDATO MAGGIORE DI UTILIZZATO O SENZA UTILIZZATO
        /// </summary>
        private async void BtnGrantedMajorUsedOrNonUsed_Click(object sender, EventArgs e)
        {
            // Init the Pdf report 
            string filePath = $"{dataFolder}/ACCORDATO_MAGGIORE_DI_UTILIZZATO_O_SENZA_UTILIZZATO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new GrantedMajorUsedOrNonUsedPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            GrantedMajorUsedOrNonUsedFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report 
            filePath = $"{dataFolder}/ACCORDATO_MAGGIORE_DI_UTILIZZATO_O_SENZA_UTILIZZATO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new GrantedMajorUsedOrNonUsedExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            GrantedMajorUsedOrNonUsedFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// ALTRI CREDITI MA DETERIORATI
        /// </summary>
        private async void BtnOtherCreditsButImpaired_Click(object sender, EventArgs e)
        {
            // Init the Pdf report 
            string filePath = $"{dataFolder}/ALTRI_CREDITI_MA_DETERIORATI_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new OtherCreditsButImpairedPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            OtherCreditsButImpairedFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init Excel report 
            filePath = $"{dataFolder}/ALTRI_CREDITI_MA_DETERIORATI_{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.xlsx";
            var initExcelProgress = 0;

            var builderExcel = new OtherCreditsButImpairedExcelReportBuilder(progress: GetReportProgress(initExcelProgress));

            OtherCreditsButImpairedFakeData.FillBuilderByData(builderExcel);

            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// ASSENZA CENSITO COLLEGATO
        /// </summary>
        private async void BtnAbsenceRegisteredConnected_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/ASSENZA_CENSITO_COLLEGATO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new AbsenceRegisteredConnectedPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            AbsenceRegisteredConnectedFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/ASSENZA_CENSITO_COLLEGATO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new AbsenceRegisteredConnectedExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            AbsenceRegisteredConnectedFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// GARANZIE CONNESSE CON OPERAZIONI DI NATURA COMMERCIALE
        /// </summary>
        private async void BtnGuaranteesConnectedWithOperationsOfACommercialNature_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/GARANZIE_CONNESSE_CON_OPERAZIONI_DI_NATURA_COMMERCIALE_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            GuaranteesConnectedWithOperationsOfACommercialNatureFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/GARANZIE_CONNESSE_CON_OPERAZIONI_DI_NATURA_COMMERCIALE_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            GuaranteesConnectedWithOperationsOfACommercialNatureFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// INCONGRUENZA TRA DURATA ORIGINARIA E RESIDUA
        /// </summary>
        private async void BtnInconsistencyBetweenOriginalAndResidualDuration_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/INCONGRUENZA_TRA_DURATA_ORIGINARIA_E_RESIDUA_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            InconsistencyBetweenOriginalAndResidualDurationFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();
            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/INCONGRUENZA_TRA_DURATA_ORIGINARIA_E_RESIDUA_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            InconsistencyBetweenOriginalAndResidualDurationFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// PRESENZA DI POOL CAPOFILA E NON POOL TOTALE O VICEVERSA
        /// </summary>
        private async void BtnPresenceOfLeadPoolAndNotTotalPoolOrViceVersa_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/PRESENZA_DI_POOL_CAPOFILA_E_NON_POOL_TOTALE_O_VICEVERSA_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            PresenceOfLeadPoolAndNotTotalPoolOrViceVersaFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/PRESENZA_DI_POOL_CAPOFILA_E_NON_POOL_TOTALE_O_VICEVERSA_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            PresenceOfLeadPoolAndNotTotalPoolOrViceVersaFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// RIEPILOGO PROSPETTO ANDAMENTALE
        /// </summary>
        private async void BtnSummaryOfPerformanceStatement_Click(object sender, EventArgs e)
        {
            string filePath = $"{dataFolder}/RIEPILOGO_PROSPETTO_ANDAMENTALE_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new SummaryOfPerformanceStatementPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            SummaryOfPerformanceStatementFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/RIEPILOGO_PROSPETTO_ANDAMENTALE_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new SummaryOfPerformanceStatementExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            SummaryOfPerformanceStatementFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// SCONFINAMENTI
        /// </summary>
        private async void BtnTrespassing_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/SCONFINAMENTI_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new TrespassingPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            TrespassingFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/SCONFINAMENTI_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new TrespassingExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            TrespassingFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// SCONFINAMENTI OPERATIVI - SINTETICO
        /// </summary>
        private async void BtnOperationalOverrunsSintetics_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/SCONFINAMENTI_OPERATIVI__SINTETICO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new OperationalOverrunsSinteticsPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            OperationalOverrunsSinteticsFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/SCONFINAMENTI_OPERATIVI__SINTETICO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new OperationalOverrunsSinteticsExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            OperationalOverrunsSinteticsFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// SCONFINAMENTI OPERATIVI - ANALITICO
        /// </summary>
        private async void BtnOperationalOverrunsAnalitics_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/SCONFINAMENTI_OPERATIVI__ANALITICO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new OperationalOverrunsAnaliticsPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            OperationalOverrunsAnaliticsFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/SCONFINAMENTI_OPERATIVI__ANALITICO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new OperationalOverrunsAnaliticsExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            OperationalOverrunsAnaliticsFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// SEGNALAZIONE INADEMPIENZE PROBABILI PER SOGGETTO
        /// </summary>
        private async void BtnReportingUnlikelyToPayBySubject_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/SEGNALAZIONE_INADEMPIENZE_PROBABILI_PER_SOGGETTO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new ReportingUnlikelyToPayBySubjectPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            ReportingUnlikelyToPayBySubjectFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/SEGNALAZIONE_INADEMPIENZE_PROBABILI_PER_SOGGETTO_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new ReportingUnlikelyToPayBySubjectExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            ReportingUnlikelyToPayBySubjectFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// SOFFERENZE SEGNALATE CON ALTRI CUBI
        /// </summary>
        private async void BtnSufferingsReportedWithOtherPhenomena_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/SOFFERENZE_SEGNALATE_CON_ALTRI_CUBI_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new SufferingsReportedWithOtherPhenomenaPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            SufferingsReportedWithOtherPhenomenaFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);

            // Init the Excel report
            filePath = $"{dataFolder}/SOFFERENZE_SEGNALATE_CON_ALTRI_CUBI_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new SufferingsReportedWithOtherPhenomenaExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            SufferingsReportedWithOtherPhenomenaFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            await ReportGeneratorAsync(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        /// <summary>
        /// TIPO ATTIVITA’ INCOMPATIBILE CON RISCHI A SCADENZA
        /// </summary>
        private async void BtnTypeOfActivityIncompatibleWithRisksAtMaturity_Click(object sender, EventArgs e)
        {
            string filePath = $"{dataFolder}/TIPO_ATTIVITA_INCOMPATIBILE_CON_RISCHI_A_SCADENZA_{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builder = new TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            TypeOfActivityIncompatibleWithRisksAtMaturityFakeData.FillBuilderByData(builder);
            var report = builder.Build();

            await ReportGeneratorAsync(report, (Button)sender, filePath, initPdfProgress);
        }

        private void BtnTypeOfActivityIncompatibleWithRisksAtMaturityExcel_Click(object sender, EventArgs e)
        {
            string filePath = $"{dataFolder}/TIPO_ATTIVITA_INCOMPATIBILE_CON_RISCHI_A_SCADENZA_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initPdfProgress = 0;

            var builder = new TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder(progress: GetReportProgress(initPdfProgress));
            TypeOfActivityIncompatibleWithRisksAtMaturityFakeData.FillBuilderByData(builder);
            var report = builder.Build();

            ReportGenerator(report, (Button)sender, filePath, initPdfProgress);
        }

        private async void BtnMonthlyReport_Click(object sender, EventArgs e)
        {
            // Init the Pdf report
            string filePath = $"{dataFolder}/REPORT_ANALITICO_PER_CONTROPARTE_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.pdf";
            int initPdfProgress = 0;

            var builderPdf = new MonthlyReportPdfReportBuilder(progress: GetReportProgress(initPdfProgress));
            MonthlyReportFakeData.FillBuilderByData(builderPdf);
            var reportPdf = builderPdf.Build();

            // Generate the report
            await ReportGeneratorAsync(reportPdf, (Button)sender, filePath, initPdfProgress);
        }

        private void BtnMonthlyReportExcel_Click(object sender, EventArgs e)
        {
            // Init the Excel report
            string filePath = $"{dataFolder}/REPORT_ANALITICO_PER_CONTROPARTE_{DateTime.Now:yyyy-MM-dd__hh_mm_ss}.xlsx";
            int initExcelProgress = 0;

            var builderExcel = new MonthlyReportExcelReportBuilder(progress: GetReportProgress(initExcelProgress));
            MonthlyReportFakeData.FillBuilderByData(builderExcel);
            var reportExcel = builderExcel.Build();

            // Generate the report
            ReportGenerator(reportExcel, (Button)sender, filePath, initExcelProgress);
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            PdfFont boldFont = PdfFontFactory.CreateFont(Resource.CALIBRIB, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            PdfFont regularFont = PdfFontFactory.CreateFont(Resource.CALIBRI, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            string filePath = $"{dataFolder}/test-{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.pdf";

            IReportProgress progress = new ReportProgress(value => { /* Some code for the execution progress value processing */ });

            // Creating a report builder instance
            var builder = new PdfReportBuilder<PdfReport>("Chart Samples", progress, PageOrientation.Portrait);


            Random rand = new Random();

            #region Bar
            var bar = new Bar(new double[] { 1, 23, 71, 44, 67 }, "Bar Label");
            var barChart = BarChart.Create(bar, "Bar chart test", new BarOptions { PlotDimensions = new Size(700, 250) });

            builder.AddChart(new PdfReportChart(barChart));

            // BarChart with size customising 
            builder.AddChart(new PdfReportChart(barChart, 400f));
            #endregion

            #region BarSeries
            var barSeriesChart = BarSeriesChart.Create(
                new List<BarSerie> {
                new BarSerie(1.62, "Item 1"),
                new BarSerie(3.67, lineWidth: 4),
                new BarSerie(1.32, "Item 3", 3.2),
                new BarSerie(4.13, "Item 4", position: 1.9),
                new BarSerie(4.79, $"Item with Value({4.79})"),
                },
                "Bar Series chart test",
                new BarSeriesOptions
                {
                    PlotDimensions = new Size(700, 250)
                }
            );

            builder.AddChart(new PdfReportChart(barSeriesChart));
            #endregion

            #region Base64Image
            var base64Image = Base64Image.Create(base64Icons[0]);

            builder.AddChart(new PdfReportChart(base64Image, 50f));
            #endregion

            #region HeatmapChart
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder randomString = new StringBuilder();

            string GetItemText()
            {
                randomString.Clear();
                for (int i = 0; i < rand.Next(3, 5); i++)
                {
                    randomString.Append(chars[rand.Next(chars.Length)]);
                }

                return randomString.ToString();
            }

            byte[][] icons = new byte[11][];

            for (int k = 0; k < base64Icons.Length; k++)
            {
                var item = Convert.FromBase64String(base64Icons[k]);
                icons[k] = item;
            }


            var list = Enumerable.Range(1, 50)
                                .Select(_ =>
                                    new Heatmap(Math.Round(rand.NextDouble() * .5, 2),
                                    GetItemText(),
                                    icons[rand.Next(0, 9)]))
                                .ToList();

            var opts = new HeatmapOptions
            {
                ColCount = 9,
                UsePercentageValue = true
            };

            HeatmapChart heatmapChart = HeatmapChart.Create(list, "Heatmap chart test", opts);

            // HeatmapChart with size customising 
            builder.AddChart(new PdfReportChart(heatmapChart, 700f));

            // HeatmapChart
            builder.AddChart(new PdfReportChart(heatmapChart));
            #endregion

            #region Pie
            var pieChart = PieChart.Create(
                // new List<Pie> {
                //    new Pie(778, "Item 1"),
                //    new Pie(283, "Item 2"),
                //    new Pie(184, "Item 3"),
                //    new Pie(76, "Item 4"),
                //    new Pie(43, "Item 5"),
                //},
                new List<Pie> {

                    new Pie(10.62, "Item 1"),
                    new Pie(34.67, "Item 2"),
                    new Pie(15.32, "Item 3"),
                    new Pie(40.13, "Item 4"),
                    new Pie(4.79, "Item 5"),
                },
                "Pie chart test",
                new PieOptions
                {
                    DonutSize = 0.5,
                    SizeScale = 0.8,
                    ShowValues = true,
                }
            );

            builder.AddChart(new PdfReportChart(pieChart));
            #endregion

            #region Scatter
            var scatterAxis = new ScatterAxis(new double[] { 1, 23, 71, 89, 100 }, "Scatter Axis Label");
            var scatterChart = ScatterChart.Create(
                scatterAxis,
                new List<ScatterAxis>
                {
                    new ScatterAxis(new double[] { 1, 23, 56, 37, 67 }, "Item 1"),
                    new ScatterAxis(new double[] { 17, 28, 71, 44, 47 }, "Item 2"),
                    new ScatterAxis(new double[] { 11, 2, 45, 50, 67 }, "Item 3"),
                    new ScatterAxis(new double[] { 21, 13, 65, 44, 72 }, "Item 4")
                },
                "Scatter chart test",
                new ScatterOptions { PlotDimensions = new Size(700, 250) });

            builder.AddChart(new PdfReportChart(scatterChart));

            #endregion

            #region Signal
            var signalChart = SignalChart.Create(
                new List<Signal>
                {
                    new Signal(new double[] { 1, 23, 56, 37, 67 }, "Item 1"),
                    new Signal(new double[] { 17, 28, 71, 44, 47 }, "Item 2"),
                    new Signal(new double[] { 11, 2, 45, 50, 67 }, "Item 3"),
                    new Signal(new double[] { 21, 13, 65, 44, 72 }, "Item 4")
                },
                "Signal chart test",
                new SignalOptions { PlotDimensions = new Size(700, 250) });

            builder.AddChart(new PdfReportChart(signalChart));

            #endregion

            try
            {
                var report = builder.Build(); // Building report
                report.ToFile($"{filePath}"); // Saving the report into a file

                MessageBox.Show("Pdf generation has been completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = $"{dataFolder}/test-{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.xlsx";

            IReportProgress progress = new ReportProgress(value => { /* Some code for the execution progress value processing */ });

            // Creating a report builder instance
            var builder = new ExcelReportBuilder<ExcelReport>("Report title", progress, PageOrientation.Portrait);

            // Creating report elements
            var companyLine = new ExcelReportCompanyLine("Some CompanyName", new DateTime(2018, 09, 30));
            var filterSection = new ExcelReportFilterSection("Filter Label", "Filter values ...");
            var mainNestedSection = new ExcelMainNestedSection(11f, 1)
            {
                Title = "Dati Controparte",
                Content = new Dictionary<string, string> { { "Codice Controparte", "CTP123456789012 – Società XY" }, { "NDG", "0000000309169701" } }
            };

            // Adding the elements into the builder 
            builder.AddCompanyLine(companyLine);
            builder.AddFilterSection(filterSection);

            //// Adding a Chart
            //builder.AddChart(new ExcelReportChart(ChartType.Scatter));

            //builder.AddMainNestedSection(mainNestedSection);

            //// Adding a Chart
            //builder.AddChart(new ExcelReportChart(ChartType.Pie));

            //// Adding a Chart
            //builder.AddChart(new ExcelReportChart(ChartType.Bar));

            //// Adding a Base64Image
            //builder.AddChart(new ExcelReportChart(ChartType.Base64Image, 100f));

            try
            {
                var report = builder.Build(); // Building report
                report.ToFile($"{filePath}"); // Saving the report into a file

                MessageBox.Show("Excel generation has been completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHeatmap_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            var list = Enumerable.Range(1, 50)
                                .Select(_ => new Heatmap(Math.Round(rand.NextDouble() * 5, 4), $"Item - {_}", new byte[1]))
                                .ToList();

            var opts = new HeatmapOptions
            {
                ColCount = (int)(nudColCount.Value),
                UsePercentageValue = chbxUsePecentageValue.Checked
            };

            HeatmapChart heatmapChart = HeatmapChart.Create(list, "Heatmap Test", opts);

            byte[] imageBytes = heatmapChart.GetImageAsByteArray();

            string path = "./img-output";

            string filePath = Path.Combine(path, $"heatmap-{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.png");
            if (!Directory.Exists("./img-output/"))
            {
                Directory.CreateDirectory(path);
            }

            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                memoryStream.WriteTo(fileStream);


            MessageBox.Show($"Image saved as {filePath}");
        }


        string[] base64Icons =
        {
            "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII=",
            "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAACOElEQVR4nO2az0sWQRzGP5V1yYNa4asEnfXWQTwU/Ti/Hb136y8ou0Z66Oa1PAj6B0iXjga1QegpsAg0KBS6RaBBRunIxDMwLLu+747r7r4yDwwz88x3Z7/Pzo+dfb8vREREhOIesAL8AkzNaVe+tIuKmG2A8yYnzRQZCXvBHvAQGKF+jACP5JPpdmRey9iKaBqm5ZudZh2xK+MWzUNLvu10Y+zmYhVYBd6nuETp2P5VKSTrXqbD/aOQspEU3G6T0yLkbah/cY0EIC52Ap9SmYiLXYiL/SRxahZ7kvHCS3rxrBUCE4U0DCaOSApngWHlPTkiLWAJ+K12my92+Uk8X+ImYo4jZAD4Iv4P8F25rW+qPQ9TqWN5rUJmxb0BhsQN6VvBqD0L14CfTRLySdyNFH9T/HpGP33AO7UvN0XIP3HWuUHgOnBF9Wc5IzKja7Y0eo0QYrwne6DyPvAKuJrRxx2Jt+lWUQeqEOJ2q4/e7vUVuOzZXgK21fYkxIEqhKzqHYLyNfFznu1L70eDcyEOVCFkIsVPiv+m+hnP1q6bB15yvC3frkvIX3H9Kb5f/F6GkKPSQl1CNsXZRezjrvgNT8iLnOT6teX7dQl5Ku4zMCZuXHWj9tIcOEkhdgp98Np+eGXLXwzst5ZD44CmhYsp2vx5h3NWkANl9bMjw7yQ2wW1nad6jBYJ9KzI2MbsmobHRUJvbW9LndZTqBujEuE+HboOU7sDX0+Hpx3aGkIXHO3JPwxERETwH4d7wAkufrQSbwAAAABJRU5ErkJggg==",
            "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII=",
            "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAAAsTAAALEwEAmpwYAAACoElEQVR4nO1ZzW4TMRC2UuDAkZ8DEIgdKg5cEQhOvAQBBA9SlEsQgrbXqp1Jo75BKpoDP5feCn0AKngAoAegLWpmSLTlEKPZ8teSKtnY62xRPmmklVaJv88ej8ffKjXCCCO4o27HCvPNGwaprJGfGuB3BvmrBvouET8jvY3fIZUL2LyuKjanho2L0D5vkKcN0LpBtklCI33UwFPjM618cOLjM3TaINc00E5S4v8Igfg/qpdqdCoI+SLyPQO85Uq8S2wWkO+mRvxKzR7VwAspELd7AnhexvJK/kzNHjdAL1Mnj7/T6oWM6YW8zEZI8ubPJl++XLfHnAUESRs8MKpO5E2V7w+RvN1NJ74zEPmzs82TGnhj2AIM8NZAJVbqvMvAj15H9nOrE8fDV5HjfmBIRF5OR5dD6najbf9Gx1pbarQd0oh2irX2hSSzP+0yY09WI7sfj1cdVwF4qj/2FZuTPsVlsFKXFbi11HbcC7QuTWNP/rtdpfvmk7z/9K0TR2XFbfbNr6huX+sjfajsZbAUooj0oLcA4KVhEzUHhEZe7ClALh7ZFUBr/ayAl1Z5P7wIAN7ovQIeLimpCUCK/n8B5tCnEPrZxCmtwFqwMpqOAF4MdpCltAITPQWI6ZRVAQZaV/tt5j5kTYAGet+3myeta+YEIE+qUBca3wI0UpSvts6pJBBHwEveeggNNKuSIr+wfSITl3rkzYF9U/EqMyCgpFwgXuUQU2dOOaNuxwxyIzh55Oc3K/aIN3NXDNdwM8/PvJm7e0zeAJVJA815m/luEK8yjeqkkb84b9hEvikyyAHjTpwiqfNStlVoyOkobccgvZP8RiNPJj5hU0HF5sR0Et9Gena5eMjNTtqRuCWRZ6Q3P99NxF1lFj6zjjCCOvz4ARXq0PBS824rAAAAAElFTkSuQmCC",
            "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAAAsTAAALEwEAmpwYAAADVUlEQVR4nO3X20sUYRgG8KUIb/oDOtx0UdCdWDfduMN+Y66bq+6UE2W0m9BBMjonEXnCAk20jdTKlbUuio4X5sy4m1rRAbQspESi6GKjg5pRWGkJ+cQ34majbrM0szPCvvBc7DfvwPNjZ1jWYolPfIwZ76Of0DDlsxZQ3z1qDEIrwIsvv+AzAqEVIPR9zBiEloCQEQitAaFYI/QAhGKJ0AsQihVCT0AoFgi9ASEFwrQAr8rEAcopE15ha9FpZG3MRarDCUKIHPf+MnN/AzzPz7XZbBVsyupw6clh2RQUNLSi6uGQ+QDFxcVzCCFXpiuujD09C/vrms0FIIQcUVN+cvaeumEOQHJy8kJCyEi0gNV2B8rb3hsPsNlsJZGKbnSxqCtIRU/DGrRXO8BOupZfccEUgE5l6c3rUlB/2I4XjekYkzKAlj9pLneAZcf31m/bZzyAEPKZltnEsag5ZMfzhqmllblxLE0GOLNzTAEATaTC04XeQ38nZjWAEGI8oHzHqvbJgBr3UtR6lk0prDyn9xwvzDMegEvLhs4ftYeL1XqWom4agPKc3vPzygpTAEohpY9F+whBcqIjeNJ4AB1Izp5oAR9b8/+7vFdDQEW0gMd3/SYC3MpcBCnjm9ryowEevo5+8wDooCWTh5Sh4l3IROB+qyblvVr/I0PAmQPREeHFdaDlfrtm5b16/KWELwG4vBy4yUDG0DRZx898CZqW9+oCOGtBpJgSQPjmxYxLOM64xJdPOnvx6VkTcNsDXFwyntse+YxeW7v7Iface42qB8PGA1Zu75rHcGIh4xKHGU4ETXHlU0xM/5kz6PN6w5+LTjyRd2jS3G040BgyDsDwd+YznCBNFJqIbZ2Ed33f5fLPExPlUETfwDBItvTXLo27pBvVHT9iC+D5q3OtnHhHWWYitY29GPD7wwCKqfH3TrtLs7nwaWwBDH1sZihD48gJYujDYBjwtW8QazYFZ9ynoe9FTAAMLyywuoSRSGVommsDYUBTXeTyNOyGICrvfYsBgBMO/qsMzXbPNfQkJcnJy73+z30aKyfsjLpQtMNwQlBNGZquLfnoyt2lalcGuETBovdYXeJbtYVK8y7KUbvPcMIb/QGc8EN9oehC3y3dAfGJj0XV/AahkTEmX+fLiAAAAABJRU5ErkJggg==",
            "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAIUUlEQVR4nO2ae2xT9x3FL+paKg3oulbb/ttCAzZO7MRxnm4ggbwfBAJ1CHk/7ZCBIDAY3VCzMsEo4RURgsJgUBAhmMSJnQeBCNoloUB5jFG1QxDWNvQRHkGbClp5bJ/pZ4XKirIFO9dJqHKkI13d+/uec77X9/r+/LuWpDGMYQxjGMMY3IpfFKGYXIzJo5idHsWc8FhE1+Ri7noU81DQvr2ILnFs8iIq7WN/yVTpWcbURWg8i9g8pYgbU4rAJZro9jSxSWhJzwYYpzSSoDRxSmmCJ1SY6FEYOaQwsVRRRIyygKnqRbysMvCCoNgW++zHjCxTmKgRNY4aSiOd00zECw9pNEJdgM6rkLNeRuhjr5eR7SoTga4pMm6akSCvQir6tJ7onlYa8ZNGC36ezYuaArapC3msKQR1IV+qCynRZPBDuTyElqaA5eoCvurzEF5bPZcwXhpJaPPx1OZzUZsPvnk81OazUc7G+0NVzARtPmV9Xvjmc0Fn5DVpJBCQR4gul17/PNDlcdU/D+1weety8PPP5Vqf9x1dLkHScCIwj+igXO4F5UJQLg2B6Uwa1gAiQzqTAnOx9WW4F5RD1LAYB2cRGJLNPX02hGSzLyyMH0gjBIOB5/TZVIks+izuh+Sgd6thWAae0zPpnZ4FoZn8cXQ8khgXmsVue6YsbofmMtktNmHZvBiewcXwTAjLoEGcfWmUQFyF4ZnYRLbwDM7FuuPpEJHGtoh0mJXO1dgRuOcHQ6SBlyLS6RIZI9LZJKt4dBq6qDQeRy/kYUSqPN/2i09zaslpEFz8AR1y5YxO45FgVBq+st1jcamcjVsIcalslElUKvkAHCmXbtxCtoissal0yiIYn0pCYiokLODLKBknOatOgSPl0k1KYmJiKl/bMy8kZuiCBk7NSYEkAyWSjPhtJzhSTu0kAyv7MrcPSSg5BZ95Bkg20Jsh8xT37XZwpJzaBgMT5r3BXXv2FLxdF5rPZsMbYJjPdklmrP8zOFJufcN8Kvuyv+OyyIJ53EidBynJBMiaTpKksvfBkXLrpyQTLLIvSOYzlwTSk1CkJ0PaXG66Y8a37T1wpNz6InN6Mj2ih4zZeDpdnjmXoqy5kDWHQ/KHk6QdJ8GR7vDInMNh0UP2XIxOF+ckUZmbBDlJLHVHuF0nwJHu8MhNosTew2wqnC4uSORkwWwoSJDhWToA9raBI93hkZ9AvOghP5E2p4uNCXxqSgRjrHtWXA4eB0e6w8OUwBR7D4lcd7q4OIHe4gTIj+bH7ghnPgaOdIeHMZFXRQ/F8dx2unhxLA+WxEGpgRfkDlbfir6+FfpR9sWMJbGMFz0sjuNbp4uXxvBgWay8J+D8eZ5vPMrvmo7yuOko9OO/G1sob2mR77e8OAGih2UxLpyAFTH0roiBEplugZYWNK3N/OVYM/xfNvHx8Wb85fBcnsiroocV0S7cAqsi+fuqKFgd7cIkot+nfrKJ0hONPDzZBI480ch/BAfYL8aWitqheK+MZIroYVUUXU4Xr47k5JuR8JtI1x+D7Q2o2m2c62iE/mxv5PN2GxEdNkLbbXQNNKajkb92Nrm+sLE6knjRw5uRLjwG18yick0ErJnl/EQIGHfGivGMlftnbTAAzR1NvPxk/JkWJp21UTXgWCv/Omvl12az82uQayIosfcQ4cJE6K1wTKUzoXQmNc7UXbLicaGB9y9aoT8vNNBz0cqc/1V7wUrshQa+GKj2YgOnL1mce2VeGo5Z9PDWTAolZ7F2Joq14bA2jJs85Y+hyxaKPrJw76N66M/L9Rz6xMIrg2mIMZct1AykIbSFx9NkEZnfDueW6OH3M12czK0Lo3tdGKwPH/zV0yd1eF+xQH/+rY7bVyykOOt9pZYFV+q4M5Dmx0fwGqx+/QxCRPZ1M1z8OSywYTqbNsyAP8wYfEHkfBXPd9Vy6XodPGFXLdZPzfxMchGitquWxn6al4TXYLUbZrBTZN8wnQ2u+ksbQ9GUhcLGUHrLogZfErtRh0/3Eb7truUfn9eSI8mEbjO53bX8U2gLj8HGv6NnYlkod0X2spAhLIkJbNbTueV12KRnufQU+OIQU7+u4yeSzOip5qc3apnyNGO36FklMm9+fYiLogLb9MSX66E8hK8qwpggjXKUBzKpXE+PPbOe6CELIjFuezCnK4KhIpgyaZSjIoitIuv2YHneNtlRFYxfZSCPK4N4KLalUYod/gTsDORRHwf9rnAKVQFs3RUIVQFcE5eZNMqw14cfVQVyXWTcFeCGK7Xck/F7/Dm/xx/26LCVjuAfI/pDZNnjT5PItlvHh2aV/GsYduzT8dpeP+7s08E+P3Y/7QzRnRAZ9ur4kz2Tjlv7A/Fwq+EBXwL2+/HNAT844Me7743glWA28Nx+LbvsWbTcf1dLyLAYV/sSVa3lXrUWqn2xmXW8JA0zhOdBLU0iw0Et3xz0JXJYA1RrCDqs4fZhHzjsQ1e1Vp5VnKdBjS8BNT5cF941Gm6ZfVz9N+oQUatmcq2Gc7UaOKLmUa2GLQ0KJkpuwoFAJh3RsLXPS3h+aNa4+Z4fDC2ejLeo2WTx5lG9Gixqeuq9+JVZJd+sUWhZ1Kys9+am3cObRxY1ZW77tncFTSp8bSo6bV4gaFVx16pih1VJsCtPC1Fj8ybEpqJSaH2n60VHo5fMkxw50ehFTLOK9mYVPGGTilvNKg43qShpnkbcUQUKi5JXxCcoKLbFPnGsWcXyZhXmvprvNIRmi5cMc/vhwjEl3q1KNrYq+ax1GrhEUavkHaElPctoVeF5XEnhcQU7jitoa1NwrU1Bb5uCB30U21fFMfsYJQWiZqRzj2EMYxjDGKTvOf4L3jA+gTF4fnYAAAAASUVORK5CYII=",
            "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAK10lEQVR4nO0ba1MTWTafZvbD7v6EfU/Nr9ja2X+wu7Wfd2pnP419A85sqSgIvgjEdPPwlRDS9yaBQAgEUBQFX6uCaBRUXFR8gDhuMYoiD0l0apmzde5Nh3QnmJA0GLe8Vacq1X373nPOPe97YrF8HB/Huo2KL9lPHBL7QiasRCaMyoReViQ6oRA6KxP2AwL+xmf4TsxhJYpV/UO9tf5Ty4c46q1NP5et9CtZYqdliUYVwiAXiH/br0jq33FNS6GP2mLP5/wEk4iusTLw7fHDaRaA4Z4gTAyGYOZ2B8w/CEN0qpMD/n5+u4O/wzk4F79RrAZmSEzFPSyFNmq+Vn8jE9qmSGxZIzq43w83TwZh/n4Y3n7XmRPMjXfAzd4gBPc38TU5MyS2LBPa6iD+X79vui2oowqhZQqhMUSurtgLfWqAn3CuRK8GKCF9ngDfQ0gFjckSK31vdsJeRD9TJDrCkbEyOOFqhtm75hNuhNk7YTjuDCTUQyZsWNnU+LsNJd5hVf+qELaICKjlPpi43P5OpBcehmH8fBtcam2FcF0z0AofOLd5oX4zg9oixn97d/mh+0AzLE1kpzJoLzw7/ZqNWHAQ+pcNIV6WmKTpevfBZk5cOgTRuI2daYOuA02c0Kwsv9ULp69GYPzRJZi528VFfqy/DWJT6fdYfBSGnsOBuCTQH2WJfruuxCsSK9dE/mpnMC1SsalOuHEiCO4dvhXCinzgqOwFe+M1qA4/BNuZZ1A5sACVkSjYhl6D7V+zUNU3DbazM1B57Q0HuX0s8X3DDh83qKtJw1BHa5LHoGXrQrwsMStuUGv1wq2TbenF8nJ7sliCo7wbqgNjYBucTxCWNURiUN0yBo6yTr7W4RL/O1XiVm+Q4yZcJpPM13mJLSOX0xEffRyGc/6WhKuSy7ugqnty7USnZcQbqDr+BGz90xAcnYJnk6dgciiU1lYgE7gkIK6S98/mWXvCFpCwdGKPQUxLdfzUi/1g997gom0K8WnA0XWf79W0zw9z4+H06iAkcH6/1fvb/P28JFwdGhvjZq/uhcG7WxAvl4TA1je9boRrYLs0B3JpmO+plvvhxb9TXe/Rw80JF1lREfokZwYoPMgRrg4trvHk8TnX9YpjYLuUg57nCgML4Nh7QhjI7T4eGxi9g2aLZEJ35ES8Y5PnlwphS6hTE4N6P49uKSQ3JYivHFrcOOI1GHoNjr3HOQ4Bm5+7XaNBFp6BxnIKm2WM7YmI8IwidtYX973b2zf25I0wuJBQB4wZjHgeP5JQhcCaiK8t9nyOlrR+szclvH0SaRfWvtifl84Hx+ZgduYxzM5MQetY7kysOvsclG+ENN47H9Lh+vJOB88dZEL/i8Z8DafPKOeqqufqm6lO8O8Vem+nI3mdHhL+w6sJDi9npvJaq7rpNsfJU+bjLjkZ51ONcWmVqCf7YgZhS3jKxqwOc3W+WFkXVF6NFgwDMFaQdx3juEUMrvr5qLAFskSX7Fs8P8vIAIWwf+BCbY4mvU496eRJDL7DcDZf/UWxRyYg8S15qEBCFXqecNyObE2VgqBdeIQaiX6ZWfwldhonG2Pv+xdCK6cfMcGArQM44lKACVQy7pibxF3iyQziX/8pLz1ZGa/GJC+CqSrX/abR907oalAdvMdxbK/RS+/cvbBQA8KW3llAUazeP+ICvr3+lAzv4Ldenq7aLuYvrpPTTxP6r8HE9NP8mTC0yL1TTVHqAWoRaw2hv19d/AkrwUlYlDQWH3jQs7vHlJN6+v1368MAVIOqU8IlntOrQT9t0WKCratLgEQZTho5rtf/gaBIMPa7rpgqshrxpqoBHeG4Xgi06D3YsWBmdyjjpQVhMHFZH1DwGhxa//b7Bc8ATMO5F5P1duDRgJBihbBLq0sAYY9x0osxvf5grI3Pq079p+AZYDv3QgRFO/V2DGMaUVanE6tLgMRe4iTM9JI/xowLn1denCt4BmCSJOIBr94TjMcZQNjMu2zAW5xkzKywaovPbZcXE1bcaLSyfbYWBuS6D3qruiKWUrWKl8veZGbA4zDEHvghOuaG6G0nHNnCdAxIh3i2z/J9n80zngZbGbwedUJ0rEHQMtmRmQFyXAVe3VQ54Rq4tlHBgLgKFDIDsMqMuB76J9PRMDuiZqECRBjBZ0Mu3cfNe8XHWLrGTSbSiGG2z9bCgFz20YwgLdcf4veDriyMIBFu8GF/g+7jY3WeD84Nhqo9Ohrun2rIwg1KIhC6HmrUfXzB2ygCoYarH0wgdMatZ8C1oDtzIKRIdBtO6nfpGYASYWYovJ4M0ELhuyeEAdegzymkWCFsy6oMcEjsC5zk3aXXn9e3nHDgG3F3h2XpQmUAeqmaYj/UYjY7soI/AttJMydD9TwdVmPoQmYj+gW6FNX0dNhsBlQH73IcWyv1B/gy4souHcbBe3IwIerQi9D4SaFDjrKwaQURsxng2C0KIqPdeiM+HBK4KxLttWQaspV+hZODNr0RiY46gZbRvEti6WoBZqTEVcemxEVJCRUBUBLuLfu0GMD7t2yLolEsik4PutJykktBjkXRdLWAvBkQiSWKooNNegM+PSDEHy95Dm069NOMDEgui588rJeCpVEn+HYJKbCrw6aJbr5gb46XxUspLNzUH1rvYc/ayuKJixFCl+uKGcxc0S84ebYhcTFidnqcC2CzhbJZtNRhsJOM6/MhF0+KkJY19xEpEgvyW+F6gy247YQz7saVG2ETaoQ5Ez84z9URcek9mIrn0XgEq0i02bLWUVfk+wW/HCWpnEVVCFULwyKXd/M7uvd5Odq8R4WFG87U4E24vmhtMfuVJZchS6xU0625Yb0qvLruBLVU2ANHxVFTA6TMJ78Ajj09okeglKbELHPDTmjcIXBTJLrdkuuox8AI+/AIg6O1+uBCBBhO8FbQhDpshE2oOv0M5B0it3eXUJ7lGfHqrom7PYlez6tBAgcaD61FxuhiBLddEND8bLGfJyP53humhUgM7L5bwuARBi2VFGavpRI/4Heb1yKjDWxCVCT6I+rUDUOEqOUK51UPv5AQcUKnCJYi5qW42HGmdZ/1HvLA4k09DggYvcZ9/rJM2J8spjdIEmyTS88EzfBo0SJnxM4ufnWdS5scflPdcocbWW09ulNNMcjJxCNuXB03qV9b1mMo8Z4h5HI6ddA8BCLj3r7CCMwi0WLb3aJREvWYd5ZEYhzwN/pzfGdvvA6OfSd4c6X2PYa3QwE3l7R0e3Kx14iXWOm6EK8NhdBNMqoDYdCpeFK8gwaLt5xwp6eBG6SsW2WTAE8zZFf5GsjUdHvg3glfj2K/XidvHNiEqBlGdJEP+tKrRDKi470NcNHXCB0OFbwVKi+01hdTzhw8YQyx2/ercIE1cjGfN+TzRsA5CVdH2LzpOp9poIXVXCSKX0+dB55feTfSZgCGt0mnzl2dadZ+rQN9LPbhYbSFyGDcjcmHMYs0AzCrw7VFbC8iPAxy8vbzZgzsKcRWNJ50xCUCqzLD7W5ejcmVaPwWU3Cez690gy9jbI+huqXQhr2IfoZpp5ZDaMxg5RT6XR643ubmuouniaE0GkqEV9dc/Bm+wzlYwOQ1vKQ/TeGafO2N/ndILgO7sbAhCXty8vnbnCLRJSxjYSUn62JGoY2KitAnWInFzgw8QVmiAzJhj/g1HN5FSvQt/sZneGnB5xC2Fb8pCP3+OD6Oj8Py/z7+Bz4nX90q+Hq0AAAAAElFTkSuQmCC",
            "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAAAsTAAALEwEAmpwYAAACMUlEQVR4nO3Rv0sbYRzH8e/cYrFOblJXKf4B9xw6ZNP8CQ4uDoGuhYKLULA2FDsoiGStoIiDdzr0TqJ4UtFKuzVJEUuxFHuaQT1x+8pFHHxifO557vtcrnBf+EAgv17vBCC77LLTcqxyYpgV3zKrvmfU/Dz8h3Dklu4Q1hqe7hAWHZ6uEKYOb2+IUTlhRPBkQzTCUWtIgnAkDWkjHGOFUMH7v//FrvIhPnN/Yt/BH/0hBjEcrB/3RhnCqqe5FgH/HGq4hhDvwQDVkKhwghDvUbhsiCpcIcSTgotCqOARQrxYcP6KX98W+rd2A2o4cHu5vX89fTAxTga/coAFLliBCxhuvjyCL9w9cni38w2nyq8a3xHu0gEvcCFPBudHFdLNwflJh4jgVCEieCAbIgtXDZGFBw+EXDiQaxXgqH7w+TLgWRFwZcbEvrWNJnjP+g6W5vJYfwd4sQT0AaohIbw+Bei/ub+l2UHsXdtubHF2sOn58D3ny4Rw2ZC7X5yHya4u+Eek4aIQKrgo5DIunL9PX4YLh6XnV9Rwn9tRqfN6YXfoNRncqKwys2Y7Zs3GcJMbBfz1oYMcfvz+CX78PNr4jttZHqvaOTI4P6qQ4yY4P8kQEZwqRAy35UJk4aoh8nA7WkjcAFFIfLidzD9xt6I7hr+LTxsLH2uH6wpJHJ6eECsevH0hFi08uRBLL1xfiJUsnC6kzXD1kJTBo4ekHM4fq60OmFV7M1z4uOkF2WWXHVDcDTimfbqjSa1IAAAAAElFTkSuQmCC",
            "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAAAsTAAALEwEAmpwYAAAMn0lEQVR4nNWZeVDTh7bH0/uefa/tzKud2weExa1qF0uVVcAFSTQgFhQ1WvYlyS8SZRMES4GgKEEWWVoUFMlGVkJCIKwCsoh1AS1SK25YO2iv+t683t6O4kK+bxKq1xYCXqte75k5/2WS7/f8zvnkmwmJ9JLKodRhCulfrRZLVr1DEdC2UAW0XirfU08p9x5w2+E+Mit31mLSq1pcLvdPVKHnciqfJqUKPO9SBZ5w/8odjklucExwgcNWF717hvvDwIbQU+yuyJCgpvi3SK9CUQSrrCgCWhJF4HnFKFrgiYVcdzjGu8Eh3gVOSS76TxVrEFAfClZnJIhOjrFZHZF/JToiRayuyOUkkF57qaK9Cr3+g8L3olP4nrVUvucDo/Aidzhu/3Xa8QvhXkgdoWv9EdFGPBZtqlmdnB9YHZwsdjt7zgsVTjnk9RGV75lFEdBuGUWX0eDMXQL7eFejcKckN/1q1VoEN4VPKpow1R2cHlZ7ZAxxhHj3uYheXrr8baqARlAEnkcNog29eK87nLa5wjHexSh+Rb7nCL0mAMwj7GcX3vn7pxI5THRE1hJdkXSih/jHSeYhoDlQBJ6lFL7nL6PTXgbn1NEVsY93wcLk5zDtzqc008G5GVATGj35tMuWWxoOksqnXX48bd6vB2kQvs0VXmWf6kenvemFCydGjx3rVZ/BZc+iH8YVTVfSX6eUr/ChCmjKxwe5bxmctrsaKWKYtmua64ifmo7Qw4yXIpro5BjX0VeyFk67Xe657/b42S/PL2SMePcvl2/yKKPdfoQ/1wx3OBh2O8EFzgkuWCXx1fvXB4PV/nf8vegOb2FhZbnPQ7sdzve996z6pUSZj7Y2rXrc6dvHON+zj3LWO8YthMPW0XbluY+s02xAeCvzpYkmOjkIrAuFRzEN9ulOCCoIHFHo+DhypObX1u4Y30C0M951Nsea3X7DDrEL9UsLqCMvc02IDg42qAOwOG8ZHHe4IGofB3XNiieEG8VD1VhZYtLA1AXv4ty33Th2shGpsqSH3mLfYW/p6uHAhtAXd5jtkfCT07Ew0w2Ldi1FyqFENLeqfyP6yS6S5WD2Ns4d8rbuPofSniljDFy8cBLfD54xdl9fO4p02Xpfsd8db6nPXUMEeF7CGW1s+Ij94JCxEJRMKvIkmYb9Hld4a6sGdXVyKCoPYVtxDD5JpcB8kwo2vKGbVtzz0b8xcPniqccGHvW5/qMoayjS+4n99V4Vq+FfF/ybLPOPdEhjBFaUroRdujP8ctdCVF0yZk0edUuLGjqdFEJ5MSILWHDmLsL7CXRYMrNglX4J1infwIKjHUWq3WbHm5bU6Th9uu3B7w086oHvvoa89RACZUHwEq/CRl0gmB1PZ8RfGwz3IqrxMEMLg6FpEJsU3tysQk2NBCUVeQjfGwy7NDfM2RoIi/BskBM6YJV0CmS26t4HWxQ/eyTLR5FqF+O01G6z4y23mCVDGfzsG+f6u8c1YeiLAyegaReDoYyAp8gbhqDGbGeP+8VDr/KHa/YSuGS4IbE0Ds2tVSb3u7GxEtXVYuSW78Ia3hrYpi7DjCgCZmHFsErugeW27odktvK+81bZL19WKMYi1YFwmGIf5bTBdstqvVeeNw40FuFsX6dJI4Z103XJQChZoIm8sa56IyJaCYS1sOAj8oNjhguW8SjIFu1Em1HkWOFtbdWor1egsoqP5P0JoGRQMS+ZhumbY2DGEMA6uQ+WsS2wJBTwSZM8lFarJ0eq4cWzP10Kj5IAvb/EH/sb8vHNmQ6TRgYv9+Lw12pEVW7GCuFKzE9zwJpcPxys/HKC/dZAp5NBrNgPTiEB1x2L8X7SOliz0mAeqYb1F9/Aaks9bNgKhO+RQdekHfNeNY2agnENkAklpk5dYESq8LAA1LwN+rXi9citzcSp3hZcvXLapBm1VgRdvdQkUQ4frkJtrRSlkjwE5wbB3rDfcQGwCM8EOa4V1p/3wpJTjdmbK7G1SIbmlvHfx9B5QsWIBUOabdLAk0hVd1VgZeFG+AjXIEObimMnGnHlUs8YAwbcNTWpxnxgU1MltNoK5JRnwC/LsN+L8V5sOMwjCkH+/BSst5+E5SYVbGMVyDykRFtbzYRIlSuF2JLNhxVDcsOkgfGQ2nxCjdUFgfAq/xRcTTI6v9bh4sDfjep0owZGn4AWDQ1KVKkFSClJxIpdNMz7Yjmmb46GWXg5rFPOwiq+A5bsSrh/rkB5pWpSpPKlfIRk8DGLqBixiJBWk5kSB6NocEmvP8gh+dzPISnnhO/Hnz+i4/SpFpOr0n2mHv7FTNDKvJGs3oaWbg0GvjuOhgaFUXRdnQIS5QFEF3Hglr4EHyT5wIZIMuAPNil9sIo5DCt2JdbskKOqrnpSpBYL+KBz+bBhiR6aM6QiC4bsw8cTv8slbbqXSbr9IJcEQ5dFOGJWWAnmM4qwu2A/JkJqb38bWAdiQCldibjKaNQ0VkCiLAWniIAj1wVzE9bBkrELVrGtsE7uhVVULWZyKrElX4amlppJkcor4cMjUYTZbBECs5TnyYRk7M/LoXWke0N+JP1fgt7EbZYZbhEWuJBgPsKLpsKZyIYttxt7q4/hbF+XSSN933UiSpgEyr7V+GT7QszgBOO/wwpATuwCOa7NOPmPYlXYdVCBNhPTbnuEVJUIW/P4WBBdAdstIiSWqH81awKbN/xI8DB7G9/Hzh2+sf41/c+J/z6ir3gfqLTDTyJ7pOyIgtPOdiwqvIRs9TGcOdM1IVJV9bVYFL0frwfJYBFShiWfK7FfppoUqUKZAGG7BZjDkYCyXYIiqeaJY9aiuU5WYtLAgqlTjdjsP67DxbKND+/mvzl8/8u3hvXCWUYjd8V2EPLoWM5Vwn7vFaQoTuJkT8eESFVppKitV5oU/gip+4R8+KUJMYMtwbqdcgg12nHXqrOMi+wg37FJ9JGBJ7F5ru8Izqti9XcKp955UPjGXb1wptHIfakdJNl0rOAq8XHOVcRJenD0eMczIXV3iQCU7WLMZIkRlqN64pjHYlOjPIj2jCD0bTAbm0QfGRgPm9/1d+GcNlX/P0U2+uGCt6AXTAeUC/BQbgddnjdWpx3CBzmD2Cw+jSPd7ZMiVaUWYVuBAI5xEnzMESOpRIPGw1qT2NRWFKE7xReDn02FdKMD3APTxibRQd/XbrJnv4UzvRMn0f7GXPx13yw8yH8dI+XWgHK+8al0FVIQwC3Ah3sugSU8jcbO9jFIrZCLwMwS4v3NUiyKF4Mn1KC1zTQ2a8t5OLGdhsHP3sGBz5bCOWCn6SR6Yy1p6aDva7fO0/9r6Fhx4qRJtL+lGP97wBb386dg5KAFoPjEaKT3K1cQabvwQeYAAsrOorpeA7FciNBMMWZGyrAyTY4DldUTYrN+3xfoi3LCtwGWSPH3w+zgwqdLoiBIU35cQ9rQtGGO/nbSnzFQk4z+SZJo/5GDuH3QHvf3GoyYA7J5GJHMR2+qPeLi0rFg52l8xKkAc28VVHVak9jUqMrRms3GBWIujgfNQUxgCKwjyp8tiVoTcsRS3sP5rBn6n4qnY6A6EWfPtE+IzbNHJfjLoUW4n/cnXI+ciwtfUNEhK5gUm1ppMY7u2IhBxjRUhbjBNygBFs8zifbU5+Mod7b+b0VmuKRk42xP04TY1FUL0FwvmRSbOsEenExehath03AgaAVcA9NfbBIdaPsKJzPm4Jf8t3FVFoC+47pnwmbT/lScTViKgbD3kBuyFvNC9r7cJHrxmBBdGZ/gb7lv4ppkPb7p1kyKTa1agNb8aAxEO+IEYz5iQhmYFnbo+SbRJ4scIblpTKITIPVyTxU6sxfjp+w38IPYB32dsjHYrJaXoosXgUHOx6hnLEFgcIwxff6RJGrFkgyPSaK/LwtCvpTMqLj1HiEa2imoOTcRUq/0NaKjYBVu8f4T18s90FovhFZeimOZofg+8kPIGDRQQ9L/cBIlM2U3zRmy9HGT6LhFlE4xZyo2WLCUx54miQ5+24qvS9bj+93voI9li10hdMwL3vuHk6g5Q3rBjCWLsY5TvkF6pqIr/82a28+x4V0/8xKTqN6cKW8iR8hoJBKe3x9+NlnXF9vwrte+qCRqw5bdM2fKRWaE0pb0Imva7uuLDEbm5VzVP48kasmU/WTBlBdaMcXWpJdZ1lk/2NrwhkRzc649eJYkSmYqLpNZyhjzINE/90/uaTk/zrThDRXO2XPtztMkUQuW4qgFIacb7ov0KtWsnB/NbHjX02dmDf3f75PojE3yETKhqDVjyl1Ir3pN4117Z1rWUOqsnKFrNmzlj2RCmUMmlNP+2bpIr3L9P2BM0mEgjkKRAAAAAElFTkSuQmCC",
            "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAsTAAALEwEAmpwYAAAD+klEQVR4nN1XXWxUVRA+QtHYds/ZLYIUfFCJDyohGBGMkvRBk951aXdmm/smPphIIlDQmFjfLk8Cd2a3mJBINSYi/iQEXiAxMRoTYwzlAYsJ+ObPG7HdLRBI8C/WzD13b2937912KU0Ik5zk5Jw5Z+Z8M9/MvUrd0eKeWJ7B8rMZpL0a+QMN9K1BvmSQpjTSNYM8I8POaUr2Qp0xOZMZpK1yR9t29VBls0b6yCBP140sYkxroA8N8NPzGs7AuysN8Ge3wWjCoP8M0nHtlnuSX+2WezTwT0tjfHZo5AlTOJBrdgDpcAvvfw7iCuWXNZS3dLr+mvglMpc12cuUaIfohmeSnQCutJ0Xd79o4NGFhECo1YmVXhuCmXtkyHzl4KG1snfLITCFAzkNdGHJkxDoR1MczSaj4JZ7DPAnljK3n4Ya+eNEBoh0D9ITERpD/JQtHlRbtGGgmlRRU/Q3JdmKRCNdMcBfaPDXR4vuieUa6RmNvNsAHdVI32igiwZoMtCPuC1naVL2RMcgv6+RdklFVZ63LLIB/nqxIdVRNYoBvhxe+I9GOpkt0kDvdq9TLVLkjiz4RY18Su4ObVSbFLXEpwk+vqmBv7Mv4mFT5BcEPlMcfTiI5c6xFXJ2lXukO7Hp9HkdBvzXAtTmJuKxJt2ugcMPaqTfbiG5/pQQxaFuEs9bFjzA6v4qthL1upBWG6Qz7RgXVOTsjHLuq6r+vTXljNdU/oYdznhVOcOyJzo54G2pxnNIG6I58DaBKd7zUzi9R/SrqrCuqvIXaio/kzSqypkQnVSERMKs/jJTOvSYqsvOsRVS3Qz6bwgLDPDXQYUDvqyBzwu09uXpxuNO1JFIFFNnAdC/YiiL5VeC5JpHLOyRkb9qKv/2lHpxrYyackbCtXA/HyCWKDqFBQZ53AAdMcCv6hL3Z4f8jdILbB9Qqqby52Zf6ow03itrMSTOpjrQ1SYLHnLL94cOXK8bmFT9vY33/qFeWhNz4HpLOLuQVmvk0wtxoB6euAMCe3OICutiIbiaarwbRh+P5iX/eQmJBr6a5kBuqPLkQkIwrZx3Yjnyg1214ZsjlgX0uSlVHm1kQRZ4XwILXrcvdIbnJqEz0iIJP01FQNc5D/R3O73A0tCZmI+GsbE/2QGgY4m9AOl72wltLxAWmAF6JOgFfV7HbCFapBPd2/kBDfx7u30gg1SwSLj3Cs+FamFiXpOYC+wLd8J9b1V7vSDsB0hvtvoFa6gFseqYP5h4wJZfOt7mF9ElU6K3JEQ596CR7z6Z18OU5ERVOV7rDOvzOjJFfk7abfCJhvyV7e3Bj2j43xggMGWAfpH+EHwRAR2Nfk5DBxqdmN/4EonEPh7//wE0V2CoNf1pOAAAAABJRU5ErkJggg==",
            "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAAFGUlEQVR4nO2ZW09cVRSAp95+gL4Avld9Ehof1ACJ8UXl0irFDNgYESyXAu2c6TAzgEUryrW1xhK5lMveE22KfTBNbFIPyoO23mai1qot6XSfqddSGJhI1Z596DLrFMbpzDAz5zLgw6xkhWTYZ+39nbX22nutY7FkJCMZycj/QRamsreExCxhQczJW9eJATpuAzZ+P2cTTysSbVQC1KmqRBvxN/wfjklmZ1HM/hAhFsVsHprKhkUxW144lZOb3sV7h+7kl8hWhdFJzuicIlFIpDiGM3KMBzyl+OxadkNTWXaECKuYJaQH4NzkXYpEmrhELydb/JpQEg2gt+IBLYg5eeiJVY8EP773QdMh5ICngEv0R70AsV4iP8gBkh8DcyonNyRm2UyHAIBNCiNtnBHFLIgIGK4w6sY5TF10LMTk7ZyRI2YDxAEawbki5w7mlxUHC8tCwYLyIoMQsAknSDeE8l9CoJGeQQAEmS985qmUFx16fNs9c/lltsWHy+9e/Q3DKXqyaxfGQJxwwnh3E4x16VN8doo4Yen8aCwQIy5Db3++sFwIFm4HhAlv7Kg9MTPdDzvLtkK90AdN3cehue8DXYrP1gk9UFXyBMx8ciBmz8gSfVQ3CHpioWD7HvyLKTY6O6EnEMIxfAYcw59DjXAQqhpe06Q1tn5w0bNh3Tt0GnaWlcJfM6Mx2SzRWZOy4DkR7XIMJ/QEQjy3oxnsA9O3LEqv1gq98NGEOzzP8q8nVFUYaTB+Ysc57I50Nqgh8ZJw0DQI1Kbu4zDS2RADwiUiGfIKZ3RbvIwy+Gq9Gt8v1O5LuDDH4Glo3v0KCC094Bz3JRxrf+dT1SbajpvFJFJsAIQcMwLidL0Ofy7+DIGL34LD3ZtwLNpqTgzyni4IvKFyRq4aAXG7OmEp9Atc9n8HjrZ+Yx5hdC6VW3MsCCMPrHVYpRxaQ5+BTegAwfFG0tCqbR1MCIIKlzz3aQZZqSd0g+C+cLV2r2gX2HvfNxRaCnol4Ck1Je1qAWms3gVHJ8ZgWQmq4WVDsAQgde2jSUEUie7SDsI8rUZDa4+rH+au+FMC2d13IjkIo27dIMu/n4Tl+S9BCbyrHaTtEMz+NpMSSCqhpegCwXobN9jyP4Ciwmw0iKQjtFY3+40lpsKgZzYahOvZ7GakX7NBQE/6lX3Wh+SfOpaUi4d1gdj3DcDLLe3w97U/gMtXwdlsB6HtLd0gXCJXNJfA173PbpF9FZz7KoD7KkHxD2gGqdtRBYd6D9yi1eXW9b2iyF6r/SbEip7fryu0tGhyEI/2Wv36V9Y82WeVEUL2Vd5Q/IfTeo0XBqahxv5mzDU+whsSwPQdmkFUmG8qc2Wf1cYvdHehsaUzj4UNY42N5enekS/UwgoXYgQCbaCtWqEXRNIa7/yotxgVfBMKI2cjDWOjAGtsLE9xAVjqPl/t0qXoCbSBtl4seVIto6MgzplS6qJgB1BtnkVMgI0CrLHxLRptPtTaelSImXjNB0YesZgpeD2Idjk2CkTihqH99fB2e40uxWenqDvWE5KqLRazZQMadMPcW1ks+6wh7rUWpaNlOrIeEIBzea1FKsjXFal3FzU2sV3Re8YcAMLTEk6JBDuAmFFMA2Hke9M3dqqCaRGbZ3hg6fYCPstovWkpVm9zO9zIC9ASLpGjXKKzyRdPZ/HuhL0q3Se2mc3ttfYQ+Mnmm98USUP4Yyh6Dr8Z+snmtH/I0dLcTjo4IxnJSEYsaZB/AeDkYE4RCjZ/AAAAAElFTkSuQmCC"
        };

        private void btnNoRemark_Click(object sender, EventArgs e)
        {
            PdfFont boldFont = PdfFontFactory.CreateFont(Resource.CALIBRIB, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            PdfFont regularFont = PdfFontFactory.CreateFont(Resource.CALIBRI, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            string filePath = $"{dataFolder}/NOREMARK-{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.pdf";

            IReportProgress progress = new ReportProgress(value => { /* Some code for the execution progress value processing */ });

            // Creating a report builder instance
            var builder = new PdfReportBuilder<PdfReport>("ESITO DIAGNOSI SMARTREG EBA EVOLUTION 2.0", progress, PageOrientation.Portrait);
           
            var columnSection = new PdfColumnSection(11f,  boldFont, new float[] { 0.64f, 0.36f }, 12f)
            {
                Content = new string[] {
                    "09999 - Banca di Prova", 
                    "Data Riferimento: 2018-09-30" ,
                    "Survey CRM", 
                    "Data Diagnosi: 2023-09-28 16:25:26" ,
                    "",
                    "Data Ricezione kjdkj dmkds "
                }
            };
            builder.AddColumnSection(columnSection);


            builder.AddElement(new PdfSimpleText("La diagnosi non ha rilevato anomalie.", 11f, regularFont, 30f,-50f));
            try
            {
                var report = builder.Build(); // Building report
                report.ToFile($"{filePath}"); // Saving the report into a file

                MessageBox.Show("Pdf generation has been completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
