using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Game Aristotle", menuName = "GameObjects/Items")]
public class Item_Data_So : ScriptableObject
{
    public string item_name;
    public Item_Type item_type;
    public int stack_limit;
    public Sprite item_icon;

    public virtual void Use_Effect()
    {


    }
}
