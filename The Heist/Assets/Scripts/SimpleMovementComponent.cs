using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementComponent : MonoBehaviour
{
    private float speed = 10.0f;


    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.left* (Time.deltaTime*speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.right*(Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A)){
            transform.position -= Vector3.forward*(Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.forward*(Time.deltaTime * speed);
        }
        
    }


}
