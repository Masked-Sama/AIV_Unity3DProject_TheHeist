using UnityEngine;

public class WeaponDataTem : MonoBehaviour
{
    [SerializeField]
    private WeaponData data;

    public WeaponData Data
    {
        get { return data; }
        set { data = value; }
    }

}
