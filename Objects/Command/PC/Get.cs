using Objects.Command.Interface;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Global;
using static Objects.Item.Item;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;
using Objects.Language;

namespace Objects.Command.PC
{
    public class Get : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Get [Item Name] {Container}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Get" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result("What would you like to get?", true);
            }
            else if (command.Parameters.Count == 1)
            {
                IParameter parameter = command.Parameters[0];

                if (parameter.ParameterValue.ToUpper() == "ALL")
                {
                    for (int i = performer.Room.Items.Count - 1; i >= 0; i--)
                    {
                        IItem roomItem = performer.Room.Items[i];
                        ICommand innerCommand = new Command();
                        IParameter innerParameter = new Parameter(roomItem.KeyWords[0]);
                        innerCommand.Parameters.Add(innerParameter);
                        IResult innerResult = PerformCommand(performer, innerCommand);
                        GlobalReference.GlobalValues.Notify.Mob(performer, new TranslationMessage(innerResult.ResultMessage));
                    }
                    return new Result("", false);
                }
                else
                {
                    IItem item = GlobalReference.GlobalValues.FindObjects.FindItemsInRoom(performer.Room, parameter.ParameterValue, parameter.ParameterNumber);

                    if (item != null)
                    {
                        if (!item.Attributes.Contains(ItemAttribute.NoGet))
                        {
                            if (item is ICorpse corpse)
                            {
                                if (corpse.Killer != performer)
                                {
                                    return new Result($"Unable to pickup the corpse belonging to {corpse.Killer.KeyWords[0]}.", true);
                                }
                            }


                            IRoom room = performer.Room;
                            GlobalReference.GlobalValues.Engine.Event.Get(performer, item);
                            room.RemoveItemFromRoom(item);
                            AddItemToPerformer(performer, item);

                            string message = string.Format("You pickup the {0}.", item.SentenceDescription);
                            return new Result(message, false);
                        }
                        else
                        {
                            string message = string.Format("You were unable to get {0}.", item.SentenceDescription);
                            return new Result(message, true);
                        }
                    }
                    else
                    {
                        string message = string.Format("You were unable to find {0}.", parameter.ParameterValue);
                        return new Result(message, true);
                    }
                }
            }
            else
            {
                IParameter parameterContainer = command.Parameters[1];
                if (GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parameterContainer.ParameterValue, parameterContainer.ParameterNumber, true, true, false, false) is IContainer container)
                {
                    if (container is ICorpse corpse)
                    {
                        if (corpse.Killer != performer)
                        {
                            return new Result($"Unable to get items from the corpse belonging to {corpse.Killer.KeyWords[0]}", true);
                        }
                    }

                    IParameter parameterItem = command.Parameters[0];
                    if (parameterItem.ParameterValue.ToUpper() == "ALL")
                    {
                        for (int i = container.Items.Count - 1; i >= 0; i--)
                        {
                            IItem containerItem = container.Items[i];
                            ICommand innerCommand = new Command();
                            IParameter innerParameter = new Parameter(containerItem.KeyWords[0]);
                            innerCommand.Parameters.Add(innerParameter);
                            innerCommand.Parameters.Add(parameterContainer);
                            IResult innerResult = PerformCommand(performer, innerCommand);
                            GlobalReference.GlobalValues.Notify.Mob(performer, new TranslationMessage(innerResult.ResultMessage));
                        }
                        return new Result("", false);
                    }
                    else
                    {
                        List<IItem> foundItems = container.Items.FindAll(i => i.KeyWords.Contains(parameterItem.ParameterValue)).ToList();
                        if (foundItems.Count > parameterItem.ParameterNumber)
                        {
                            IItem item = foundItems[parameterItem.ParameterNumber];
                            string message;

                            if (!item.Attributes.Contains(ItemAttribute.NoGet))
                            {
                                GlobalReference.GlobalValues.Engine.Event.Get(performer, item, container);
                                container.Items.Remove(item);
                                AddItemToPerformer(performer, item);

                                if (item is IMoney)
                                {
                                    IMoney money = item as IMoney;
                                    string moneyString = GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(money.Value);
                                    message = string.Format("You get the {0} from the {1}.", moneyString, parameterContainer.ParameterValue);
                                }
                                else
                                {
                                    message = string.Format("You get the {0} from the {1}.", parameterItem.ParameterValue, parameterContainer.ParameterValue);
                                }
                            }
                            else
                            {
                                message = string.Format("You were unable to get {0}.", item.SentenceDescription);
                            }

                            return new Result(message, false);
                        }
                        else
                        {
                            string message = string.Format("Unable to find item {0} in container {1}.", parameterItem.ParameterValue, parameterContainer.ParameterValue);
                            return new Result(message, true);
                        }
                    }
                }
                else
                {
                    string message = string.Format("Unable to find container {0}.", parameterContainer.ParameterValue);
                    return new Result(message, true);
                }
            }
        }

        private void AddItemToPerformer(IMobileObject performer, IItem item)
        {
            if (!(item is IMoney money))
            {
                performer.Items.Add(item);
            }
            else
            {
                performer.Money += money.Value;
            }
        }
    }
}
