﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Tilemaps;
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
        Collider2D[] hitEnemiesFront = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitEnemiesBack = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitEnemiesLeft = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitEnemiesRight = Physics2D.OverlapCircleAll(attackFront.position, attackRange, enemyLayers);

        Collider2D[] hitWalls = Physics2D.OverlapCircleAll(attackFront.position, attackRange, wallLayers);

        foreach (Collider2D enemy in hitEnemiesFront)
        {
         
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D enemy in hitEnemiesBack)
        {
           // enemy.GetComponent<Enemy1>.TakeDamage(10);
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D enemy in hitEnemiesLeft)
        {
           // enemy.GetComponent<Enemy1>.TakeDamage(10);
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D enemy in hitEnemiesRight)
        {
           // enemy.GetComponent<Enemy1>.TakeDamage(10);
            UnityEngine.Debug.Log("You have hit " + enemy.name);
        }

        foreach (Collider2D wall in hitWalls)
        {
            UnityEngine.Debug.Log(wall);
        }
    }
}
