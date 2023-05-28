using UnityEngine;

namespace Items
{
    
    
    public class Item : ScriptableObject
    {
        public string displayName;
        public Sprite icon;
        public PlayerAbilities abilityModifiers;
        public int price = 10;

        public static Item GetById(string id)
        {
            return Resources.Load<Item>("Items/" + id);
        }

        public virtual string GetDisplayType()
        {
            return "Предмет";
        }
    }
}
