using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services.ExcelManager
{
    public class NpoiExcelManager : IExcelManager
    {
        public List<PropertyHeader> HeaderMap { get; set; }

        public List<VehicleDTO> AddVehicles(Stream fileStream)
        {
            var excelVehicles = new List<VehicleDTO>();
            try
            {
                IWorkbook book = new XSSFWorkbook(fileStream);

                var worksheet = book.GetSheet("Vehicles");
                GetColumnsNpoi(worksheet);

                var vinColumnIndex = HeaderMap.Single(hm => hm.Header == "VIN").ColumnIndex;
                var row = 1; // Row 0 are headers

                //while (!string.IsNullOrEmpty(GetNpoiCellValue(worksheet, row, vinColumnIndex)))
                while (HeaderMap.Any(hm => !string.IsNullOrEmpty(GetNpoiCellValue(worksheet, row, hm.ColumnIndex))))
                {
                    var excelVehicle = new VehicleDTO();
                    foreach (var map in HeaderMap)
                        if (map.ColumnIndex > -1)
                        {
                            dynamic excelValue = GetNpoiCellValue(worksheet, row, map.ColumnIndex);
                            excelValue = TryBoolean(excelValue, map.PropertyType);
                            //var typedValue = excelValue == null ? null : Convert.ChangeType(excelValue, map.PropertyType);
                            dynamic typedValue;
                            if (map.PropertyType == typeof(decimal))
                            {
                                try {

                                    var excelValueString = excelValue.ToString();
                                    if (excelValueString.StartsWith("$"))
                                        excelValue = excelValueString.Replace("$", string.Empty);

                                    typedValue = excelValue == null ? null : Convert.ChangeType(excelValue, map.PropertyType);
                                } catch {
                                    typedValue = null;
                                }
                            }
                            else
                            {
                                if (map.ColumnIndex == 0)
                                {
                                    typedValue = excelValue?.Length > 17 || excelValue?.Length <= 0 ? null : excelValue?.Contains(" ") ? null : NullIf(excelValue) == null ? null : Convert.ChangeType(excelValue, map.PropertyType);
                                }
                                else {
                                    typedValue = NullIf(excelValue) == null ? null : Convert.ChangeType(excelValue, map.PropertyType);
                                }

                            }
                                

                            var propertyInfo = excelVehicle.GetType().GetProperty(map.PropertyName);
                            propertyInfo?.SetValue(excelVehicle, typedValue, null);
                        }

                    excelVehicles.Add(excelVehicle);
                    row++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return excelVehicles;
        }

        public List<DriverDTO> AddDrivers(Stream fileStream)
        {
            var excelDrivers = new List<DriverDTO>();
            try
            {
                IWorkbook book = new XSSFWorkbook(fileStream);

                var worksheet = book.GetSheet("Drivers");
                GetColumnsNpoi(worksheet);

                //var vinColumnIndex = HeaderMap.Single(hm => hm.Header == "DL Number").ColumnIndex;
                var row = 1; // Row 0 are headers


                //while (!string.IsNullOrEmpty(GetNpoiCellValue(worksheet, row, vinColumnIndex)))
                while (HeaderMap.Any(hm => !string.IsNullOrEmpty(GetNpoiCellValue(worksheet, row, hm.ColumnIndex))))
                {
                    var excelDriver = new DriverDTO();
                    foreach (var map in HeaderMap)
                        if (map.ColumnIndex > -1)
                        {
                            dynamic excelValue = GetNpoiCellValue(worksheet, row, map.ColumnIndex);
                            excelValue = TryBoolean(excelValue, map.PropertyType);

                            dynamic typedValue;

                            if (map.PropertyType == typeof(DateTime))
                                try { typedValue = excelValue == null ? null : Convert.ChangeType(excelValue, map.PropertyType); } catch { typedValue = null; }
                            else
                                typedValue = NullIf(excelValue) == null ? null : Convert.ChangeType(excelValue, map.PropertyType);

                            var propertyInfo = excelDriver.GetType().GetProperty(map.PropertyName);
                            propertyInfo?.SetValue(excelDriver, typedValue, null);
                        }

                    excelDrivers.Add(excelDriver);
                    row++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return excelDrivers;
        }

        private void GetColumnsNpoi(ISheet worksheet)
        {
            var i = 0;
            while (!string.IsNullOrEmpty(GetNpoiCellValue(worksheet, 0, i)))
            {
                var headerValue = GetNpoiCellValue(worksheet, 0, i);
                var header = headerValue.Trim();
                var map = HeaderMap.FirstOrDefault(hm => hm.Header.Equals(header));
                if (map != null) map.ColumnIndex = i;
                i++;
            }
        }

        private static string GetNpoiCellValue(ISheet worksheet, int rowIndex, int columnIndex)
        {
            var row = worksheet.GetRow(rowIndex);
            var cell = row?.GetCell(columnIndex);
            if (cell == null) return "";
            return cell.CellType switch
            {
                //CellType.Numeric => cell.NumericCellValue.ToString(CultureInfo.InvariantCulture),

                CellType.Numeric =>
                         DateUtil.IsCellDateFormatted(cell)
                                ? cell.DateCellValue.ToString()
                                : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture),
                CellType.String => cell.StringCellValue,
                CellType.Boolean => cell.BooleanCellValue.ToString(),
                CellType.Unknown => cell.BooleanCellValue.ToString(),
                _ => ""
            };
        }

        private static dynamic TryBoolean(dynamic value, Type propertyType)
        {
            if (propertyType != typeof(bool)) return value;
            return value == "Yes";
        }

        public static string NullIf(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) { return null; }
            return value;
        }
    }
}