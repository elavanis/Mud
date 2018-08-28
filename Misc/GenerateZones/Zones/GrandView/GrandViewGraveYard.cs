using System;
using System.Collections.Generic;
using System.Text;
using Objects.Zone.Interface;
using static GenerateZones.RandomZoneGeneration;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewGraveYard : BaseZone, IZoneCode
    {
        public GrandViewGraveYard() : base(21)
        {
        }

        public IZone Generate()
        {
            List<string> names = new List<string>() { "Falim Nasha", "Bushem Dinon", "Stavelm Eaglelash", "Giu Thunderbash", "Marif Hlisk", "Fim Grirgav", "Strarcar Marshgem", "Storth Shadowless", "Tohkue-zid Lendikrafk", "Vozif Jikrehd", "Dranrovelm Igenomze", "Zathis Vedergi", "Mieng Chiao", "Thuiy Chim", "Sielbonron Canderger", "Craldu Gacevi"
            "Rumeim Shennud","Nilen Cahrom","Bei Ashspark","Hii Clanbraid","Sodif Vatsk","Por Rorduz","Grorcerth Forestsoar","Gath Distantthorne","Duhvat-keuf Faltrueltrim","Ham-kaoz Juhpafk","Rolvoumvald Gibenira","Rondit Vumregi","Foy Sheiy","Fiop Tei","Fruenrucu Jalbese","Fhanun Guldendal"};

            RandomZoneGeneration randZoneGen = new RandomZoneGeneration(5, 5, Zone.Id);
            RoomDescription description = new RoomDescription();
            description.LongDescription = "The dirt has been freshly disturbed where a body has been recently placed in the ground.";
            description.ExamineDescription = "Some flowers have been placed on the headstone that belongs to {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LongDescription = "The headstone has been here a while and is starting to show its age.";
            description.ExamineDescription = "The headstone name has worn off and is impossible to read.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LongDescription = "A grand tower of marble rises to the sky.  This person must have been important or rich in life.";
            description.ExamineDescription = "The headstone that belongs to {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);


            FlavorOption option = new FlavorOption();
            option.FlavorValues.Add("{name}", names);
            description.FlavorOption.Add(option);
            randZoneGen.RoomDescriptions.Add(description);

            Zone = randZoneGen.Generate();

            return Zone;
        }
    }
}
