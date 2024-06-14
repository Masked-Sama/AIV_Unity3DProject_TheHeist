using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    bool Shoot(Vector3 initialPosition, Vector3 direction, ShootType shootType);
    void Reload();
}
