using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles;
using RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Common
{
    public class ReportGenerator : IReportGenerator
    {
        public void GenerateReportToExcel(string reportType, string reportContent, MemoryStream memoryStream,bool? isRaterV7 = null)
        {
            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(reportContent);

            if (dataTable != null)
            {
                IXLWorksheet sheet;

                if (reportType == "POMReport")
                {
                    dataTable.TableName = reportType;

                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        sheet = workbook.Worksheets.Add("Proof of Mailing List");

                        POMReportWorksheetStyle.ReportWorksheetStyle(sheet, dataTable);

                        workbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else if (reportType == "DMVReport" && dataTable.Rows.Count > 0)
                {
                    dataTable.TableName = reportType;

                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        sheet = workbook.Worksheets.Add(dataTable);

                        sheet.Table(reportType).ShowAutoFilter = false;
                        sheet.Table(reportType).Theme = XLTableTheme.None;

                        DMVReportWorksheetStyle.ReportWorksheetStyle(sheet);

                        workbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else if (reportType == "InvoiceDetailReport" && dataTable.Rows.Count > 0)
                {
                    dataTable.TableName = reportType;

                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        sheet = workbook.Worksheets.Add(dataTable);

                        sheet.Table(reportType).ShowAutoFilter = false;
                        sheet.Table(reportType).Theme = XLTableTheme.None;

                        InvoiceDetailReportWorksheetStyle.ReportWorksheetStyle(sheet);

                        workbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else if (reportType == "VehicleReport" && dataTable.Rows.Count > 0)
                {
                    dataTable.TableName = "Vehicles";

                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        sheet = workbook.Worksheets.Add(dataTable);

                        sheet.Table(dataTable.TableName).ShowAutoFilter = false;
                        sheet.Table(dataTable.TableName).Theme = XLTableTheme.None;

                        VehicleReportStyle.ReportWorksheetStyle(sheet, workbook, isRaterV7);

                        workbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else if (reportType == "DriverReport" && dataTable.Rows.Count > 0)
                {
                    dataTable.TableName = "Drivers";

                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        sheet = workbook.Worksheets.Add(dataTable);

                        sheet.Table(dataTable.TableName).ShowAutoFilter = false;
                        sheet.Table(dataTable.TableName).Theme = XLTableTheme.None;

                        DriverReportStyle.ReportWorksheetStyle(sheet, workbook);

                        workbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else if (reportType == "AgingReport" && dataTable.Rows.Count > 0)
                {
                    dataTable.TableName = reportType;

                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        sheet = workbook.Worksheets.Add(dataTable);

                        sheet.Table(reportType).ShowAutoFilter = false;
                        sheet.Table(reportType).Theme = XLTableTheme.None;

                        AgingReportWorksheetStyle.ReportWorksheetStyle(sheet);

                        workbook.SaveAs(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
        }
    }
}
