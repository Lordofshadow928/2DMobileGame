using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureAnimation : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        boxCollider = this.GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("openTreasure");
            boxCollider.enabled = false; // Disable the collider to prevent further interactions
        }
    }
}
