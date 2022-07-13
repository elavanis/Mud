using Objects.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System.Collections.Generic;

namespace Objects.Global.Notify.Interface
{
    public interface INotify
    {
        void Room(IMobileObject performer, IBaseObject? target, IRoom room, ITranslationMessage message, List<IMobileObject>? excludedMobs = null, bool requiredToSee = false, bool requiredToHear = false);
        void Zone(IMobileObject performer, IBaseObject? target, IZone zone, ITranslationMessage message, List<IMobileObject>? excludedMobs = null, bool requiredToSee = false, bool requiredToHear = false);
        void Mob(IMobileObject performer, IBaseObject? target, IMobileObject notifie, ITranslationMessage message, bool requiredToSee = false, bool requiredToHear = false);
        void Mob(IMobileObject notifie, ITranslationMessage message);
    }
}
