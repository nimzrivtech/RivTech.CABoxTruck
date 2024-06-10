using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles
{
    public static class DriverReportStyle
    {
        public static void ReportWorksheetStyle(IXLWorksheet sheet, XLWorkbook workbook)
        {
            //Header styles
            IXLRanges rangeReportHeader = sheet.Ranges("A1:H1");
            rangeReportHeader.Style.Font.Bold = true;
            rangeReportHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            //Sheet Columns
            sheet.Column("A").Width = 20;
            sheet.Column("B").Width = 20;	
            sheet.Column("C").Width = 20;	
            sheet.Column("D").Width = 25;	
            sheet.Column("E").Width = 12;	
            sheet.Column("F").Width = 12;	
            sheet.Column("G").Width = 40;
            sheet.Column("H").Width = 40;

            //Turn off scientific notation when editing cell for driver's license
            var dlColumn = sheet.Column("D");
            dlColumn.Style.NumberFormat.Format = "@";
        }
    }
}
