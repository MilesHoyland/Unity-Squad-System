using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] int health = 100;

    public bool IsDead { get { if (health > 0) return false; else return true; } }
    
    public void TakeDamage()
    {
        health -= 10;
    }
   
    void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
