namespace GSARTLHelper.Models
{
    public static class SarGroupExtensions
    {
        public static SarGroup? GetBestGuessFromPartialName(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
            {
                return null;
            }
            var groups = GetSarGroups();
            var bestMatch = groups
                .Where(g => g.Name.Contains(partialName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(g => g.Name.Length)
                .FirstOrDefault();
            return bestMatch;
        }

        public static SarGroup? GetGroup(Guid Id)
        {
            return GetSarGroups().FirstOrDefault(g => g.Id == Id);
        }
        public static string? GetGroupName(Guid? Id)
        {
            if(Id == null) { return null; }
            return GetSarGroups().FirstOrDefault(g => g.Id == Id)?.Name;
        }
        public static List<SarGroup> GetSarGroups()
        {
            return new List<SarGroup>
            {
                 new SarGroup { Id = new Guid("AB67B6D4-F795-43AD-9506-69880F7BEAFD"), Name = "Alberni Valley Rescue Squad" },
                 new SarGroup { Id = new Guid("F1B9CA16-CB19-4DD2-961F-FE3EB6CC6477"), Name = "Archipelago SAR" },
                 new SarGroup { Id = new Guid("5C8A48A2-62F6-44E8-93CC-18ECAAE7E2ED"), Name = "Arrow Lakes SAR" },
                 new SarGroup { Id = new Guid("BE921E46-2521-4B8C-9438-0645CCB19DE8"), Name = "Arrowsmith SAR" },
                 new SarGroup { Id = new Guid("F964C5E1-2337-4761-AB36-2965F57E7DC4"), Name = "Atlin SAR" },
                 new SarGroup { Id = new Guid("71FFF997-108B-4DDF-914E-F81069F8EA26"), Name = "Barriere SAR" },
                 new SarGroup { Id = new Guid("CDA9B3A3-FC65-4803-8685-813BCE298DEF"), Name = "Bella Coola Valley SAR" },
                 new SarGroup { Id = new Guid("8940A059-6A5A-4004-9D1E-758C4D576C2E"), Name = "Bulkley Valley SAR" },
                 new SarGroup { Id = new Guid("8B8D2A86-91D6-4606-B2D7-A9EDF3C8EE35"), Name = "Burns Lake SAR" },
                 new SarGroup { Id = new Guid("499A9B5C-E712-4968-8B9F-1DEB9D402915"), Name = "Campbell River SAR" },
                 new SarGroup { Id = new Guid("DBCE7252-92D1-481F-ADD0-1923F1F7FD39"), Name = "Castlegar SAR" },
                 new SarGroup { Id = new Guid("3642094A-274C-44DA-B379-ED42E9265FF8"), Name = "Central Cariboo SAR" },
                 new SarGroup { Id = new Guid("EA48BC49-A566-4248-A4B5-C1A6DBBEF00B"), Name = "Central Fraser Valley SAR" },
                 new SarGroup { Id = new Guid("C704D0F8-AF89-4143-BD3E-CB4EAC7A7AA7"), Name = "Central Okanagan SAR" },
                 new SarGroup { Id = new Guid("4643747E-FB2A-4230-B4B8-8506F9951DFF"), Name = "Chetwynd SAR" },
                 new SarGroup { Id = new Guid("6FD79D61-ED33-46E1-A1AA-E3BC1F24A143"), Name = "Chilliwack SAR" },
                 new SarGroup { Id = new Guid("12FDF8F5-4127-456E-9F6D-C6912161BE6F"), Name = "Columbia Valley SAR" },
                 new SarGroup { Id = new Guid("FF7F03D5-E414-443F-8315-82BB378ABBD8"), Name = "Comox Valley SAR" },
                 new SarGroup { Id = new Guid("6C4ED3C8-18D1-4F25-BEB1-D8C3B5272267"), Name = "Coquitlam SAR" },
                 new SarGroup { Id = new Guid("F2874DE5-0066-45A5-A3F8-1A38A64D67EC"), Name = "Cowichan SAR" },
                 new SarGroup { Id = new Guid("A3190007-E0EA-49F8-95F8-F8FF8396A38B"), Name = "Cranbrook SAR" },
                 new SarGroup { Id = new Guid("74E5E639-A831-427F-8494-4DCD0A55BF37"), Name = "Creston Valley SAR" },
                 new SarGroup { Id = new Guid("83081A8A-6C30-45A8-B9B1-D625090148D6"), Name = "Elkford SAR" },
                 new SarGroup { Id = new Guid("D44C628D-E9AD-43CE-9671-2965C64FF0E5"), Name = "Fernie SAR" },
                 new SarGroup { Id = new Guid("D0AEC6BB-EE25-437B-8A8C-BEAA284FF685"), Name = "Fort Nelson SAR" },
                 new SarGroup { Id = new Guid("589F720D-A304-49CF-B40C-8E944D7C9D2F"), Name = "Fort St. James SAR" },
                 new SarGroup { Id = new Guid("0E63E227-27A8-4B11-8043-EC2C589A4CBA"), Name = "Golden SAR" },
                 new SarGroup { Id = new Guid("169C529D-792A-4615-ABE9-78203A6A70A4"), Name = "Grand Forks SAR" },
                 new SarGroup { Id = new Guid("EF91565B-77F8-428B-9D15-D0EAA4043A0E"), Name = "Hope SAR" },
                 new SarGroup { Id = new Guid("E05BB4DE-8786-4289-B39B-2574C41E2777"), Name = "Houston SAR" },
                 new SarGroup { Id = new Guid("C111498E-74F0-46B3-8442-0B38D435C854"), Name = "Juan de Fuca SAR" },
                 new SarGroup { Id = new Guid("CE558295-B9E2-41A3-88F1-AE029EC1AE0D"), Name = "Kamloops SAR" },
                 new SarGroup { Id = new Guid("B04FE1E7-979F-40CC-892C-A74EEA295ECB"), Name = "Kaslo SAR" },
                 new SarGroup { Id = new Guid("16FAD27A-76E6-471E-A3FA-146CCB740DBC"), Name = "Kent Harrison SAR" },
                 new SarGroup { Id = new Guid("2EFA81F4-3E9B-4E45-BE94-8166AAA9298C"), Name = "Keremeos SAR" },
                 new SarGroup { Id = new Guid("87756CEF-01AC-493A-89BE-464CADAE7EE1"), Name = "Kimberley SAR" },
                 new SarGroup { Id = new Guid("6A39C786-4C74-4B1E-91A6-4E1FCFE7584D"), Name = "Kitimat SAR" },
                 new SarGroup { Id = new Guid("37F43ECF-9CD5-4EB5-9699-2C0DBB7974F3"), Name = "Ladysmith SAR" },
                 new SarGroup { Id = new Guid("64B9D581-05FD-4D9B-ABFF-B24BFAFCEA2C"), Name = "Lions Bay SAR" },
                 new SarGroup { Id = new Guid("4A2BA222-3766-4F38-86E4-1724FDE94AF3"), Name = "Logan Lake SAR" },
                 new SarGroup { Id = new Guid("46698CE0-B146-40E2-B834-0089DECE3896"), Name = "Mackenzie SAR" },
                 new SarGroup { Id = new Guid("D83B66D5-C697-448F-9E4F-5DA7828A66A5"), Name = "Metchosin SAR" },
                 new SarGroup { Id = new Guid("FEC27732-35F1-43D6-9597-7E0D2FEEDD65"), Name = "Mission SAR" },
                 new SarGroup { Id = new Guid("11A311E6-6768-46AA-9F3C-571AA95E3726"), Name = "Nakusp SAR" },
                 new SarGroup { Id = new Guid("937148E6-72FD-456B-9DD9-38E975492D5F"), Name = "Nanaimo SAR" },
                 new SarGroup { Id = new Guid("2679596A-3D96-4E74-913E-BF273235FC6F"), Name = "Nechako Valley SAR" },
                 new SarGroup { Id = new Guid("B2CD40D5-6ABF-4FF8-A89E-F5FE6B995C89"), Name = "Nelson SAR" },
                 new SarGroup { Id = new Guid("5B2C5A11-07DC-4104-99A5-364C204D4B32"), Name = "Nicola Valley SAR" },
                 new SarGroup { Id = new Guid("97717FBA-977C-49CD-B105-D55FA705AA14"), Name = "North Peace SAR" },
                 new SarGroup { Id = new Guid("0EBB892E-F241-466A-B1E1-1E05F01EF0C7"), Name = "North Shore SAR" },
                 new SarGroup { Id = new Guid("DF7FBC23-17F2-41AB-8DEB-AF00D27C5B7F"), Name = "Oliver Osoyoos SAR" },
                 new SarGroup { Id = new Guid("62231DA2-DCB8-413E-8C60-2A0B2E8DC214"), Name = "Parkland SAR" },
                 new SarGroup { Id = new Guid("B3AE6204-9E95-452A-9C10-5EB4178E0ABE"), Name = "Pemberton SAR" },
                 new SarGroup { Id = new Guid("65D6C93C-63F9-476F-8A63-5C8A968F4E11"), Name = "PEMO SAR" },
                 new SarGroup { Id = new Guid("124CA510-0BD2-4F53-82BE-3A5F4960BFCC"), Name = "Penticton SAR" },
                 new SarGroup { Id = new Guid("2207EB6C-E292-47E8-AE3D-68855B846109"), Name = "Powell River SAR" },
                 new SarGroup { Id = new Guid("20A1A9F3-CB11-43F8-BEFE-C8933A566764"), Name = "Prince George SAR" },
                 new SarGroup { Id = new Guid("9893E045-D590-44E4-8AF4-5DBB1F1661A4"), Name = "Prince Rupert SAR" },
                 new SarGroup { Id = new Guid("8CA3E11B-5A87-4C72-A11C-F14225AC7AAF"), Name = "Princeton SAR" },
                 new SarGroup { Id = new Guid("2F07BE02-EC9F-435D-B8E2-0B31799879BC"), Name = "Quesnel SAR" },
                 new SarGroup { Id = new Guid("9202836C-DC5C-4C62-8190-022A03769098"), Name = "Revelstoke SAR" },
                 new SarGroup { Id = new Guid("88B20B0B-04BC-4596-A2F5-7CCCB317A4A0"), Name = "Ridge Meadows SAR" },
                 new SarGroup { Id = new Guid("76A1101B-464D-4D12-ABBC-266599F2F5DD"), Name = "Robson Valley SAR" },
                 new SarGroup { Id = new Guid("A1C1A217-4AE3-462C-A7D4-68FFAA8C0210"), Name = "Rossland SAR" },
                 new SarGroup { Id = new Guid("8E824695-1EDD-49FE-BADF-F42F8A34A95F"), Name = "Salt Spring Island SAR" },
                 new SarGroup { Id = new Guid("890789F2-9DB1-4785-BF00-BF051676B11E"), Name = "Shuswap SAR" },
                 new SarGroup { Id = new Guid("3CD654FA-8B48-4C75-BB75-B7834639990C"), Name = "South Cariboo SAR" },
                 new SarGroup { Id = new Guid("7AF5AD55-2655-4218-B693-56F48353E190"), Name = "South Columbia SAR" },
                 new SarGroup { Id = new Guid("C0A7C62C-8C76-43F5-9357-E65394CCA2CB"), Name = "South Fraser SAR" },
                 new SarGroup { Id = new Guid("98851139-E736-4974-A672-6972F3A8965F"), Name = "South Peace SAR" },
                 new SarGroup { Id = new Guid("452E4432-F01C-4FDB-8DB4-E7399FC09A97"), Name = "Sparwood SAR" },
                 new SarGroup { Id = new Guid("F7CD798B-52AD-423E-B66F-BD01D3B70D5D"), Name = "Squamish SAR" },
                 new SarGroup { Id = new Guid("BFDAB6B8-BEE2-4157-A144-23965C166BD8"), Name = "Stewart SAR" },
                 new SarGroup { Id = new Guid("893B590C-A159-4A89-9CA4-A3DF24D6ABFE"), Name = "Sunshine Coast SAR" },
                 new SarGroup { Id = new Guid("D5A57651-6C40-4A8E-A442-F0D7294FE0ED"), Name = "Terrace SAR" },
                 new SarGroup { Id = new Guid("B865EDF6-F0A7-4241-8CD4-8A5942C7B18A"), Name = "Tumbler Ridge SAR" },
                 new SarGroup { Id = new Guid("8FB71ABC-4C5C-43A8-8E68-AA75DD2262EB"), Name = "Vernon SAR" },
                 new SarGroup { Id = new Guid("5153C373-4B73-45F6-99B6-F4DA00D28B92"), Name = "Wells Gray SAR" },
                 new SarGroup { Id = new Guid("52078C4B-4A25-4103-A7EA-224B68AB5C55"), Name = "West Chilcotin SAR" },
                 new SarGroup { Id = new Guid("77F87A03-46E8-4C70-A05B-F32DAE58276B"), Name = "Westcoast Inland SAR" },
                 new SarGroup { Id = new Guid("0B0DC8D4-9A62-4987-8EFA-43FC2A31D598"), Name = "Whistler SAR" },

            };
        }
    }
}
