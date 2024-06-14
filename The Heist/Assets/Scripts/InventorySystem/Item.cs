using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData itemObj;
    [Range(1,30)]
    [SerializeField]
    private int quantity;

    public ItemData ItemObj {  get { return itemObj; } }
    public int Quantity { get { return quantity; } }

  
}