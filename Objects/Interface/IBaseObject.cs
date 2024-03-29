﻿using Objects.Command.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Interface
{
    public interface IBaseObject : ILoadable
    {
        Dictionary<string, List<string>> FlavorOptions { get; }
        int Id { get; set; }
        List<string> KeyWords { get; }

        /// <summary>
        /// Description when Examining
        /// </summary>
        //string ExamineDescription { get; set; }
        string ExamineDescription { get; set; }
        /// <summary>
        /// Description when looking at object
        /// </summary>
        string LookDescription { get; set; }
        /// <summary>
        /// Description when item is listed in room or room description for look.
        /// </summary>
        string ShortDescription { get; set; }
        /// <summary>
        /// Description used when item used in sentence, room title.
        /// </summary>
        string SentenceDescription { get; set; }
        /// 
        int ZoneId { get; set; }
        Dictionary<string, List<string>> ZoneSyncOptions { get; }

        List<IEnchantment> Enchantments { get; }

        #region Object Commands
        IResult Turn(IMobileObject performer, ICommand command);
        IResult Push(IMobileObject performer, ICommand command);
        #endregion Object Commands
    }
}