using Objects.Die;
using Objects.Effect;
using Objects.Global;
using Objects.Global.Language;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Interface;
using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using static Shared.TagWrapper.TagWrapper;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            GenerateZones.Program.Main(null);
            GenerateZoneMaps.Program.Main(null);
            string assets = @"C:\Mud\Assets\Map";

            if (!Directory.Exists(assets))
            {
                Directory.CreateDirectory(assets);
            }

            foreach (string file in Directory.GetFiles(assets))
            {
                File.Delete(file);
            }

            string maps = @"C:\Mud\Maps";
            foreach (string file in Directory.GetFiles(maps))
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(assets, fileName));
            }
        }


        private static ISpell GenerateSpell(string spellName)
        {
            ISpell spell = new SingleTargetSpell();
            spell.SpellName = spellName;
            spell.ManaCost = 0;
            spell.Parameter.Dice = new Dice(5, 2);
            string message = "The nurse says {0} and is briefly surrounded in a aura of light.";
            List<ITranslationPair> translate = new List<ITranslationPair>();
            ITranslationPair translationPair = new TranslationPair(Translator.Languages.Magic, spellName);
            translate.Add(translationPair);
            ITranslationMessage translationMessage = new TranslationMessage(message, TagType.Info, translate);
            spell.RoomNotificationSuccess = translationMessage;
            spell.PerformerNotificationSuccess = new TranslationMessage("you cast a spell");
            return spell;
        }
    }
}
