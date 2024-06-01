using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMovementComponent : MonoBehaviour
{
    private float speed = 10.0f;
    protected InputAction moveAction;
    private Vector3 computedSpeed = new Vector3();

    private void OnEnable()
    {
        moveAction = InputManager.Player.Move;
    }

    private void Update()
    {

        Move();
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += Vector3.left* (Time.deltaTime*speed);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position += Vector3.right*(Time.deltaTime * speed);
        //}
        //if (Input.GetKey(KeyCode.A)){
        //    transform.position -= Vector3.forward*(Time.deltaTime * speed);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position += Vector3.forward*(Time.deltaTime * speed);
        //}
        
    }
    protected void Move()
    {
        computedSpeed.z = (moveAction.ReadValue<Vector2>().normalized).x * (Time.deltaTime * speed);
        computedSpeed.x = (moveAction.ReadValue<Vector2>().normalized).y * (Time.deltaTime * speed) * -1;
        transform.position += computedSpeed;
    }


}
