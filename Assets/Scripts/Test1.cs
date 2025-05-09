using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * 9 * hor * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector2.up * 5;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} Enter 2D {collision.gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log($"{gameObject.name} Trigger 2D {collider.gameObject.name}");

    }
    
}
