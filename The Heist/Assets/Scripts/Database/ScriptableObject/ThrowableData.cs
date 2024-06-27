using UnityEngine;
public enum GrenadeType
{
    Incendiary,
    Stun
}
[CreateAssetMenu(fileName = "ThrowableDataTemplate",  menuName = "WeaponsData/Grenade", order = 1)]
public class ThrowableData : ItemData, IInventoried
{
    [SerializeField] private float explosionRange;
    [SerializeField] private DamageContainer damageContainer;
    [SerializeField] private Texture2D texture;
    [SerializeField] private GrenadeType grenadeType;

    public float ExplosionRange {  get { return explosionRange; } }
    public DamageContainer DamageContainer { get { return damageContainer; } }
    public Texture2D Texture2D { get { return texture; } }
    public GrenadeType GrenadeType { get {  return grenadeType; } }

    public void Awake()
    {
        itemType = ItemType.ThrowableWeapon;
    }
}