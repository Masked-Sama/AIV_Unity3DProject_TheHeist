using UnityEngine;

public interface IGrenade
{
    void Throw(Transform cameraView, Transform spawnTransform, float force, float throwUpwardForce);
}
