using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles
{
    public static class AgingReportWorksheetStyle
    {
        public static void ReportWorksheetStyle(IXLWorksheet sheet)
        {
            //Header styles
            IXLRanges rangeReportHeader = sheet.Ranges("A1:O1");
            rangeReportHeader.Style.Font.Bold = true;
            rangeReportHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            //Cell Sizes
            sheet.Column("A").Width = 28;
            sheet.Column("B").Width = 14;
            sheet.Column("C").Width = 45;
            sheet.Column("D").Width = 32;
            sheet.Column("E").Width = 11;
            sheet.Column("F").Width = 17.5;
            sheet.Column("G").Width = 10.25;
            sheet.Column("H").Width = 16;
            sheet.Column("I").Width = 12;
            sheet.Column("J").Width = 15;
            sheet.Column("K").Width = 15;
            sheet.Column("L").Width = 15;
            sheet.Column("M").Width = 15;

            //Currency Columns
            string[] currencyColumns = { "F", "H", };

            foreach (var col in currencyColumns)
            {
                foreach (var cell in sheet.Column(col).CellsUsed())
                {
                    double value;
                    if (double.TryParse(cell.GetString(), out value))
                    {
                        cell.Style.NumberFormat.Format = "$#,##0.00;($#,##0.##)";
                        if (value < 0)
                        {
                            cell.Style.Font.FontColor = XLColor.Red;
                        }
                    }
                }
            }

            //String to currency columns
            string[] stringToCurrencyColumns = { "I", "J", "K", "L", "M" };
            foreach (var col in stringToCurrencyColumns)
            {
                foreach (var cell in sheet.Column(col).CellsUsed())
                {
                    string stringValue = cell.GetString();

                    double value;
                    if (double.TryParse(stringValue, out value))
                    {
                        cell.SetValue(value);
                        cell.Style.NumberFormat.Format = "$#,##0.00;($#,##0.00)";

                        if (value < 0)
                        {
                            cell.Style.Font.FontColor = XLColor.Red;
                        }
                    }
                }
            }

            // Date Columns (B, E, G)
            string[] dateColumns = { "B", "E", "G" };
            foreach (var col in dateColumns)
            {
                foreach (var cell in sheet.Column(col).CellsUsed())
                {
                    if (DateTime.TryParse(cell.GetString(), out DateTime dateValue))
                    {
                        cell.SetValue(dateValue);
                        cell.Style.DateFormat.Format = "mm/dd/yyyy"; // or any desired date format
                    }
                }
            }
        }
    }
}
