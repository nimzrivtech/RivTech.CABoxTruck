using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsertDataInLvDatasetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

          const string SQL_INSERT = @"
          Insert into [LvDataset] values(1,'Light','Light', 1, '1,2,3,4,5,6', 'MainGWV', 1)
          Insert into [LvDataset] values(2,'Medium','Medium', 2, '1,2,3,4,5,6', 'MainGWV', 1)
          Insert into [LvDataset] values(3,'Heavy','Heavy', 3, '1,2,3,4,5,6', 'MainGWV', 1)
          Insert into [LvDataset] values(4,'Extra Heavy','Extra Heavy', 4, '1,2,3,4,5,6', 'MainGWV', 1)

          Insert into [LvDataset] values(5,'Light Trucks','Light Trucks', 5, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(6,'Medium Trucks','Medium Trucks', 6, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(7,'Heavy Trucks','Heavy Trucks', 4, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(8,'Extra Heavy Truck Tractors','Extra Heavy Truck Tractors', 1, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(9,'Not a Truck/Tractor','Not a Truck/Tractor', 7, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(10,'Extra Heavy Trucks','Extra Heavy Trucks', 2, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(11,'Heavy Truck Tractors','Heavy Truck Tractors', 3, '7', 'MainGWV', 1)
          Insert into [LvDataset] values(12,'Trailer Types','Trailer Types', 8, '7', 'MainGWV', 1)

          Insert into [LvDataset] values(1,'Light','Light', 1, '1,2,3,4,5,6', 'VehicleGVW', 1)
          Insert into [LvDataset] values(2,'Medium','Medium', 2, '1,2,3,4,5,6', 'VehicleGVW', 1)
          Insert into [LvDataset] values(3,'Heavy','Heavy', 3, '1,2,3,4,5,6', 'VehicleGVW', 1)
          Insert into [LvDataset] values(4,'Extra Heavy (including tractors)','Extra Heavy (including tractors)', 4, '1,2,3,4,5,6', 'VehicleGVW', 1)

          Insert into [LvDataset] values(5,'Light Trucks','Light Trucks', 1, '7', 'VehicleGVW', 1)
          Insert into [LvDataset] values(6,'Medium Trucks','Medium Trucks', 2, '7', 'VehicleGVW', 1)
          Insert into [LvDataset] values(7,'Heavy Trucks','Heavy Trucks', 3, '7', 'VehicleGVW', 1)
          Insert into [LvDataset] values(8,'Extra Heavy Truck Tractors','Extra Heavy Truck Tractors', 4, '7', 'VehicleGVW', 1)

          Insert into [LvDataset] values(141,'Agricultural Hopper Truck','Agricultural Hopper Truck', 1, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(102,'Auto Hauling Trailer','Auto Hauling Trailer', 2, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(142,'Boom Truck/Crane Truck >26k GVW','Boom Truck/Crane Truck >26k GVW', 3, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(143,'Boom Truck/Crane Truck 0-16k GVW','Boom Truck/Crane Truck 0-16k GVW', 4, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(144,'Boom Truck/Crane Truck 16-26k GVW','Boom Truck/Crane Truck 16-26k GVW', 5, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(129,'Bucket Truck/Cherry Picker 0-16k GVW','Bucket Truck/Cherry Picker 0-16k GVW', 6, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(130,'Bucket Truck/Cherry Picker 16-26k GVW','Bucket Truck/Cherry Picker 16-26k GVW', 7, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(131,'Bucket Truck/Cherry Picker >26K GVW','Bucket Truck/Cherry Picker >26K GVW', 8, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(98,'Bulk Commodity Trailer','Bulk Commodity Trailer', 9, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(137,'Bus 9-15 passengers','Bus 9-15 passengers', 10, '1,2,3,4,5,6,7', 'VehicleDesc', 1)

          Insert into [LvDataset] values(138,'Bus 16-60 passengers','Bus 16-60 passengers', 11, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(139,'Bus 61+ passengers','Bus 61+ passengers',12, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(133,'Car Carrier 0-16 GVW','Car Carrier 0-16 GVW', 13, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(140,'Car Carrier >16k GVW','Car Carrier >16k GVW', 14, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(117,'Catering Truck','Catering Truck', 15, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(113,'Cement Mixer','Cement Mixer', 16, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(111,'Concession Trailer','Concession Trailer', 17, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(38,'Delivery Van>10k GVW','Delivery Van>10k GVW', 18, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(16,'Dry Freight Trailer','Dry Freight Trailer', 19, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(21,'Dump Body Trailer','Dump Body Trailer', 20, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(2,'Dump Truck 0-16k GVW','Dump Truck 0-16k GVW', 21, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(120,'Dump Truck 16-33k GVW','Dump Truck 16-33k GVW', 22, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(66,'Dump Truck 33-45k GVW','Dump Truck 33-45k GVW', 23, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(65,'Dump Truck >45k GVW','Dump Truck >45k GVW', 24, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(92,'Flatbed Trailer','Flatbed Trailer', 25, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(83,'Flatbed Truck 0-16 GVW','Flatbed Truck 0-16 GVW', 26, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(121,'Flatbed Truck 16-26k GVW','Flatbed Truck 16-26k GVW', 27, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(114,'Flatbed Truck >26k GVW','Flatbed Truck >26k GVW', 28, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(122,'Front Loader 0-45k','Front Loader 0-45k', 29, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(127,'Front Loader >45k GVW','Front Loader >45k GVW', 30, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(8,'Full size Van','Full size Van', 31, '1,2,3,4,5,6', 'VehicleDesc', 1)
          Insert into [LvDataset] values(145,'Full size Van (0-8 passengers)','Full size Van (0-8 passengers)', 32, '7', 'VehicleDesc', 1)

          Insert into [LvDataset] values(60,'Garbage Truck 0-45 GVW','Garbage Truck 0-45 GVW', 33, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(68,'Garbage Truck >45 GVW','Garbage Truck >45 GVW', 34, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(132,'Gooseneck Trailer','Gooseneck Trailer', 35, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(109,'Hearse','Hearse', 36, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(95,'Horse Trailer(1-2 stalls)','Horse Trailer(1-2 stalls)', 37, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(110,'Ice Cream Truck','Ice Cream Truck', 38, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(108,'Limousine','Limousine', 39, '1,2,3,4,5,6', 'VehicleDesc', 1)
          Insert into [LvDataset] values(146,'Limousine <120 inches in Length','Limousine <120 inches in Length', 40, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(147,'Limousine 121-180 inches in Length','Limousine 121-180 inches in Length', 41, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(148,'Limousine >180 inches in length','Limousine >180 inches in length', 42, '7', 'VehicleDesc', 1)

          Insert into [LvDataset] values(94,'Livestock Trailer','Livestock Trailer', 43, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(126,'Logging Trailer','Logging Trailer', 44, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(96,'Lowboy Trailer','Lowboy Trailer', 45, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(105,'Luxury Auto','Luxury Auto', 46, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(149,'Luxury Utility Vehicle','Luxury Utility Vehicle', 47, '7', 'VehicleDesc', 1)

          Insert into [LvDataset] values(91,'Mini Van','Mini Van', 48, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(106,'Motor Home','Motor Home', 49, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(5,'Pickup<= 1/2 Ton 4x2','Pickup<= 1/2 Ton 4x2', 50, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(6,'Pickup<= 1/2 Ton 4x4','Pickup<= 1/2 Ton 4x4', 51, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(116,'Pickup >  1/2 Ton 4x2','Pickup >  1/2 Ton 4x2', 52, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(7,'Pickup >  1/2 Ton 4x4','Pickup >  1/2 Ton 4x4', 53, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(119,'Pickup w/ 5th Wheel 0-16k GVW','Pickup w/ 5th Wheel 0-16k GVW', 54, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(93,'Pole Trailer','Pole Trailer', 55, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(34,'Private Passenger Auto','Private Passenger Auto', 56, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(134,'Pump Truck(Cement) 0-16k GVW','Pump Truck(Cement) 0-16k GVW', 57, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(135,'Pump Truck(Cement) 16-26k GVW','Pump Truck(Cement) 16-26k GVW', 58, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(136,'Pump Truck(Cement) >26k GVW','Pump Truck(Cement) >26k GVW', 59, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(100,'Rag Top Trailer','Rag Top Trailer', 60, '1,2,3,4,5,6,7', 'VehicleDesc', 1)

          Insert into [LvDataset] values(22,'Refrigerated Dry Freight Trailer','Refrigerated Dry Freight Trailer', 61, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(44,'Refrigerated Truck 0-16k GVW','Refrigerated Truck 0-16k GVW', 62, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(45,'Refrigerated Truck 16-26k GVW','Refrigerated Truck 16-26k GVW', 63, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(46,'Refrigerated Truck >26k GVW','Refrigerated Truck >26k GVW', 64, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(123,'Roll On Vehicle 0-45 GVW','Roll On Vehicle 0-45 GVW', 65, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(128,'Roll On Vehicle >45k GVW','Roll On Vehicle >45k GVW', 66, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(103,'Sport Auto','Sport Auto', 67, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(89,'Stake Truck 0-16k GVW','Stake Truck 0-16k GVW', 68, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(124,'Stake Truck 16-26k GVW','Stake Truck 16-26k GVW', 69, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(118,'Stake Truck >26k GVW','Stake Truck >26k GVW', 70, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(37,'Step Van 0-10k GVW','Step Van 0-10k GVW', 71, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(41,'Straight Truck 0-16k GVW','Straight Truck 0-16k GVW', 72, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(42,'Straight Truck 16-26k GVW','Straight Truck 16-26k GVW', 73, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(43,'Straight Truck >26K GVW','Straight Truck >26K GVW', 74, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(17,'Tank Trailer','Tank Trailer', 75, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(90,'Tank Truck <= 1400 gals','Tank Truck <= 1400 gals', 76, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(125,'Tank Truck > 1400 gals','Tank Truck > 1400 gals', 77, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(99,'Tilt Trailer','Tilt Trailer', 78, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(84,'Tow Truck - one axle','Tow Truck - one axle', 79, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(150,'Tow Truck - one axle 0-26k GVW','Tow Truck - one axle 0-26k GVW', 80, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(151,'Tow Truck - one axle >26k GVW','Tow Truck - one axle >26k GVW', 81, '7', 'VehicleDesc', 1)

          Insert into [LvDataset] values(1,'Tractor','Tractor', 82, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(112,'Travel Trailer','Travel Trailer', 83, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(115,'TW Truck-two axle','TW Truck-two axle', 84, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(97,'Unidentified Trailer','Unidentified Trailer', 85, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(101,'Utility Trailer <= 1 ft.','Utility Trailer <= 1 ft.', 86, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(104,'Utility Trailer > 12 ft','Utility Trailer > 12 ft', 87, '1,2,3,4,5,6,7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(107,'Utility Vehicle','Utility Vehicle', 88, '1,2,3,4,5,6', 'VehicleDesc', 1)
          Insert into [LvDataset] values(86,'Wheelchair Bus','Wheelchair Bus', 89, '1,2,3,4,5,6', 'VehicleDesc', 1)
          Insert into [LvDataset] values(85,'Wheelchair Van','Wheelchair Van', 90, '1,2,3,4,5,6', 'VehicleDesc', 1)
         
          Insert into [LvDataset] values(152,'Utility Vehicle (except Luxury)','Utility Vehicle (except Luxury)', 91, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(153,'Wedge Trailer','Wedge Trailer', 92, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(154,'Wheelchair Bus (16-60 passengers)','Wheelchair Bus (16-60 passengers)', 93, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(155,'Wheelchair Bus (61+ passengers)','Wheelchair Bus (61+ passengers)', 94, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(156,'Wheelchair Van (0-8 passengers)','Wheelchair Van (0-8 passengers)', 95, '7', 'VehicleDesc', 1)
          Insert into [LvDataset] values(157,'Wheelchair Van (9-15 passengers)','Wheelchair Van (9-15 passengers)', 96, '7', 'VehicleDesc', 1)

          GO ";

          const string SQL_DELETE = @"
          TRUNCATE TABLE [LvDataset];
          GO ";
    }
}
