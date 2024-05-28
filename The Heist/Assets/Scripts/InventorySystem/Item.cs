using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemObject itemObj;

    public ItemObject ItemObject {  get { return itemObj; } }
}