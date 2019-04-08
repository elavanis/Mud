using Objects.Global;
using Objects.Interface;
using Objects.LoadPercentage.Interface;
using Objects.Magic.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Objects.Mob.Interface;
using Objects.Item.Items.Interface;
using Objects.Item.Interface;
using Objects.Room.Interface;

namespace Objects
{
    public abstract class BaseObject : ILoadable, IBaseObject
    {
        #region Properties
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }

        [ExcludeFromCodeCoverage]
        public int Zone { get; set; }

        /// <summary>
        /// Description used in the middle of a sentence."
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string SentenceDescription { get; set; } = "";

        /// <summary>
        /// Description used when the look command is used in a room and the object is in side.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Description used when looking at the item."
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string LookDescription { get; set; }

        /// <summary>
        /// Description used when examining an item."
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string ExamineDescription { get; set; }

        private List<string> _keywords = null;
        public List<string> KeyWords
        {
            get
            {
                if (_keywords == null)
                {
                    _keywords = new List<string>();
                }
                return _keywords;
            }
        }

        private Dictionary<string, List<string>> _flavorOptions = null;
        public Dictionary<string, List<string>> FlavorOptions
        {
            get
            {
                if (_flavorOptions == null)
                {
                    _flavorOptions = new Dictionary<string, List<string>>();
                }

                return _flavorOptions;
            }
        }

        private Dictionary<string, List<string>> _zoneSyncOptions = null;
        public Dictionary<string, List<string>> ZoneSyncOptions
        {
            get
            {
                if (_zoneSyncOptions == null)
                {
                    _zoneSyncOptions = new Dictionary<string, List<string>>();
                }

                return _zoneSyncOptions;
            }
        }

        private List<IEnchantment> _enchantments = null;
        public List<IEnchantment> Enchantments
        {
            get
            {
                if (_enchantments == null)
                {
                    _enchantments = new List<IEnchantment>();
                }
                return _enchantments;
            }
        }
        #endregion Properties

        public virtual void FinishLoad(int zoneObjectSyncValue = -1)
        {
            if (zoneObjectSyncValue != -1)
            {
                ZoneObjectSyncLoad(zoneObjectSyncValue);
            }

            if (_flavorOptions != null && _flavorOptions.Count != 0)
            {
                foreach (string key in FlavorOptions.Keys)
                {
                    int selectedPosition = GlobalReference.GlobalValues.Random.Next(FlavorOptions[key].Count);
                    string selectedFlavorOption = FlavorOptions[key][selectedPosition];

                    ShortDescription = ShortDescription.Replace(key, selectedFlavorOption);
                    LookDescription = LookDescription.Replace(key, selectedFlavorOption);
                    ExamineDescription = ExamineDescription.Replace(key, selectedFlavorOption);

                    SentenceDescription = SentenceDescription.Replace(key, selectedFlavorOption);
                    for (int i = 0; i < KeyWords.Count; i++)
                    {
                        KeyWords[i] = KeyWords[i].Replace(key, selectedFlavorOption);
                    }
                }
            }


            if (this is ILoadableItems loadableItem)
            {
                if (loadableItem.LoadableItems.Count > 0)
                {
                    foreach (ILoadPercentage percentage in loadableItem.LoadableItems)
                    {
                        if (percentage.Load)
                        {
                            AddItem(this, percentage.Object);
                            percentage.Object.FinishLoad();
                        }
                    }
                }
            }
        }

        private void AddItem(BaseObject baseObject, IBaseObject @object)
        {
            if (baseObject is IContainer container)
            {
                if (@object is IItem item)
                {
                    container.Items.Add(item);
                    return;
                }
            }

            if (baseObject is IRoom room)
            {
                if (@object is INonPlayerCharacter npc)
                {
                    room.AddMobileObjectToRoom(npc);
                    npc.Room = room;
                    return;
                }

                if (@object is IItem item)
                {
                    room.AddItemToRoom(item);
                    return;
                }
            }
        }

        public virtual void ZoneObjectSyncLoad(int syncValue)
        {
            if (ZoneSyncOptions.Count != 0)
            {
                foreach (string key in ZoneSyncOptions.Keys)
                {
                    string selectedFlavorOption = ZoneSyncOptions[key][syncValue];

                    ShortDescription = ShortDescription.Replace(key, selectedFlavorOption);
                    LookDescription = LookDescription.Replace(key, selectedFlavorOption);
                    ExamineDescription = ExamineDescription.Replace(key, selectedFlavorOption);
                    if (SentenceDescription != null) //rooms do not have sentence descriptions
                    {
                        SentenceDescription = SentenceDescription.Replace(key, selectedFlavorOption);
                    }

                    if (key == "ZoneSyncKeywords")
                    {
                        KeyWords.AddRange(selectedFlavorOption.Split(','));
                    }
                }
            }
        }
    }
}
