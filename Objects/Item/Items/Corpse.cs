using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Corpse : Container, ICorpse
    {
        public Corpse()
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
            Corpse corpse = new Corpse();
            corpse.ExamineDescription = this.ExamineDescription;
            corpse.LookDescription = this.LookDescription;
            corpse.SentenceDescription = this.SentenceDescription;
            corpse.ShortDescription = this.ShortDescription;
            corpse.TimeOfDeath = this.TimeOfDeath;

            foreach (IItem item in this.Items)
            {
                corpse.Items.Add(item);
            }

            return corpse;
        }
    }
}
