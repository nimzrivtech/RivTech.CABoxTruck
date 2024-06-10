using System;
using System.Collections.Generic;
using System.Text;
using ClosedXML.Excel;

namespace RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles
{
    public static class InvoiceDetailReportWorksheetStyle
    {
        public static void ReportWorksheetStyle(IXLWorksheet sheet)
        {
            sheet.Column("A").Width = 31.43;
            sheet.Column("B").Width = 13.57;
            sheet.Column("C").Width = 10.71;
            sheet.Column("D").Width = 20.71;
            sheet.Column("E").Width = 35.29;
            sheet.Column("F").Width = 17.57;
        }
    }
}
