using UnityEngine;

public class Item : MonoBehaviour
{
    private const float speedRotation = 100;

    [SerializeField]
    private ItemObject itemObj;
    [Range(1,30)]
    [SerializeField]
    private int quantity;

    public ItemObject ItemObj {  get { return itemObj; } }
    public int Quantity { get { return quantity; } }

    private void Update()
    {
        transform.Rotate(0,Time.deltaTime * speedRotation,0);
    }
}