using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Corpse : Container, ICorpse
    {
        [ExcludeFromCodeCoverage]
        public DateTime TimeOfDeath { get; set; }

        public ICorpse Clone()
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
