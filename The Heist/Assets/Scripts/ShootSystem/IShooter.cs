using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    void Shoot(Vector3 initialPosition, Vector3 direction, ShootType shootType);
    void Reload();
}
