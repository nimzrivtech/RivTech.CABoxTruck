using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsertSecondBusinessGroupInLvDatasetTable : Migration
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
          Insert into [LvDataset] values(1,'All Other','All Other', 1, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(2,'Armored Cars','Armored Cars', 2, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(3,'Automobile Dismantlers','Automobile Dismantlers', 3, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(4,'Automobile Used to transport Liquefied Petroleum Gases (Texas only)','Automobile Used to transport Liquefied Petroleum Gases (Texas only)', 4, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(5,'Automobiles Hauling Explosives (not otherwise specified - Texas only)','Automobiles Hauling Explosives (not otherwise specified - Texas only)', 5, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(6,'Building - Commercial','Building - Commercial', 6, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(7,'Building - Private Dwellings','Building - Private Dwellings', 7, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(8,'Extra Heavy Truck Tractors','Extra Heavy Truck Tractors', 8, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(9,'Canneries and Packing Plants','Canneries and Packing Plants', 9, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(10,'Carriers Engaged in Both Private Carriage and Transport for Others if at least 20% of Operation is Transport for Others','Carriers Engaged in Both Private Carriage and Transport for Others if at least 20% of Operation is Transport for Others', 10, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(11,'Common Carrier - Irregular Route Common Carriers and Specialized Carriers (Texas Only) (28)','Common Carrier - Irregular Route Common Carriers and Specialized Carriers (Texas Only) (28)', 11, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(12,'Common Carriers','Common Carriers', 12, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(13,'Contract Carriers (Other than Chemical or Iron and Steel Haulers)','Contract Carriers (Other than Chemical or Iron and Steel Haulers)', 13, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(14,'Contract Carriers Hauling Chemicals','Contract Carriers Hauling Chemicals', 14, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(15,'Contract Carriers Hauling Iron and Steel','Contract Carriers Hauling Iron and Steel', 15, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(16,'Contractors Automobiles Hauling Explosives (Texas Only) (86)','Contractors Automobiles Hauling Explosives (Texas Only) (86)', 16, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(17,'Electrical, Plumbing, Masonry, Plastering and Other Repair or Service','Electrical, Plumbing, Masonry, Plastering and Other Repair or Service', 17, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(18,'Excavating','Excavating', 18, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(19,'Exempt Carriers (Other than Livestock Haulers)','Exempt Carriers (Other than Livestock Haulers)', 19, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(20,'Exempt Carriers Hauling Livestock','Exempt Carriers Hauling Livestock', 20, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(21,'Film Delivery','Film Delivery', 21, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(22,'Fish and Seafood','Fish and Seafood', 22, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(23,'Frozen Food','Frozen Food', 23, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(24,'Fruit and Vegetable','Fruit and Vegetable', 24, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(25,'Garbage (Garbage and Ash Removal)','Garbage (Garbage and Ash Removal)', 25, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(26,'Individually Owned or Family Corporation (Other than Livestock Hauling)','Individually Owned or Family Corporation (Other than Livestock Hauling)', 26, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(27,'Junk Dealers','Junk Dealers', 27, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(28,'Livestock Hauling','Livestock Hauling', 28, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(29,'Logging and Lumbering','Logging and Lumbering', 29, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(30,'Machinery','Machinery', 30, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(31,'Magazine or Newspapers','Magazine or Newspapers', 31, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(32,'Mail and Parcel Post','Mail and Parcel Post', 32, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(33,'Meat and Poultry','Meat and Poultry', 33, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(34,'Metal','Metal', 34, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(35,'Mining','Mining', 35, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(36,'Quarrying','Quarrying', 36, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(37,'Sand and Gravel (Other than Quarrying)','Sand and Gravel (Other than Quarrying)', 37, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(38,'Street and Road','Street and Road', 38, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(39,'Structural Iron or Steel','Structural Iron or Steel', 39, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(40,'Tow Trucks for Hire (New York Only with Towed Vehicle PD Liability > $25,000)','Tow Trucks for Hire (New York Only with Towed Vehicle PD Liability > $25,000)', 40, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(41,'Tow Trucks for Hire (Other than New York) or Tow Trucks for Hire (New York Only with Towed Vehicle PD Liability <= $25,000)','Tow Trucks for Hire (Other than New York) or Tow Trucks for Hire (New York Only with Towed Vehicle PD Liability <= $25,000)', 41, '7', 'SecondaryBusGroup', 1)
          Insert into [LvDataset] values(42,'Tow Trucks For Hire All Other','Tow Trucks For Hire All Other', 42, '7', 'SecondaryBusGroup', 1)
          GO ";

        const string SQL_DELETE = @"
          DELETE FROM LvDataset WHERE DatasetType = 'SecondaryBusGroup';
          GO ";
    }
}
