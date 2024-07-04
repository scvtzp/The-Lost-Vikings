namespace Manager
{
    // 무기같은애들 Equip로 아이템 타입 한번에 묶고
    // 예외처리해서 따로 처리할까 생각들어서 고민중
    public enum ItemType
    {
        Dia,
        Gold,
    }

    public struct ItemData
    {
        public ItemType Type;
        public int Count;
    }
    
    public class ItemManager
    {
        public void AddItem(ItemData itemData)
        {
            
        }
    }
}