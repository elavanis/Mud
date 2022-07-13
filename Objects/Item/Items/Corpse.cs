using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Corpse : Container, ICorpse
    {
        public Corpse(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base("", "", examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            Opened = true;
        }

        [ExcludeFromCodeCoverage]
        public DateTime TimeOfDeath { get; set; }

        [ExcludeFromCodeCoverage]
        public IMobileObject Killer { get; set; }
        [ExcludeFromCodeCoverage]
        public IMobileObject OriginalMob { get; set; }

        public new ICorpse Clone()
        {
            Corpse corpse = new Corpse(this.ExamineDescription, this.LookDescription, this.SentenceDescription, this.ShortDescription);
            corpse.TimeOfDeath = this.TimeOfDeath;
            corpse.Items.AddRange(this.Items);

            return corpse;
        }
    }
}
