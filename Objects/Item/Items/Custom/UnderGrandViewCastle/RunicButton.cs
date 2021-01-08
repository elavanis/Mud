using Objects.Command;
using Objects.Command.Interface;
using Objects.Effect;
using Objects.Global;
using Objects.Item.Items.Custom.UnderGrandViewCastle.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Objects.Item.Items.Custom.UnderGrandViewCastle
{
    public class RunicButton : Item
    {
        private int oldRed = 255;
        private int oldGreen = 255;
        private int oldBlue = 255;


        public override IResult Push(IMobileObject performer, ICommand command)
        {
            string message = "";

            int red = GetRuneValue(performer, 4);
            int green = GetRuneValue(performer, 6);
            int blue = GetRuneValue(performer, 22);

            string color = GetColorName(red, green, blue);

            int change = CalcChange(red, green, blue);

            switch (change)
            {
                case 0:
                    message = "You push the button but nothing happens.";
                    break;
                case 1:
                case 2:
                    message = $"The pool of liquid in the center of the room bubbles slightly and turns {color}.";
                    break;
                case 3:
                case 4:
                    message = $"The pool of liquid in the center of the room bubbles and turns {color}.";
                    break;
                case 5:
                case 6:
                    message = $"The pool of liquid in the center of the roars with bubbles splashing on the floor around the pool {color}.";
                    break;
            }

            oldRed = red;
            oldBlue = blue;
            oldGreen = green;

            return new Result(message, true);
        }

        private int CalcChange(int red, int green, int blue)
        {
            int change = Math.Abs(ConvertRGBToPos(red) - ConvertRGBToPos(oldRed));
            change += Math.Abs(ConvertRGBToPos(green) - ConvertRGBToPos(oldGreen));
            change += Math.Abs(ConvertRGBToPos(blue) - ConvertRGBToPos(oldBlue));

            return change;
        }

        private int ConvertRGBToPos(int rgb)
        {
            switch (rgb)
            {
                case 255:
                    return 2;
                case 128:
                    return 1;
                case 0:
                default:
                    return 0;
            }
        }

        private string GetColorName(int red, int green, int blue)
        {
            if (red == 0)
            {
                if (green == 0)
                {
                    if (blue == 0)
                    {
                        return "black";
                    }
                    else if (blue == 128)
                    {
                        return "dark blue";
                    }
                    else
                    {
                        return "blue";
                    }
                }
                else if (green == 128)
                {
                    if (blue == 0)
                    {
                        return "dark green";
                    }
                    else if (blue == 128)
                    {
                        return "teal";
                    }
                    else
                    {
                        return "azure blue";
                    }
                }
                else
                {
                    if (blue == 0)
                    {
                        return "green";
                    }
                    else if (blue == 128)
                    {
                        return "spring green";
                    }
                    else
                    {
                        return "cyan";
                    }
                }
            }
            else if (red == 128)
            {
                if (green == 0)
                {
                    if (blue == 0)
                    {
                        return "maroon";
                    }
                    else if (blue == 128)
                    {
                        return "purple";
                    }
                    else
                    {
                        return "violet";
                    }
                }
                else if (green == 128)
                {
                    if (blue == 0)
                    {
                        return "olive";
                    }
                    else if (blue == 128)
                    {
                        return "gray";
                    }
                    else
                    {
                        return "slate blue";
                    }
                }
                else
                {
                    if (blue == 0)
                    {
                        return "chartreuse";
                    }
                    else if (blue == 128)
                    {
                        return "mint green";
                    }
                    else
                    {
                        return "sky blue";
                    }
                }
            }
            else
            {
                if (green == 0)
                {
                    if (blue == 0)
                    {
                        return "red";
                    }
                    else if (blue == 128)
                    {
                        return "rose";
                    }
                    else
                    {
                        return "magenta";
                    }
                }
                else if (green == 128)
                {
                    if (blue == 0)
                    {
                        return "orange";
                    }
                    else if (blue == 128)
                    {
                        return "dark pink";
                    }
                    else
                    {
                        return "orchid";
                    }
                }
                else
                {
                    if (blue == 0)
                    {
                        return "yellow";
                    }
                    else if (blue == 128)
                    {
                        return "pale yellow";
                    }
                    else
                    {
                        return "white";
                    }
                }
            }
        }

        private int GetRuneValue(IMobileObject performer, int roomNumber)
        {
            IRoom room = GlobalReference.GlobalValues.World.Zones[performer.Room.Zone].Rooms[roomNumber];
            foreach (var item in room.Items)
            {
                IRunicStatue runicStatue = item as IRunicStatue;

                if (runicStatue != null)
                {
                    switch (runicStatue.SelectedRune)
                    {
                        case 0:
                            return 255;
                        case 1:
                            return 128;
                        case 2:
                            return 0;
                    }
                }
            }

            return 255;
        }
    }
}
