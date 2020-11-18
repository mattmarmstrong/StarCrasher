using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    //transforms to set attacks in each direction 

    public Transform attack;
    
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask wallLayers;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack();
        }
        
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        //create an array to store which enemies were hit 
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //enemy.GetComponent<EnemyCombat>.TakeDamage(20);         
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        

        foreach (Collider2D wall in hitWalls)
        {
            UnityEngine.Debug.Log(wall);
        }
    }
}

