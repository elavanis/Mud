using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Global.Language.Translator;

namespace Objects.Item.Items.Interface
{
    public interface IRead
    {
        Languages WrittenLanguage { get; set; }
        string Read(IMobileObject mobileObject);
    }
}
