using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class SecondaryBusinessGroupParser : IListParser
    {
        public Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            string text = (short)id switch
            {
                1 => "All Other",
                2 => "Armored Cars",
                3 => "Automobile Dismantlers",
                4 => "Automobile Used to transport Liquefied Petroleum Gases (Texas only)",
                5 => "Automobiles Hauling Explosives (not otherwise specified - Texas only)",
                6 => "Building - Commercial",
                7 => "Building - Private Dwellings",
                8 => "Building Wrecking Operators",
                9 => "Canneries and Packing Plants",
                10 => "Carriers Engaged in Both Private Carriage and Transport for Others if at least 20% of Operation is Transport for Others",
                11 => "Common Carrier - Irregular Route Common Carriers and Specialized Carriers (Texas Only) (28)",
                12 => "Common Carriers",
                13 => "Contract Carriers (Other than Chemical or Iron and Steel Haulers)",
                14 => "Contract Carriers Hauling Chemicals",
                15 => "Contract Carriers Hauling Iron and Steel",
                16 => "Contractors Automobiles Hauling Explosives (Texas Only) (86)",
                17 => "Electrical, Plumbing, Masonry, Plastering and Other Repair or Service",
                18 => "Excavating",
                19 => "Exempt Carriers (Other than Livestock Haulers)",
                20 => "Exempt Carriers Hauling Livestock",
                21 => "Film Delivery",
                22 => "Fish and Seafood",
                23 => "Frozen Food",
                24 => "Fruit and Vegetable",
                25 => "Garbage (Garbage and Ash Removal)",
                26 => "Individually Owned or Family Corporation (Other than Livestock Hauling)",
                27 => "Junk Dealers",
                28 => "Livestock Hauling",
                29 => "Logging and Lumbering",
                30 => "Machinery",
                31 => "Magazine or Newspapers",
                32 => "Mail and Parcel Post",
                33 => "Meat and Poultry",
                34 => "Metal",
                35 => "Mining",
                36 => "Quarrying",
                37 => "Sand and Gravel (Other than Quarrying)",
                38 => "Street and Road",
                39 => "Structural Iron or Steel",
                40 => "Tow Trucks for Hire (New York Only with Towed Vehicle PD Liability > $25,000)",
                41 => "Tow Trucks for Hire (Other than New York) or Tow Trucks for Hire (New York Only with Towed Vehicle PD Liability <= $25,000)",
                42 => "Tow Trucks For Hire All Other",
                _ => ""
            };
            return Task.FromResult(text);
        }
    }
}
