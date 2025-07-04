using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField]private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
    
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Death");
                GetComponent<PlayerMoveMent>().enabled = false;
                dead = true;
            }
        }
        
    }
}
