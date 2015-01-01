
namespace GoodPlayer
{
    public interface IPersistenceService<ItemType, LoadHintType>
    {
        void Save(ItemType item);
        ItemType Load(LoadHintType hint);
    }
}
