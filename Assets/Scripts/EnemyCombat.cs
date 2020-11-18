using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public Transform attack;
    public float attackRange = 1f;

    public LayerMask playerLayer;
    public LayerMask enemyLayer; 


    int damagePerAttack;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        setDpa(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Collider2D>().IsTouchingLayers(playerLayer))
        {
            Attack();
        }
        
        
       
    }

    public void setDpa(int dPA)
    {
        damagePerAttack = dPA;
    }

    IEnumerator Attack()
    {
        //Wait a full second before attacking to create an intermitten attack 
        yield return new WaitForSeconds(1.0f);

        //Looks for overlap with player layer in the attack range

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attack.position, attackRange, playerLayer);

        
        foreach (Collider2D p in hitPlayer)
        {
            p.GetComponent<PlayerHealth>().TakeDamage(damagePerAttack);
        }
        
       
       
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //GetComponent<AIPath>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
       
        
    }



}
