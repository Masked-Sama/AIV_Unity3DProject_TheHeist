using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemObject itemObj;
    [Range(1,30)]
    [SerializeField]
    private int quantity;

    public ItemObject ItemObj {  get { return itemObj; } }
    public int Quantity { get { return quantity; } }    
}