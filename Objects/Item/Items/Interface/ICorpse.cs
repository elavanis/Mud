using Objects.Interface;
using Objects.Item.Interface;
using System;

namespace Objects.Item.Items.Interface
{
    public interface ICorpse : IBaseObject, IContainer, IItem
    {
        DateTime TimeOfDeath { get; set; }

        ICorpse Clone();
    }
}