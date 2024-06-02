using Codice.Client.BaseCommands;
using UnityEngine;

public enum DamageType
{
    Melee,
    Ranged,
    Explosion,
    Fire
}

public interface IDamageble {
    void TakeDamage(DamageContainer damage);
}
