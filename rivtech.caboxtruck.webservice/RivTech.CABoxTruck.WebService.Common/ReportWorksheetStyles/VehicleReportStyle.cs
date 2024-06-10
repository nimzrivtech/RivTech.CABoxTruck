using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
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
    public static class VehicleReportStyle
    {
        public static void ReportWorksheetStyle(IXLWorksheet sheet, XLWorkbook workbook, bool? isRaterV7 = null)
        {
            //Header styles
            IXLRanges rangeReportHeader = sheet.Ranges("A1:Q1");
            rangeReportHeader.Style.Font.Bold = true;
            rangeReportHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            //Sheet Columns
            sheet.Column("A").Width = 20;
            sheet.Column("B").Width = 5;
            sheet.Column("C").Width = 7;
            sheet.Column("D").Width = 9;
            sheet.Column("E").Width = 9;
            sheet.Column("F").Width = 20;
            sheet.Column("G").Width = 20;
            sheet.Column("H").Width = 35;
            sheet.Column("I").Width = 25;
            sheet.Column("J").Width = 25;
            sheet.Column("K").Width = 15;
            sheet.Column("L").Width = 40;
            sheet.Column("M").Width = 20;
            sheet.Column("N").Width = 40;
            sheet.Column("O").Width = 20;
            sheet.Column("P").Width = 20;
            sheet.Column("Q").Width = 25;

            //Create data set
            //col1 is from the Data sheet col2 is from the main sheet (Vehicles)
            var dataSetSheet = workbook.Worksheets.Add("Data");
            CreateVehicleTypesDataSet(sheet, dataSetSheet, "B", "G");

            //added raterVersion for CreateVehicleDescriptionDataSet
            if (isRaterV7 == true) CreateVehicleDescriptionDataSetV7(sheet, dataSetSheet, "D", "H");
            else CreateVehicleDescriptionDataSet(sheet, dataSetSheet, "D", "H");

            CreateVehicleBusinessDataSet(sheet, dataSetSheet, "F", "I");
            CreateVehicleWeightDataSet(sheet, dataSetSheet, "H", "J");
            CreateRadiusDataSet(sheet, dataSetSheet, "J", "K");
            CreateDEDDataSet(sheet, dataSetSheet, "P", "N");
            CreateDEDDataSet(sheet, dataSetSheet, "R", "O");

            //Match data format to avoid validation error when editing cell
            MatchDataFormat(sheet, dataSetSheet);

            CreateYesNoDataSet(sheet, dataSetSheet, new Dictionary<string, string>
            {
                { "L", "L" },
                { "N", "M" },
                { "T", "P" },
                { "V", "Q" }
            });

            dataSetSheet.Cell("B2").Value = "Vehicle Type";
            dataSetSheet.Cell("D2").Value = "Vehicle Description";
            dataSetSheet.Cell("F2").Value = "Vehicle Business Class";
            dataSetSheet.Cell("H2").Value = "Gross Vehicle Weight (lbs)";

            dataSetSheet.Cell("J2").Value = "Vehicle Radius";
            dataSetSheet.Cell("L2").Value = "Comprehensive / Fire & Theft (Yes or No)";
            dataSetSheet.Cell("N2").Value = "Collision (Yes or No)";
            dataSetSheet.Cell("P2").Value = "Comprehensive / Fire & Theft Deductible";
            dataSetSheet.Cell("R2").Value = "Collision Deductible";
            dataSetSheet.Cell("T2").Value = "Cargo (Yes or No)";
            dataSetSheet.Cell("V2").Value = "Refrigeration (Yes or No)";

            dataSetSheet.Column("B").Width = 20;
            dataSetSheet.Column("D").Width = 42;
            dataSetSheet.Column("F").Width = 65;
            dataSetSheet.Column("H").Width = 25;

            dataSetSheet.Column("J").Width = 15;
            dataSetSheet.Column("L").Width = 40;
            dataSetSheet.Column("N").Width = 20;
            dataSetSheet.Column("P").Width = 40;
            dataSetSheet.Column("R").Width = 20;
            dataSetSheet.Column("T").Width = 17;
            dataSetSheet.Column("V").Width = 25;
        }

        private static void CreateVehicleTypesDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, VehicleTypes);
        }

        private static void CreateVehicleDescriptionDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, VehicleDescriptions);
        }

        private static void CreateVehicleDescriptionDataSetV7(IXLWorksheet mainSheet,
           IXLWorksheet dataSetSheet,
           string col1,
           string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, VehicleDescriptionsV7);
        }
        private static void CreateVehicleBusinessDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, VehicleBusinesses);
        }

        private static void CreateVehicleWeightDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, VehicleWeights);
        }
        private static void CreateRadiusDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, VehicleRadius);
        }

        private static void CreateYesNoDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet, Dictionary<string, string> colSets)
        {
            foreach (var colSet in colSets)
                CreateDataSetAndBind(mainSheet, dataSetSheet, colSet.Key, colSet.Value, YesNo);
        }

        private static void CreateDEDDataSet(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2)
        {
            CreateDataSetAndBind(mainSheet, dataSetSheet, col1, col2, DED);
        }

        private static void CreateDataSetAndBind(IXLWorksheet mainSheet,
            IXLWorksheet dataSetSheet,
            string col1,
            string col2,
            string[] types)
        {
            var validationCell = dataSetSheet.Cell($"{col1}4");
            validationCell.InsertData(types);

            IXLRanges cellRange = dataSetSheet.Ranges($"{col1}4:{col1}" + (types.Length + 3));
            cellRange.Style.Border.SetBorders(XLBorderStyleValues.Thin);

            var col = mainSheet.Column(col2);
            var validation = col.SetDataValidation();
            var dataSetRange = dataSetSheet.Range($"${col1}$4:${col1}$" + (types.Length + 3));
            validation.List(dataSetRange);
        }

        private static void MatchDataFormat(IXLWorksheet mainSheet, IXLWorksheet dataSheet)
        {
            //Vehicle Radius
            var vehicleRadiusColumn = mainSheet.Column("K");
            vehicleRadiusColumn.Style.NumberFormat.Format = "@";

            var vehicleRadiusColumnDataSetRange = dataSheet.Column($"J");
            vehicleRadiusColumnDataSetRange.Style.NumberFormat.Format = "@";

            //Comprehensive F/T DED
            var compreDEDColumn = mainSheet.Column("N");
            compreDEDColumn.Style.NumberFormat.Format = "@";

            var compreFTDED = dataSheet.Column($"P");
            compreFTDED.Style.NumberFormat.Format = "@";

            //Collision DED
            var colliDEDColumn = mainSheet.Column("O");
            colliDEDColumn.Style.NumberFormat.Format = "@";

            var collisionDED = dataSheet.Column($"R");
            collisionDED.Style.NumberFormat.Format = "@";
        }

        private static void SetBorders(this IXLBorder border, XLBorderStyleValues value)
        {
            border.LeftBorder = value;
            border.RightBorder = value;
            border.TopBorder = value;
            border.BottomBorder = value;
        }

        #region Types
        private static readonly string[] VehicleTypes =
        {
            "Other",
            "Power Unit",
            "Trailer"
        };

        private static readonly string[] VehicleDescriptions =
        {
            "Agricultural Hopper Truck",
            "Auto Hauling Trailer",
            "Bucket Truck/Cherry Picker 0-16k GVW",
            "Bucket Truck/Cherry Picker 16-26k GVW",
            "Bucket Truck/Cherry Picker >26K GVW",
            "Bulk Commodity Trailer",
            "Bus 9-15 passengers",
            "Bus 16-60 passengers",
            "Bus 61+ passengers",
            "Car Carrier 0-16 GVW",
            "Car Carrier >16k GVW",
            "Catering Truck",
            "Cement Mixer",
            "Concession Trailer",
            "Delivery Van>10k GVW",
            "Dry Freight Trailer",
            "Dump Body Trailer",
            "Dump Truck 0-16k GVW",
            "Dump Truck 16-33k GVW",
            "Dump Truck 33-45k GVW",
            "Dump Truck >45k GVW",
            "Flatbed Trailer",
            "Flatbed Truck 0-16 GVW",
            "Flatbed Truck 16-26k GVW",
            "Flatbed Truck >26k GVW",
            "Front Loader 0-45k",
            "Front Loader >45k GVW",
            "Full size Van",
            "Garbage Truck 0-45 GVW",
            "Garbage Truck >45 GVW",
            "Gooseneck Trailer",
            "Hearse",
            "Horse Trailer(1-2 stalls)",
            "Ice Cream Truck",
            "Limousine",
            "Livestock Trailer",
            "Logging Trailer",
            "Lowboy Trailer",
            "Luxury Auto",
            "Mini Van",
            "Motor Home",
            "Pickup<= 1/2 Ton 4x2",
            "Pickup<= 1/2 Ton 4x4",
            "Pickup >  1/2 Ton 4x2",
            "Pickup >  1/2 Ton 4x4",
            "Pickup w/ 5th Wheel 0-16k GVW",
            "Pole Trailer",
            "Private Passenger Auto",
            "Pump Truck(Cement) 0-16k GVW",
            "Pump Truck(Cement) 16-26k GVW",
            "Pump Truck(Cement) >26k GVW",
            "Rag Top Trailer",
            "Refrigerated Dry Freight Trailer",
            "Refrigerated Truck 0-16k GVW",
            "Refrigerated Truck 16-26k GVW",
            "Refrigerated Truck >26k GVW",
            "Roll On Vehicle 0-45 GVW",
            "Roll On Vehicle >45k GVW",
            "Sport Auto",
            "Stake Truck 0-16k GVW",
            "Stake Truck 16-26k GVW",
            "Stake Truck >26k GVW",
            "Step Van 0-10k GVW",
            "Straight Truck 0-16k GVW",
            "Straight Truck 16-26k GVW",
            "Straight Truck >26K GVW",
            "Tank Trailer",
            "Tank Truck <= 1400 gals",
            "Tank Truck > 1400 gals",
            "Tilt Trailer",
            "Tow Truck - one axle",
            "Tractor",
            "Travel Trailer",
            "TW Truck-two axle",
            "Unidentified Trailer",
            "Utility Trailer <= 1 ft.",
            "Utility Trailer > 12 ft",
            "Utility Vehicle",
            "Wheelchair Bus",
            "Wheelchair Van",
        };
        private static readonly string[] VehicleDescriptionsV7 =
        {
            "Agricultural Hopper Truck",
            "Auto Hauling Trailer",
            "Boom Truck/Crane Truck >26k GVW",
            "Boom Truck/Crane Truck 0-16k GVW",
            "Boom Truck/Crane Truck 16-26k GVW",
            "Bucket Truck/Cherry Picker 0-16k GVW",
            "Bucket Truck/Cherry Picker 16-26k GVW",
            "Bucket Truck/Cherry Picker >26K GVW",
            "Bulk Commodity Trailer",
            "Bus 9-15 passengers",
            "Bus 16-60 passengers",
            "Bus 61+ passengers",
            "Car Carrier 0-16 GVW",
            "Car Carrier >16k GVW",
            "Catering Truck",
            "Cement Mixer",
            "Concession Trailer",
            "Delivery Van>10k GVW",
            "Dry Freight Trailer",
            "Dump Body Trailer",
            "Dump Truck 0-16k GVW",
            "Dump Truck 16-33k GVW",
            "Dump Truck 33-45k GVW",
            "Dump Truck >45k GVW",
            "Flatbed Trailer",
            "Flatbed Truck 0-16 GVW",
            "Flatbed Truck 16-26k GVW",
            "Flatbed Truck >26k GVW",
            "Front Loader 0-45k",
            "Front Loader >45k GVW",
            "Full size Van (0-8 passengers)",
            "Garbage Truck 0-45 GVW",
            "Garbage Truck >45 GVW",
            "Gooseneck Trailer",
            "Hearse",
            "Horse Trailer(1-2 stalls)",
            "Ice Cream Truck",
            "Limousine <120 inches in Length",
            "Limousine 121-180 inches in Length",
            "Limousine >180 inches in length",
            "Livestock Trailer",
            "Logging Trailer",
            "Lowboy Trailer",
            "Luxury Auto",
            "Luxury Utility Vehicle",
            "Mini Van",
            "Motor Home",
            "Pickup<= 1/2 Ton 4x2",
            "Pickup<= 1/2 Ton 4x4",
            "Pickup >  1/2 Ton 4x2",
            "Pickup >  1/2 Ton 4x4",
            "Pickup w/ 5th Wheel 0-16k GVW",
            "Pole Trailer",
            "Private Passenger Auto",
            "Pump Truck(Cement) 0-16k GVW",
            "Pump Truck(Cement) 16-26k GVW",
            "Pump Truck(Cement) >26k GVW",
            "Rag Top Trailer",
            "Refrigerated Dry Freight Trailer",
            "Refrigerated Truck 0-16k GVW",
            "Refrigerated Truck 16-26k GVW",
            "Refrigerated Truck >26k GVW",
            "Roll On Vehicle 0-45 GVW",
            "Roll On Vehicle >45k GVW",
            "Sport Auto",
            "Stake Truck 0-16k GVW",
            "Stake Truck 16-26k GVW",
            "Stake Truck >26k GVW",
            "Step Van 0-10k GVW",
            " Straight Truck 0-16k GVW",
            " Straight Truck 16-26k GVW",
            " Straight Truck >26K GVW",
            "Tank Trailer",
            "Tank Truck <= 1400 gals",
            "Tank Truck > 1400 gals",
            "Tilt Trailer",
            "Tow Truck - one axle",
            "Tow Truck - one axle 0-26k GVW",
            "Tow Truck - one axle >26k GVW",
            "Tractor",
            "Travel Trailer",
            "TW Truck-two axle",
            "Unidentified Trailer",
            "Utility Trailer <= 1 ft.",
            "Utility Trailer > 12 ft",
            "Utility Vehicle (except Luxury)",
            "Wedge Trailer",
            "Wheelchair Bus (16-60 passengers)",
            "Wheelchair Bus (61+ passengers)",
            "Wheelchair Van (0-8 passengers)",
            "Wheelchair Van (9-15 passengers)"
        };

        private static readonly string[] VehicleBusinesses =
        {
            "Agricultural Hauling",
            "Airport Taxi Services",
            "Amish Taxis",
            "Auto Dealers",
            "Auto Haulers",
            "Auto Repair & Maintenance",
            "Auto Salvage Haulers",
            "Black Car Services",
            "Body Shops",
            "Building Materials & Hardware Stores",
            "Carpentry",
            "Catering Business",
            "Coal Hauling",
            "Concrete & Asphalt Work",
            "Consulting, Legal & Engineering",
            "Courier",
            "Day Care (Child & Adult Services)",
            "Debris Removal",
            "Dirt, Sand & Gravel",
            "Disinfecting & Pest Control Services",
            "Door to Door Sales",
            "Driving Schools",
            "Dry Cleaning, Laundry & Carpet Cleaners",
            "Drywall, Plastering & Insulation Work",
            "Electrical Contractor",
            "Entertainers & Musicians",
            "Equipment Rental & Leasing",
            "Escort Vehicles",
            "Excavation Work",
            "Expediters and Hotshotters",
            "Farming & Livestock",
            "Flooring",
            "Florist",
            "Food & Grocery Stores",
            "Freight Forwarders",
            "Funeral Services",
            "Furniture & Home Furnishing Stores",
            "Garbage & Trash",
            "Gas & Service Stations",
            "Glass & Glazing Work",
            "Hazardous Materials",
            "Heating & Air Conditioning",
            "Heavy Construction - Road & Utilities",
            "Hotel Shuttle Services",
            "Household Movers",
            "Ice Cream Vendors",
            "Janitorial & Building Maintenance Services",
            "Landscaping, snowplowing & Firewood",
            "Limousine Services",
            "Livestock Hauling",
            "Logging & Log Transport",
            "Machinery & Heavy Equipment",
            "Manufacturing",
            "Masonry, Plastering/Drywall, Carpenters, Floorwork (Renewals Only)",
            "Masons",
            "Meals on Wheels",
            "Mobile Home Toters",
            "Newspaper & Mail Delivery",
            "Non Business",
            "Non-Emergency Medical Transportation",
            "Non-Residential & Industrial Builders",
            "Other For-Hire Trucking Operations",
            "Other Passenger Transportation (Not for Hire)",
            "Other Retail Trade Operations",
            "Other Trade Contractors",
            "Out of State Contractors",
            "Painting & Wallpaper Hanging",
            "Personal Care - Beauty & Barber Shops",
            "Plumbing",
            "Prepared Food & Lunch Truck",
            "Real Estate & Insurance",
            "Refrigerated Goods",
            "Religious Organizations",
            "Repair Services",
            "Residential Builders",
            "Restaurant, Pizza & Fast Food Delivery",
            "Retail Operations (Renewals Only)",
            "Roofing, Siding & Sheet Metal Work",
            "Scrap Metal, Scrap Auto & Recycling Services",
            "Security Guard & Private Investigation",
            "Septic Waste Removal",
            "Services Not Otherwise Classified",
            "Social & Health Services",
            "Steel Hauling",
            "Taxi Services",
            "Towing & Service Stations (Renewals Only)",
            "Towing Services",
            "Visiting Nurse",
            "Water Well Drilling Contractors",
            "Wholesale Manufacturing (Renewals Only)",
            "Wholesale Trade"
        };

        private static readonly string[] VehicleWeights =
        {
            "0 - 10,000",
            "10,001 - 20,000",
            "20,001 - 45,000",
            "45,001 +"
        };

        private static readonly string[] VehicleRadius =
        {
            "50",
            "100",
            "200",
            "300",
            "500",
            "999"
        };

        private static readonly string[] YesNo =
        {
            "Yes",
            "No"
        };

        private static readonly string[] DED =
        {
            "No Coverage",
            "$100 DED",
            "$150 DED",
            "$250 DED",
            "$500 DED",
            "$750 DED",
            "$1,000 DED",
            "$1,500 DED",
            "$2,000 DED",
            "$2,500 DED",
            "$5,000 DED"
        };
        #endregion
    }
}
