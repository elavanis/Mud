using Objects.Global.Weapon.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Objects.Global.Weapon
{
    public class WeaponId : IWeaponId
    {
        private string filePath;

        private long id;
        public long Id
        {
            get
            {
                return Interlocked.Increment(ref id);
            }
            private set
            {
                lock (filePath)  //we can do this because we intilize before we start the mud and will not s
                {
                    id = value;
                    GlobalReference.GlobalValues.FileIO.WriteFile(filePath, value.ToString());
                }
            }
        }

        public void Initialize()
        {
            filePath = Path.Combine(GlobalReference.GlobalValues.Settings.DamageIdDirectory, "DamageId.txt");

            string fileValue = GlobalReference.GlobalValues.FileIO.ReadAllText(filePath);

            long.TryParse(fileValue, out id);
        }
    }
}
