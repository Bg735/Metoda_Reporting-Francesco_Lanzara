using iText.IO.Font;
using iText.Kernel.Font;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Res;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Models.Reports.AgreedOtherThanUsedForPooledTransactions;
using Metoda.Reporting.Pdf.ReportElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.MonthlyReport;

public static class MonthlyReportFakeData
{
    public static void FillBuilderByData(MonthlyReportPdfReportBuilder builder)
    {
        PdfFont boldFont = PdfFontFactory.CreateFont(Resource.CALIBRIB, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
        PdfFont regularFont = PdfFontFactory.CreateFont(Resource.CALIBRI, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

        float fontSize09f = 09f;
        float fontSize11f = 11f;
        float leftMargin0f = 0f;
        float leftMargin10f = 10f;

        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterLine = new PdfReportFilterSection("Custom Filer", "A > 10 AND B <= 13");

        var filterLine2 = new PdfReportFilterSection("Filter #2",
            "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");

        var mainNestedSection = new PdfMainNestedSection(fontSize11f, leftMargin0f, regularFont, boldFont)
        {
            Title = "Dati Controparte",
            Content = new Dictionary<string, string>
                           {
                               { "Codice Controparte", "CTP123456789012 – Società XY" },
                               { "NDG", "0000000309169701" }
                           }
        };

        var datiDiRiepilogo = new PdfDocumentNestedSection(fontSize11f, leftMargin0f, boldFont)
        {
            Title = "Dati di Riepilogo",
            Content = new List<PdfDocumentNestedSectionItem>
                {
                    new PdfDocumentNestedSectionItem(fontSize09f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "Crediti per Cassa 78 cubi di cui",
                        Content = new Dictionary<string, string>
                        {
                            { "• Rischi Autoliquidanti - 550200", "28 cubi"},
                            { "• Rischi a Scadenza - 550400", "18 cubi"},
                            { "• Rischi a Revoca - 550600", "10 cubi"},
                            { "• Finanziamenti a Procedura Concorsuale e altri Finanziamenti Particolari – 550800", "1 cubo"},
                            { "• Sofferenze - 551000", "1 cubo"}
                        }
                    },
                    new PdfDocumentNestedSectionItem(fontSize09f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "Crediti di Firma 4 cubi di cui",
                        Content = new Dictionary<string, string>
                        {
                            { "• Garanzie Connesse con Operazioni di Natura Commerciale - 552200", "1 cubo"},
                            { "• Garanzie Connesse con Operazioni di Natura Finanziaria - 552400", "1 cubo"},
                        }
                    },
                    new PdfDocumentNestedSectionItem(fontSize09f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "Garanzie Ricevute – 553200",
                        TitleEnd = "4 cubi"
                    },
                    new PdfDocumentNestedSectionItem(fontSize09f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "Derivati Finanziari – 553300",
                        TitleEnd = "4 cubi"
                    },
                    new PdfDocumentNestedSectionItem(fontSize09f,  leftMargin10f, regularFont, boldFont)
                    {
                        Title = "Sezione Informativa 8 cubi di cui",
                        Content = new Dictionary<string, string>
                        {
                            { "• Operazioni effettuate per conto di terzi – 554800", "1 cubo"},
                            { "• Crediti per cassa: operazioni in “pool” - azienda capofila - 554900", "1 cubo"},
                            { "• Crediti per cassa: operazioni in “pool” - altra azienda partecipante – 554901", "1 cubo"},
                            { "• Crediti per cassa: operazioni in “pool” – totale – 554902", "1 cubo"},
                            { "• Crediti acquisiti (originariamente) da clientela diversa da intermediari - debitori ceduti - 555100", "1 cubo"},
                            { "• Rischi autoliquidanti - crediti scaduti – 555150", "1 cubo"},
                            { "• Sofferenze - crediti passati a perdita - 555200", "1 cubo"},
                            { "• Sofferenze - crediti ceduti a terzi – 555400", "1 cubo"},

                        }
                    },
                    new PdfDocumentNestedSectionItem(fontSize09f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "Segnalazione Negativa – 555999",
                        TitleEnd = "0 cubi"
                    },
                    new PdfDocumentNestedSectionItem(fontSize09f, leftMargin0f, regularFont, boldFont)
                    {
                        Title = "Totale cubi",
                        TitleEnd = "27 cubi"
                    }
                }
        };

        var sezioneCreditiPerCassa = new PdfDetailedSection(fontSize11f, boldFont)
        {
            Title = "Sezione Crediti per Cassa",
            Content = new List<PdfDetailedSectionItem>
                {
                    new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "550200 - Crediti per Cassa - Rischi Autoliquidanti",
                        Content = new Dictionary<string, string>
                        {
                            { "Divisa", "1 - Euro" },
                            { "Durata Residua", "18 – Oltre 1 anno" },
                            { "Tipo Attività", "63 – Cessione del quinto dello stipendio" },
                            //{ "Qualità del Credito", "2 – Non Deteriorato" },
                            //{ "Stato Rapporto", "138 – Altri Crediti" },
                            //{ "Tipo Garanzia","125 – Assenze di Garanzie reali e/o privilegi" },
                            //{ "Localizzazione","42002 - Potenza" },
                            //{ "Import/export","8 – Altre Operazioni" },
                            //{ "Accordato","25.000" },
                            //{ "Accordato Operativo","24.000" },
                            //{ "Utilizzato", "20.000" },
                            //{ "Importo Garantito","21.000"}
                        }
                    },

                    new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "550200 - Crediti per Cassa - Rischi Autoliquidanti",
                        Content = new Dictionary<string, string>
                        {
                            //{ "Divisa", "1 - Euro" },
                            //{ "Durata Residua", "18 – Oltre 1 anno" },
                            //{ "Tipo Attività", "63 – Cessione del quinto dello stipendio" },
                            //{ "Qualità del Credito", "2 – Non Deteriorato" },
                            //{ "Stato Rapporto", "138 – Altri Crediti" },
                            //{ "Tipo Garanzia","125 – Assenze di Garanzie reali e/o privilegi" },
                            //{ "Localizzazione","42002 - Potenza" },
                            //{ "Import/export","8 – Altre Operazioni" },
                            //{ "Accordato","1.000" },
                            { "Accordato Operativo","22.000" },
                            { "Utilizzato", "5.000" },
                            { "Importo Garantito","14.000"}
                        }
                    },

                    new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "550400 - Crediti per Cassa – Rischi a Scadenza"
                    },
                    //new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    //{
                    //    Title = " 550600 - Crediti per Cassa - Rischi a Revoca"
                    //},
                    //new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    //{
                    //    Title = "550800 – Crediti per Cassa - Finanziamenti a Procedura Concorsuale e altri Finanziamenti Particolari"
                    //},
                    //new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    //{
                    //    Title = "551000 – Crediti per Cassa - Sofferenza"
                    //}
                }
        };

        var sezioneCreditiDiFirma = new PdfDetailedSection(fontSize11f, boldFont)
        {
            Title = "Sezione Crediti di Firma",
            Content = new List<PdfDetailedSectionItem>
                {
                    new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "552200 – Crediti di Firma - Garanzie Connesse con Operazioni di Natura Commerciale"
                    },
                    new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
                    {
                        Title = "552400 – Crediti per Cassa - Garanzie Connesse con Operazioni di Natura Finanziari"
                    }
                }
        };

        //var sezioneGaranzie = new PdfDetailedSection(fontSize11f, boldFont)
        //{
        //    Title = "Sezione Garanzie",
        //    Content = new List<PdfDetailedSectionItem>
        //        {
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "553200 – Garanzie Ricevute"
        //            }
        //        }
        //};

        //var sezioneDerivati = new PdfDetailedSection(fontSize11f, boldFont)
        //{
        //    Title = "Sezione Derivati",
        //    Content = new List<PdfDetailedSectionItem>
        //        {
        //           new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "553300 – Derivati Finanziari"
        //            }
        //        }
        //};

        //var sezioneInformativa = new PdfDetailedSection(fontSize11f, boldFont)
        //{
        //    Title = "Sezione Informativa",
        //    Content = new List<PdfDetailedSectionItem>
        //        {
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "554800 – Operazioni effettuate per conto di terzi"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "554900 – Crediti per cassa: operazioni in “pool” - azienda capofila"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "554901 – Crediti per cassa: operazioni in “pool” - altra azienda partecipante"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "554902 – Crediti per cassa: operazioni in “pool” - totale"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "554800 – Operazioni effettuate per conto di terzi"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "555100 – Crediti acquisiti (originariamente) da clientela diversa da intermediari - debitori ceduti"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "555150 – Rischi autoliquidanti - crediti scaduti"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "555200 – Sofferenze - crediti passati a perdita"
        //            },
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "555400 –Sofferenze - crediti ceduti a terzi"
        //            }
        //        }
        //};

        //var sezioneNegativa = new PdfDetailedSection(fontSize11f, boldFont)
        //{
        //    Title = "Sezione Negativa",
        //    Content = new List<PdfDetailedSectionItem>
        //        {
        //            new PdfDetailedSectionItem(fontSize11f, leftMargin10f, regularFont, boldFont)
        //            {
        //                Title = "555999 – Segnalazione Negativa"
        //            }
        //        }
        //};

        #region Report Table
        var subTables = AgreedOtherThanUsedForPooledTransactionsFakeData.GetPdfTable();

        var tableTotals = subTables.Select(_ => _.TotalRow);
        decimal accordato = tableTotals.Select(_ => _.Row?.Accordato ?? 0).Sum();
        decimal sbilancio = tableTotals.Select(_ => _.Row?.Sbilancio ?? 0).Sum();

        int totalCount = subTables.Select(_ => _.Rows?.Count ?? 0).Sum();

        var mainTotal = new TotalRow<AgreedOtherThanUsedForPooledTransactionsItem>(
         new AgreedOtherThanUsedForPooledTransactionsItem
         {
             Accordato = accordato,
             Sbilancio = sbilancio,
             Utilizzato = accordato - sbilancio
         },
            $"Totale cubo ALL OF POSSIBLE per n° {totalCount} Forme Tecniche"
        );

        AgreedOtherThanUsedForPooledTransactionsPdfReportTable table = new(subTables, mainTotal)
        {
            Title = "Test Table Title"
        };

        AgreedOtherThanUsedForPooledTransactionsPdfReportTable table2 = new(
            subTables,
            mainTotal,
            "Test Table 2 Title",
            Common.Enums.IntermediateTotalLocation.TableBody);

        #endregion

        builder.AddCompanyLine(companyLine);

        builder.AddFilterSection(filterLine);
      //  builder.AddFilterSection(filterLine2);

        builder.AddMainNestedSection(mainNestedSection);
        builder.AddDocumentNestedSection(datiDiRiepilogo);
        builder.AddDetailedSection(sezioneCreditiPerCassa);
        builder.AddDetailedSection(sezioneCreditiDiFirma);
        //builder.AddDetailedSection(sezioneGaranzie);
        //builder.AddDetailedSection(sezioneDerivati);

        builder.AddTable(table);
        builder.AddTable(table2);

        //builder.AddDetailedSection(sezioneInformativa);
        //builder.AddDetailedSection(sezioneNegativa);
    }

    public static void FillBuilderByData(MonthlyReportExcelReportBuilder builder)
    {

        float fontSize12f = 12f;
        float fontSize11f = 11f;
        float leftMargin10f = 10f;
        float fontSize09f = 09f;

        short indention = 1;
        short indention2 = (short)(indention + 1);

        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterLine = new ExcelReportFilterSection();

        var mainNestedSection = new ExcelMainNestedSection(fontSize12f, indention)
        {
            Title = "Dati Controparte",
            Content = new Dictionary<string, string>
                           {
                               { "Codice Controparte", "CTP123456789012 – Società XY" },
                               { "NDG", "0000000309169701" }
                           }
        };
                
        var datiDiRiepilogo = new ExcelDocumentNestedSection(fontSize11f, indention)
        {
            Title = "Dati di Riepilogo",
            Content = new List<ExcelDocumentNestedSectionItem>
                {
                    new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Crediti per Cassa 78 cubi di cui",
                        Content = new Dictionary<string, string>
                        {
                            { "• Rischi Autoliquidanti - 550200", "28 cubi"},
                            { "• Rischi a Scadenza - 550400", "18 cubi"},
                            { "• Rischi a Revoca - 550600", "10 cubi"},
                            { "• Finanziamenti a Procedura Concorsuale e altri Finanziamenti Particolari – 550800", "1 cubo"},
                            { "• Sofferenze - 551000", "1 cubo"}
                        }
                    },
                    new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Crediti di Firma 4 cubi di cui",
                        Content = new Dictionary<string, string>
                        {
                            { "• Garanzie Connesse con Operazioni di Natura Commerciale - 552200", "1 cubo"},
                            { "• Garanzie Connesse con Operazioni di Natura Finanziaria - 552400", "1 cubo"},
                        }
                    },
                   new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Garanzie Ricevute – 553200",
                        TitleEnd = "4 cubi"
                    },
                   new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Derivati Finanziari – 553300",
                        TitleEnd = "4 cubi"
                    },
                   new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Sezione Informativa 8 cubi di cui",
                        Content = new Dictionary<string, string>
                        {
                            { "• Operazioni effettuate per conto di terzi – 554800", "1 cubo"},
                            { "• Crediti per cassa: operazioni in “pool” - azienda capofila - 554900", "1 cubo"},
                            { "• Crediti per cassa: operazioni in “pool” - altra azienda partecipante – 554901", "1 cubo"},
                            { "• Crediti per cassa: operazioni in “pool” – totale – 554902", "1 cubo"},
                            { "• Crediti acquisiti (originariamente) da clientela diversa da intermediari - debitori ceduti - 555100", "1 cubo"},
                            { "• Rischi autoliquidanti - crediti scaduti – 555150", "1 cubo"},
                            { "• Sofferenze - crediti passati a perdita - 555200", "1 cubo"},
                            { "• Sofferenze - crediti ceduti a terzi – 555400", "1 cubo"},

                        }
                    },
                   new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Segnalazione Negativa – 555999",
                        TitleEnd = "0 cubi"
                    },
                   new ExcelDocumentNestedSectionItem(fontSize09f, indention2)
                    {
                        Title = "Totale cubi",
                        TitleEnd = "27 cubi"
                    }
                }
        };

        var sezioneCreditiPerCassa = new ExcelDetailedSection(fontSize11f, indention)
        {
            Title = "Sezione Crediti per Cassa",
            Content = new List<ExcelDetailedSectionItem>
                {
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "550200 - Crediti per Cassa - Rischi Autoliquidanti",
                        Content = new Dictionary<string, string>
                        {
                            { "Divisa", "1 - Euro" },
                            { "Durata Residua", "18 – Oltre 1 anno" },
                            { "Tipo Attività", "63 – Cessione del quinto dello stipendio" },
                            { "Qualità del Credito", "2 – Non Deteriorato" },
                            { "Stato Rapporto", "138 – Altri Crediti" },
                            { "Tipo Garanzia","125 – Assenze di Garanzie reali e/o privilegi" },
                            { "Localizzazione","42002 - Potenza" },
                            { "Import/export","8 – Altre Operazioni" },
                            { "Accordato","25.000" },
                            { "Accordato Operativo","24.000" },
                            { "Utilizzato", "20.000" },
                            { "Importo Garantito","21.000"}
                        }
                    },

                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "550200 - Crediti per Cassa - Rischi Autoliquidanti",
                        Content = new Dictionary<string, string>
                        {
                            { "Divisa", "1 - Euro" },
                            { "Durata Residua", "18 – Oltre 1 anno" },
                            { "Tipo Attività", "63 – Cessione del quinto dello stipendio" },
                            { "Qualità del Credito", "2 – Non Deteriorato" },
                            { "Stato Rapporto", "138 – Altri Crediti" },
                            { "Tipo Garanzia","125 – Assenze di Garanzie reali e/o privilegi" },
                            { "Localizzazione","42002 - Potenza" },
                            { "Import/export","8 – Altre Operazioni" },
                            { "Accordato","1.000" },
                            { "Accordato Operativo","22.000" },
                            { "Utilizzato", "5.000" },
                            { "Importo Garantito","14.000"}
                        }
                    },

                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "550400 - Crediti per Cassa – Rischi a Scadenza"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = " 550600 - Crediti per Cassa - Rischi a Revoca"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "550800 – Crediti per Cassa - Finanziamenti a Procedura Concorsuale e altri Finanziamenti Particolari"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "551000 – Crediti per Cassa - Sofferenza"
                    }
                }
        };

        var sezioneCreditiDiFirma = new ExcelDetailedSection(fontSize11f, indention)
        {
            Title = "Sezione Crediti di Firma",
            Content = new List<ExcelDetailedSectionItem>
                {
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "552200 – Crediti di Firma - Garanzie Connesse con Operazioni di Natura Commerciale"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "552400 – Crediti per Cassa - Garanzie Connesse con Operazioni di Natura Finanziari"
                    }
                }
        };

        var sezioneGaranzie = new ExcelDetailedSection(fontSize11f, indention)
        {
            Title = "Sezione Garanzie",
            Content = new List<ExcelDetailedSectionItem>
                {
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "553200 – Garanzie Ricevute"
                    }
                }
        };

        var sezioneDerivati = new ExcelDetailedSection(fontSize11f, indention)
        {
            Title = "Sezione Derivati",
            Content = new List<ExcelDetailedSectionItem>
                {
                   new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "553300 – Derivati Finanziari"
                    }
                }
        };

        var sezioneInformativa = new ExcelDetailedSection(fontSize11f, indention)
        {
            Title = "Sezione Informativa",
            Content = new List<ExcelDetailedSectionItem>
                {
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "554800 – Operazioni effettuate per conto di terzi"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "554900 – Crediti per cassa: operazioni in “pool” - azienda capofila"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "554901 – Crediti per cassa: operazioni in “pool” - altra azienda partecipante"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "554902 – Crediti per cassa: operazioni in “pool” - totale"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "554800 – Operazioni effettuate per conto di terzi"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "555100 – Crediti acquisiti (originariamente) da clientela diversa da intermediari - debitori ceduti"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "555150 – Rischi autoliquidanti - crediti scaduti"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "555200 – Sofferenze - crediti passati a perdita"
                    },
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "555400 –Sofferenze - crediti ceduti a terzi"
                    }
                }
        };

        var sezioneNegativa = new ExcelDetailedSection(fontSize11f, indention)
        {
            Title = "Sezione Negativa",
            Content = new List<ExcelDetailedSectionItem>
                {
                    new ExcelDetailedSectionItem(fontSize11f, indention2)
                    {
                        Title = "555999 – Segnalazione Negativa"
                    }
                }
        };

        #region Report Table
        var subTables = AgreedOtherThanUsedForPooledTransactionsFakeData.GetExcelTable();

        var tableTotals = subTables.Select(_ => _.TotalRow);
        decimal accordato = tableTotals.Select(_ => _.Row?.Accordato ?? 0).Sum();
        decimal sbilancio = tableTotals.Select(_ => _.Row?.Sbilancio ?? 0).Sum();

        int totalCount = subTables.Select(_ => _.Rows?.Count ?? 0).Sum();

        var mainTotal = new TotalRow<AgreedOtherThanUsedForPooledTransactionsItem>(
         new AgreedOtherThanUsedForPooledTransactionsItem
         {
             Accordato = accordato,
             Sbilancio = sbilancio,
             Utilizzato = accordato - sbilancio
         },
            $"Totale cubo ALL OF POSSIBLE per n° {totalCount} Forme Tecniche"
        );

        AgreedOtherThanUsedForPooledTransactionsExcelReportTable table = new(subTables, mainTotal)
        {
            Title = "Test Table Title"
        };
        #endregion

        builder.AddCompanyLine(companyLine);

        builder.AddFilterSection(filterLine);

        builder.AddMainNestedSection(mainNestedSection);
        builder.AddDocumentNestedSection(datiDiRiepilogo);
        builder.AddDetailedSection(sezioneCreditiPerCassa);
        builder.AddDetailedSection(sezioneCreditiDiFirma);
        builder.AddDetailedSection(sezioneGaranzie);
        builder.AddDetailedSection(sezioneDerivati);

        builder.AddTable(table);

        builder.AddDetailedSection(sezioneInformativa);
        builder.AddDetailedSection(sezioneNegativa);
    }
}