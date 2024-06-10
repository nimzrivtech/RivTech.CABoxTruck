using System;
using System.Collections.Generic;
using System.Text;
using ClosedXML.Excel;

namespace RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles
{
    public static class DMVReportWorksheetStyle
    {
        public static void ReportWorksheetStyle(IXLWorksheet sheet)
        {
            IXLRanges rangeBlueColors = sheet.Ranges("A1:B1,D1,J1:L1,N1:Q1,X1:AD1,AF1:AG1,AI1:AL1");
            IXLRanges rangePinkColors = sheet.Ranges("C1");
            IXLRanges rangeGrayColors = sheet.Ranges("E1:I1,M1,R1:W1,AE1,AH1");
            IXLRanges rangeRedColors = sheet.Ranges("AM1:AN1");
            rangeBlueColors.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#99CCFF"));
            rangePinkColors.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FF99CC"));
            rangeRedColors.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#C00000"));
            rangeGrayColors.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#969696"));

            IXLRanges rangeBoldHeaders = sheet.Ranges("AM1:BA1");
            IXLRanges rangeBoldColumns = sheet.Ranges($"B2:B{sheet.LastRowUsed().RowNumber()}, C2:C{sheet.LastRowUsed().RowNumber()}, F2:F{sheet.LastRowUsed().RowNumber()}");
            rangeBoldHeaders.Style.Font.SetBold();
            rangeBoldColumns.Style.Font.SetBold();

            IXLRanges rangeHeaderVerticalBottom = sheet.Ranges("A1:AL1");
            rangeHeaderVerticalBottom.Style
              .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);

            IXLRanges rangeHeaderVerticalMiddle = sheet.Ranges("AM1:BA1");
            rangeHeaderVerticalMiddle.Style
              .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sheet.Columns("B, C, F").Style
              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            IXLRanges rangeHeader = sheet.Ranges("A1:BA1");
            rangeHeader.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rangeHeader.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rangeHeader.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            IXLRanges rangeHeaderWrapText = sheet.Ranges("A1:AL1");
            rangeHeaderWrapText.Style.Alignment.SetWrapText(true);

            sheet.Column("A").Width = 10.57;
            sheet.Column("B").Width = 8.43;
            sheet.Column("C").Width = 8.43;
            sheet.Column("D").Width = 7.57;
            sheet.Column("E").Width = 14.57;
            sheet.Column("F").Width = 7.57;
            sheet.Column("G").Width = 9.29;
            sheet.Column("H").Width = 6.29;
            sheet.Column("I").Width = 8.43;
            sheet.Column("J").Width = 12;
            sheet.Column("K").Width = 8.43;
            sheet.Column("L").Width = 20;
            sheet.Column("M").Width = 6.71;
            sheet.Column("N").Width = 14;
            sheet.Column("O").Width = 10;
            sheet.Column("P").Width = 8.43;
            sheet.Column("Q").Width = 8.43;
            sheet.Column("R").Width = 8.43;
            sheet.Column("S").Width = 6.86;
            sheet.Column("T").Width = 8.43;
            sheet.Column("U").Width = 8.29;
            sheet.Column("V").Width = 6.14;
            sheet.Column("W").Width = 6.14;
            sheet.Column("X").Width = 14.43;
            sheet.Column("Y").Width = 13.86;
            sheet.Column("Z").Width = 8.43;
            sheet.Column("AA").Width = 8.43;
            sheet.Column("AB").Width = 11.29;
            sheet.Column("AC").Width = 20;
            sheet.Column("AD").Width = 8.57;
            sheet.Column("AE").Width = 6.86;
            sheet.Column("AF").Width = 8.43;
            sheet.Column("AG").Width = 18.57;
            sheet.Column("AH").Width = 15.29;
            sheet.Column("AI").Width = 9;
            sheet.Column("AJ").Width = 9;
            sheet.Column("AK").Width = 8.43;
            sheet.Column("AL").Width = 29.71;
            sheet.Column("AM").Width = 20.29;
            sheet.Column("AN").Width = 26.86;
            sheet.Column("AO").Width = 15.57;
            sheet.Column("AP").Width = 26.71;
            sheet.Column("AQ").Width = 17.57;
            sheet.Column("AR").Width = 10.57;
            sheet.Column("AS").Width = 15.57;
            sheet.Column("AT").Width = 16;
            sheet.Column("AU").Width = 8.43;
            sheet.Column("AV").Width = 8.43;
            sheet.Column("AW").Width = 8.43;
            sheet.Column("AX").Width = 8.43;
            sheet.Column("AY").Width = 8.43;
            sheet.Column("AZ").Width = 8.43;
            sheet.Column("BA").Width = 8.43;
        }
    }
}
