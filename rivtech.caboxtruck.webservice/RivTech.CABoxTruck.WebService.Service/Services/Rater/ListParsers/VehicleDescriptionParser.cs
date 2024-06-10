using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class VehicleDescriptionParser : IListParser
    {
        readonly List<dynamic> items = new List<dynamic>
        {
            new { Id = 38, Description = "Delivery Van>10k GVW" },
            new { Id = 16, Description = "Dry Freight Trailer" },
            new { Id = 21, Description = "Dump Body Trailer" },
            new { Id = 2, Description = "Dump Truck 0-16k GVW" },
            new { Id = 66, Description = "Dump Truck 33-45k GVW" },
            new { Id = 65, Description = "Dump Truck >45k GVW" },
            new { Id = 8, Description = "Full size Van" },
            new { Id = 60, Description = "Garbage Truck 0-45 GVW" },
            new { Id = 68, Description = "Garbage Truck >45 GVW" },
            new { Id = 5, Description = "Pickup<= 1/2 Ton 4x2" },
            new { Id = 6, Description = "Pickup<= 1/2 Ton 4x4" },
            new { Id = 7, Description = "Pickup >  1/2 Ton 4x4" },
            new { Id = 34, Description = "Private Passenger Auto" },
            new { Id = 22, Description = "Refrigerated Dry Freight Trailer" },
            new { Id = 44, Description = "Refrigerated Truck 0-16k GVW" },
            new { Id = 45, Description = "Refrigerated Truck 16-26k GVW" },
            new { Id = 46, Description = "Refrigerated Truck >26k GVW" },
            new { Id = 37, Description = "Step Van 0-10k GVW" },
            new { Id = 41, Description = " Straight Truck 0-16k GVW" },
            new { Id = 42, Description = " Straight Truck 16-26k GVW" },
            new { Id = 43, Description = " Straight Truck >26K GVW" },
            new { Id = 17, Description = "Tank Trailer" },
            new { Id = 1, Description = "Tractor" },
            new { Id = 141, Description = "Agricultural Hopper Truck" },
            new { Id = 102, Description = "Auto Hauling Trailer" },
            new { Id = 131, Description = "Bucket Truck/Cherry Picker >26K GVW" },
            new { Id = 129, Description = "Bucket Truck/Cherry Picker 0-16k GVW" },
            new { Id = 130, Description = "Bucket Truck/Cherry Picker 16-26k GVW" },
            new { Id = 98, Description = "Bulk Commodity Trailer" },
            new { Id = 138, Description = "Bus 16-60 passengers" },
            new { Id = 139, Description = "Bus 61+ passengers" },
            new { Id = 137, Description = "Bus 9-15 passengers" },
            new { Id = 140, Description = "Car Carrier >16k GVW" },
            new { Id = 133, Description = "Car Carrier 0-16 GVW" },
            new { Id = 117, Description = "Catering Truck" },
            new { Id = 113, Description = "Cement Mixer" },
            new { Id = 111, Description = "Concession Trailer" },
            new { Id = 120, Description = "Dump Truck 16-33k GVW" },
            new { Id = 92, Description = "Flatbed Trailer" },
            new { Id = 114, Description = "Flatbed Truck >26k GVW" },
            new { Id = 83, Description = "Flatbed Truck 0-16 GVW" },
            new { Id = 121, Description = "Flatbed Truck 16-26k GVW" },
            new { Id = 127, Description = "Front Loader >45k GVW" },
            new { Id = 122, Description = "Front Loader 0-45k" },
            new { Id = 132, Description = "Gooseneck Trailer" },
            new { Id = 109, Description = "Hearse" },
            new { Id = 95, Description = "Horse Trailer(1-2 stalls)" },
            new { Id = 110, Description = "Ice Cream Truck" },
            new { Id = 108, Description = "Limousine" },
            new { Id = 94, Description = "Livestock Trailer" },
            new { Id = 126, Description = "Logging Trailer" },
            new { Id = 96, Description = "Lowboy Trailer" },
            new { Id = 105, Description = "Luxury Auto" },
            new { Id = 91, Description = "Mini Van" },
            new { Id = 106, Description = "Motor Home" },
            new { Id = 116, Description = "Pickup >  1/2 Ton 4x2" },
            new { Id = 119, Description = "Pickup w/ 5th Wheel 0-16k GVW" },
            new { Id = 93, Description = "Pole Trailer" },
            new { Id = 136, Description = "Pump Truck(Cement) >26k GVW" },
            new { Id = 134, Description = "Pump Truck(Cement) 0-16k GVW" },
            new { Id = 135, Description = "Pump Truck(Cement) 16-26k GVW" },
            new { Id = 100, Description = "Rag Top Trailer" },
            new { Id = 128, Description = "Roll On Vehicle >45k GVW" },
            new { Id = 123, Description = "Roll On Vehicle 0-45 GVW" },
            new { Id = 103, Description = "Sport Auto" },
            new { Id = 118, Description = "Stake Truck >26k GVW" },
            new { Id = 89, Description = "Stake Truck 0-16k GVW" },
            new { Id = 124, Description = "Stake Truck 16-26k GVW" },
            new { Id = 90, Description = "Tank Truck <= 1400 gals" },
            new { Id = 125, Description = "Tank Truck > 1400 gals" },
            new { Id = 99, Description = "Tilt Trailer" },
            new { Id = 84, Description = "Tow Truck - one axle" },
            new { Id = 112, Description = "Travel Trailer" },
            new { Id = 115, Description = "TW Truck-two axle" },
            new { Id = 97, Description = "Unidentified Trailer" },
            new { Id = 101, Description = "Utility Trailer <= 1 ft." },
            new { Id = 104, Description = "Utility Trailer > 12 ft" },
            new { Id = 107, Description = "Utility Vehicle" },
            new { Id = 86, Description = "Wheelchair Bus" },
            new { Id = 85, Description = "Wheelchair Van" },
            new { Id = 142, Description = "Boom Truck/Crane Truck >26k GVW" },
            new { Id = 143, Description = "Boom Truck/Crane Truck 0-16k GVW" },
            new { Id = 144, Description = "Boom Truck/Crane Truck 16-26k GVW" },
            new { Id = 145, Description = "Full size Van (0-8 passengers)" },
            new { Id = 146, Description = "Limousine <120 inches in Length" },
            new { Id = 147, Description = "Limousine 121-180 inches in Length" },
            new { Id = 148, Description = "Limousine >180 inches in length" },
            new { Id = 149, Description = "Luxury Utility Vehicle" },
            new { Id = 150, Description = "Tow Truck - one axle 0-26k GVW" },
            new { Id = 151, Description = "Tow Truck - one axle >26k GVW" },
            new { Id = 152, Description = "Utility Vehicle (except Luxury)" },
            new { Id = 153, Description = "Wedge Trailer" },
            new { Id = 154, Description = "Wheelchair Bus (16-60 passengers)" },
            new { Id = 155, Description = "Wheelchair Bus (61+ passengers)" },
            new { Id = 156, Description = "Wheelchair Van (0-8 passengers)" },
            new { Id = 157, Description = "Wheelchair Van (9-15 passengers)" },
        };

        public Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            var item = items.FirstOrDefault(x => x.Id == (short)id);
            string description = item?.Description ?? "";
            bool isRaterVersionSupported = (raterVersion ?? 7) >= 7 || raterVersion == 0;

            if (isRaterVersionSupported) {
                return Task.FromResult(description.Trim());
            } else {
                return Task.FromResult(item?.Description ?? "");
            }
            
        }
    }
}
