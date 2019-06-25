using Objects.Command.Interface;
using Objects.Item.Interface;

namespace Objects.Item.Items.Interface
{
    public interface IEnchantery : IItem
    {
        IResult Enchant(IItem item);
        int CostToEnchantLevel1Item { get; set; }
        decimal SuccessRate { get; set; }
    }
}