using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Game Aristotle", menuName = "GameObjects/Items")]
public class Item_Data_So : ScriptableObject
{
    public string item_name;//物品的名字
    public Item_Type item_type;//物品的类型，后续有需要自己去Item_Type扩展
    public int stack_limit;//物品的最高数量限制
    public Sprite item_icon;//物品的图标

    public virtual void Use_Effect()
    {


    }
}
