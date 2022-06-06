using Objects.Command;
using Objects.Command.Interface;
using Objects.Item.Items.Custom.UnderGrandViewCastle.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Item.Items.Custom.UnderGrandViewCastle
{
    public class RunicStatue : Item, IRunicStatue
    {
        private List<string> runes = new List<string>() { "ᚠ", "ᚢ", "ᚱ" };

        private int selectedRune = 0;

        public RunicStatue(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            Attributes.Add(ItemAttribute.NoGet);

            KeyWords.Add("statue");
            KeyWords.Add("rune");
            KeyWords.Add("runic");
        }

        public int SelectedRune
        {
            get
            {
                return selectedRune;
            }
        }

        public string CalculateExamDescription()
        {
            string rune = runes[selectedRune];
            string examDescription = $"The priest is dressed in robes with a medallion hanging from their belt.  The rune on the medallion appears to be able to be turned.  The run that is currently showing on the medallion is {rune}.";
            return examDescription;
        }

        public override IResult Turn(IMobileObject performer, ICommand command)
        {
            selectedRune++;

            if (selectedRune >= runes.Count)
            {
                selectedRune = 0;
            }

            ExamineDescription = CalculateExamDescription();

            return new Result($"You turn the medallion so that the rune {runes[selectedRune]} is showing.", true);
        }
    }
}
