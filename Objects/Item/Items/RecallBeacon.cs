using Objects.Item.Items.Interface;

namespace Objects.Item.Items
{
    public class RecallBeacon : Item, IRecallBeacon
    {
        public RecallBeacon()
        {
            Attributes.Add(ItemAttribute.NoGet);
            ShortDescription = "A blue recall beacon hovers in the air.";
            LookDescription = "The recall beacon's soft blue color of a noon day sky is contrasted by the extreme hardness of the recall beacon itself.";
            ExamineDescription = "The beacon is shaped like a giant nine foot crystal.  It hovers in the air and with effort it can be made to spin but no amount of force has been able to move it. While being mostly translucent if you look closely you can see yourself faintly reflected in its smooth finish.";
            SentenceDescription = "recall beacon";
            KeyWords.Add("recall");
            KeyWords.Add("beacon");
        }
    }
}
