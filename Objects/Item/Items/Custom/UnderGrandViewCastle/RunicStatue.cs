using Objects.Command;
using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Item.Items.Custom.UnderGrandViewCastle
{
    public class RunicStatue : Item
    {
        public override IResult Turn(IMobileObject performer, ICommand command)
        {
            return new Result("abc", true);
        }
    }
}
