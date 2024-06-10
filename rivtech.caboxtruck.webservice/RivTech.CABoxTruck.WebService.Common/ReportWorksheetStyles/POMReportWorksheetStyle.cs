using ClosedXML.Excel;
using RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Common.ReportWorksheetStyles
{
    public static class POMReportWorksheetStyle
    {
        public static void ReportWorksheetStyle(IXLWorksheet sheet, DataTable reportData)
        {
            var totalRowCount = (reportData.Rows.Count != 0) ? reportData.Rows.Count : 1;

            //Fill entire sheet with white background
            //IXLRanges rangeWhiteBackgroundColor = sheet.Ranges($"A1:O{sheet.LastRowUsed().RowNumber()}");
            //rangeWhiteBackgroundColor.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFFFFF"));
            sheet.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFFFFF"));

            IXLColumn columnCount = sheet.Column("A");
            columnCount.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            IXLColumns columnsPostageAndFee = sheet.Columns("J:K");
            columnsPostageAndFee.Style.NumberFormat.Format = "$ #,##0.00";
            columnsPostageAndFee.Style.Font.SetFontColor(XLColor.Black);
            columnsPostageAndFee.Style.Font.SetFontName("Calibri");
            columnsPostageAndFee.Style.Font.SetFontSize(10);
            columnsPostageAndFee.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            //--------Report Header-------//
            //Add double borders at top of page
            IXLRanges rangeDoubleBorders = sheet.Ranges("B1:K1");
            rangeDoubleBorders.Style.Border.TopBorder = XLBorderStyleValues.Double;
            rangeDoubleBorders.Style.Border.BottomBorder = XLBorderStyleValues.Double;
            IXLRanges rangeDoubleBorderAtK = sheet.Ranges("K1");
            rangeDoubleBorderAtK.Style.Border.RightBorder = XLBorderStyleValues.Double;

            //Title
            sheet.SetRivingtonPartnersTitle();

            //Table Data
            sheet.SetDataTableHeaders(reportData, totalRowCount);
            sheet.SetDataTable(reportData);

            //-------Report Footer---------//
            sheet.Rows($"{totalRowCount + 7}").Height = 81;

            IXLCell cellTotalNumberOfPiecesSender = sheet.Cell($"A{totalRowCount + 5}");
            cellTotalNumberOfPiecesSender.Value = POMReportConstants.PiecesSentBySenderMessage;
            cellTotalNumberOfPiecesSender.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            cellTotalNumberOfPiecesSender.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            cellTotalNumberOfPiecesSender.Style.SetFontToArialBold(fontSize: 11);
            cellTotalNumberOfPiecesSender.Style.Font.SetBold(false);
            cellTotalNumberOfPiecesSender.Style.Alignment.SetWrapText(true);
            IXLRange mergedTotalNumberOfPieces = sheet.Range($"A{totalRowCount + 5}:B{totalRowCount + 5}").Merge();
            mergedTotalNumberOfPieces.Style.Border.SetBorders(XLBorderStyleValues.Thick);
            mergedTotalNumberOfPieces.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);

            //Total Number of pieces sent by sender
            IXLCell cellTotalNumberOfPiecesCount = sheet.Cell($"A{totalRowCount + 6}");
            cellTotalNumberOfPiecesCount.Value = $"{reportData.Rows.Count}";
            cellTotalNumberOfPiecesCount.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cellTotalNumberOfPiecesCount.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            cellTotalNumberOfPiecesCount.Style.SetFontToArialBold(fontSize: 36);
            IXLRange mergedPiecesCount = sheet.Range($"A{totalRowCount + 6}:B{totalRowCount + 7}").Merge();
            mergedPiecesCount.Style.Border.SetBorders(XLBorderStyleValues.Thick);

            IXLCell cellTotalNumberOfPiecesPostOffice = sheet.Cell($"C{totalRowCount + 5}");
            cellTotalNumberOfPiecesPostOffice.Value = POMReportConstants.PiecesReceivedAtPostOfficeMessage;
            cellTotalNumberOfPiecesPostOffice.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            cellTotalNumberOfPiecesPostOffice.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            cellTotalNumberOfPiecesPostOffice.Style.SetFontToArialBold(fontSize: 11);
            IXLRange mergedPiecesCountPostOffice = sheet.Range($"C{totalRowCount + 5}:D{totalRowCount + 7}").Merge();
            mergedPiecesCountPostOffice.Style.Border.SetBorders(XLBorderStyleValues.Thick);
            mergedPiecesCountPostOffice.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);

            IXLCell cellPostMaster = sheet.Cell($"E{totalRowCount + 5}");
            cellPostMaster.Value = POMReportConstants.PostMasterMessage;
            cellPostMaster.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            cellPostMaster.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            cellPostMaster.Style.SetFontToArialBold(fontSize: 11);
            IXLRange mergedPostMaster = sheet.Range($"E{totalRowCount + 5}:E{totalRowCount + 7}").Merge();
            mergedPostMaster.Style.Border.SetBorders(XLBorderStyleValues.Thick);
            mergedPostMaster.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);

            IXLCell cellFullDeclaration = sheet.Cell($"F{totalRowCount + 5}");
            cellFullDeclaration.Value = POMReportConstants.FullDeclarationMessage;
            cellFullDeclaration.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            cellFullDeclaration.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            cellFullDeclaration.Style.SetFontToArialBold(fontSize: 11);
            cellFullDeclaration.Style.Font.SetBold(false);
            cellFullDeclaration.Style.Alignment.SetWrapText(true);
            IXLRange mergedFullDeclaration = sheet.Range($"F{totalRowCount + 5}:K{totalRowCount + 7}").Merge();
            mergedFullDeclaration.Style.Border.SetBorders(XLBorderStyleValues.Thick);
            mergedFullDeclaration.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);

            //Sheet Columns
            sheet.Column("A").Width = 6.44;
            sheet.Column("B").Width = 18.44;
            sheet.Column("C").Width = 7.22;
            sheet.Column("D").Width = 63.56;
            sheet.Column("E").Width = 41.44;
            sheet.Column("F").Width = 20.33;
            sheet.Column("G").Width = 11.44;
            sheet.Column("H").Width = 12.22;
            sheet.Column("I").Width = 38.67;
            sheet.Column("J").Width = 8.22;
            sheet.Column("K").Width = 6;
            sheet.Column("L").Width = 27.56;
            sheet.Column("M").Width = 11.89;

            IXLColumns lastColumns = sheet.Columns($"M:{sheet.LastColumn().ColumnLetter()}");
            lastColumns.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFFFFF"));
        }

        private static void SetRivingtonPartnersTitle(this IXLWorksheet sheet)
        {
            sheet.Cell("A2").Value = POMReportConstants.HeaderTitle;
            sheet.Cell("A2").Style.Border.LeftBorder = XLBorderStyleValues.Double;
            sheet.Cell("K2").Style.Border.RightBorder = XLBorderStyleValues.Double;
            sheet.Cell("A2").Style.SetFontToArialBold(fontSize: 11);
            sheet.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Range("A2:K2").Merge();

            sheet.Cell("A3").Style.Border.LeftBorder = XLBorderStyleValues.Double;
            sheet.Cell("K3").Style.Border.RightBorder = XLBorderStyleValues.Double;
            sheet.Cell("A3").Value = POMReportConstants.HeaderAddress;
            sheet.Cell("A3").Style.SetFontToArialBold(fontSize: 11);
            sheet.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Range("A3:K3").Merge();
        }

        private static void SetDataTableHeaders(this IXLWorksheet sheet, DataTable reportData, int rowCount)
        {
            //Header
            IXLRanges rangeReportHeader = sheet.Ranges("A4:K4");
            rangeReportHeader.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFFF00"));
            rangeReportHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rangeReportHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rangeReportHeader.Style.Border.SetBorders(XLBorderStyleValues.Thin);
            rangeReportHeader.Style.SetFontToArialBold(fontSize: 11);
            sheet.Ranges("A4,G4,J4,K4").Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFFFFF"));

            foreach (int x in Enumerable.Range(1, 9))
            {
                switch (x)
                {
                    case 1: sheet.Cell("A4").Value = "Count"; break;
                    case 2: sheet.Cell("B4").Value = "Policy #"; break;
                    case 3: sheet.Cell("D4").Value = "Name"; break;
                    case 4: sheet.Cell("E4").Value = "Address"; break;
                    case 5: sheet.Cell("G4").Value = "Trans Date"; break;
                    case 6: sheet.Cell("H4").Value = "Cancel Date"; break;
                    case 7: sheet.Cell("I4").Value = "Notice Type"; break;
                    case 8: sheet.Cell("J4").Value = "Postage"; break;
                    case 9: sheet.Cell("K4").Value = "Fee"; break;
                }
            }

            for (int row = 0; row <= rowCount; row++)
            {
                sheet.Range($"B{row + 4}:C{row + 4}").Merge();
                sheet.Range($"E{row + 4}:F{row + 4}").Merge();

                sheet.Ranges($"A{row + 4}:K{row + 4}").Style.Border.SetBorders(XLBorderStyleValues.Thin);
            }
        }

        private static void SetDataTable(this IXLWorksheet sheet, DataTable reportData)
        {
            for (int row = 0; row < reportData.Rows.Count; row++)
            {
                for (int column = 0; column < reportData.Columns.Count; column++)
                {
                    var reportDataValue = reportData.Rows[row][column];

                    switch (column)
                    {
                        case 0: sheet.Cell(row + 5, column + 1).Value = row + 1; break;
                        case 2: sheet.Cell(row + 5, column + 2).Value = reportDataValue.ToString(); break; //Name
                        case 3: sheet.Cell(row + 5, column + 2).Value = reportDataValue.ToString(); break; //Address
                        case 4: sheet.Cell(row + 5, column + 3).Value = reportDataValue; break; //TransactionDate
                        case 5: sheet.Cell(row + 5, column + 3).Value = reportDataValue; break; //CancelDate
                        case 6: sheet.Cell(row + 5, column + 3).Value = reportDataValue.ToString(); break; //NoticeType
                        case 7: sheet.Cell(row + 5, column + 3).Value = ""; break; //Postage
                        case 8: sheet.Cell(row + 5, column + 3).Value = ""; break; //Fee 
                        default: sheet.Cell(row + 5, column + 1).Value = reportDataValue.ToString(); break;
                    }
                }

                sheet.Ranges($"A{row + 5}:K{row + 5}").Style.Border.SetBorders(XLBorderStyleValues.Thin);
            }
        }

        private static void SetFontToArialBold(this IXLStyle sheetStyle, int fontSize)
        {
            sheetStyle.Font.SetFontColor(XLColor.Black);
            sheetStyle.Font.SetBold(true);
            sheetStyle.Font.SetFontName("Arial");
            sheetStyle.Font.SetFontSize(fontSize);
        }

        private static void SetBorders(this IXLBorder border, XLBorderStyleValues value)
        {
            border.LeftBorder = value;
            border.RightBorder = value;
            border.TopBorder = value;
            border.BottomBorder = value;
        }
    }
}
