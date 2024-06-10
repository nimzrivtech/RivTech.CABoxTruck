using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using ExcelRange = Microsoft.Office.Interop.Excel.Range;

namespace RivTech.CABoxTruck.WebService.Service.Services.ExcelManager
{
    public class InteropExcelManager : IExcelManager
    {
        public List<PropertyHeader> HeaderMap { get; set; }

        public List<DriverDTO> AddDrivers(Stream fileStream)
        {
            var excelDrivers = new List<DriverDTO>();
            var excelApp = new Application();

            var exceWorkbook = excelApp.Workbooks.Open(GetFilename(fileStream));
            var worksheet = (Worksheet)exceWorkbook.Worksheets[1];
            try
            {
                GetColumns(worksheet);
                var vinColumnIndex = HeaderMap.Single(hm => hm.Header == "DL Number").ColumnIndex;
                var row = 2; // Row 1 are headers

                while (!string.IsNullOrEmpty(((ExcelRange)worksheet.Cells[row, vinColumnIndex]).Value?.ToString()))
                {
                    var excelDriver = new DriverDTO();
                    foreach (var map in HeaderMap)
                        if (map.ColumnIndex > 0)
                        {
                            var excelValue = ((ExcelRange)worksheet.Cells[row, map.ColumnIndex]).Value;
                            excelValue = TryBoolean(excelValue, map.PropertyType);
                            var typedValue = excelValue == null ? null : Convert.ChangeType(excelValue, map.PropertyType);
                            var propertyInfo = excelDriver.GetType().GetProperty(map.PropertyName);
                            propertyInfo?.SetValue(excelDriver, typedValue, null);
                        }

                    excelDrivers.Add(excelDriver);
                    row++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                exceWorkbook.Close();
                Marshal.ReleaseComObject(exceWorkbook);
                Marshal.ReleaseComObject(excelApp);
            }

            return excelDrivers;
        }

        public List<VehicleDTO> AddVehicles(Stream fileStream)
        {
            var excelVehicles = new List<VehicleDTO>();
            var excelApp = new Application();

            var exceWorkbook = excelApp.Workbooks.Open(GetFilename(fileStream));
            var worksheet = (Worksheet) exceWorkbook.Worksheets[1];
            try
            {
                GetColumns(worksheet);
                var vinColumnIndex = HeaderMap.Single(hm => hm.Header == "VIN").ColumnIndex;
                var row = 2; // Row 1 are headers

                while (!string.IsNullOrEmpty(((ExcelRange) worksheet.Cells[row, vinColumnIndex]).Value?.ToString()))
                {
                    var excelVehicle = new VehicleDTO();
                    foreach (var map in HeaderMap)
                        if (map.ColumnIndex > 0)
                        {
                            var excelValue = ((ExcelRange) worksheet.Cells[row, map.ColumnIndex]).Value;
                            excelValue = TryBoolean(excelValue, map.PropertyType);
                            var typedValue = excelValue == null ? null : Convert.ChangeType(excelValue, map.PropertyType);
                            var propertyInfo = excelVehicle.GetType().GetProperty(map.PropertyName);
                            propertyInfo?.SetValue(excelVehicle, typedValue, null);
                        }

                    excelVehicles.Add(excelVehicle);
                    row++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                exceWorkbook.Close();
                Marshal.ReleaseComObject(exceWorkbook);
                Marshal.ReleaseComObject(excelApp);
            }

            return excelVehicles;
        }

        private static string GetFilename(Stream fileStream)
        {
            const string filename = "Vehicles-Upload-Template.xlsx";
            using var outputFileStream = new FileStream(filename, FileMode.Create);
            fileStream.CopyTo(outputFileStream);
            return filename;
        }

        private void GetColumns(_Worksheet worksheet)
        {
            var i = 1;
            while (!string.IsNullOrEmpty(((ExcelRange) worksheet.Cells[1, i]).Value))
            {
                var headerValue = ((ExcelRange) worksheet.Cells[1, i]).Value;
                var header = headerValue.ToString().Trim();
                var map = HeaderMap.FirstOrDefault(hm => hm.Header.Equals(header));
                if (map != null) map.ColumnIndex = i;
                i++;
            }
        }

        private static dynamic TryBoolean(dynamic value, Type propertyType)
        {
            if (propertyType != typeof(bool)) return value;
            return value == "Yes";
        }
    }
}