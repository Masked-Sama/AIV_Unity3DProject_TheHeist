using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;
    [Range(1,30)]
    [SerializeField]
    private int quantity;

    public ItemData ItemData {  get { return itemData; } }
    public int Quantity { get { return quantity; } }

  
}