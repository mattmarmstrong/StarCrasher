using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    //transforms to set attacks in each direction 

    public Transform attackFront;
    public Transform attackBack;
    public Transform attackRight;
    public Transform attackLeft;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

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
        Collider2D[] hitEnemiesFront = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitEnemiesBack = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitEnemiesLeft = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitEnemiesRight = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemiesFront)
        {
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D enemy in hitEnemiesBack)
        {
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D enemy in hitEnemiesLeft)
        {
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D enemy in hitEnemiesRight)
        {
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }


    }

}
