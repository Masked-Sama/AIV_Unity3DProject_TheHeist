using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemObject itemObj;

    public ItemObject ItemObj {  get { return itemObj; } }
}