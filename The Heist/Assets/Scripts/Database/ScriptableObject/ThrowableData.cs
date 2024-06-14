using UnityEngine;
[CreateAssetMenu(fileName = "ThrowableDataTemplate",  menuName = "WeaponsData", order = 1)]
public class ThrowableData : ItemData
{
    [SerializeField] private float explosionRange;
    [SerializeField] private DamageContainer damageContainer;
    [SerializeField] private Texture2D texture;

    public float ExplosionRange {  get { return explosionRange; } }
    public DamageContainer DamageContainer { get { return damageContainer; } }
    public Texture2D Texture { get {  return texture; } }

    public void Awake()
    {
        itemType = ItemType.ThrowableWeapon;
    }
}