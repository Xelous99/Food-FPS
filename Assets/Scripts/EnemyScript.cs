using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    public float health = 10f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage) {
        health -= damage;

        if (health <= 0) {
            die();
        }
    }

    private void die() {
        Destroy(gameObject);
    }
}
