using System;
using UnityEngine;

public class Player1Catch : MonoBehaviour
{
    private Rigidbody2D pizzaRb;
    private float _rotation;
    [SerializeField] private float rotationForce;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pizza"))
        {
            pizzaRb = other.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("Pizza!");
            pizzaRb.linearVelocity = Vector2.zero;
            pizzaRb.bodyType = RigidbodyType2D.Kinematic;
            pizzaRb.gameObject.transform.SetParent(transform);
            pizzaRb.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        _rotation = Input.GetAxis("Vertical");
        Vector3 rotation = transform.localEulerAngles;
        rotation.z -= (_rotation * rotationForce) * Time.fixedDeltaTime;
        transform.localEulerAngles = rotation;
    }
    
}
