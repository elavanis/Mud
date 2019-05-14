using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using System;

namespace Objects.Item.Items.Interface
{
    public interface ICorpse : IBaseObject, IContainer, IItem
    {
        IMobileObject Killer { get; set; }

        DateTime TimeOfDeath { get; set; }

        new ICorpse Clone();
    }
}