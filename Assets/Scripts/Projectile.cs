using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;

    [SerializeField]
     float projectileSpeed = 1f;


    [SerializeField]
    public float damage = 5f;

    [SerializeField]
    float LifeSpan = 3f;

    float lifespan = 0f;

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction * projectileSpeed * Time.deltaTime;
        ageProjectile();
    }

    private void ageProjectile() {
        lifespan += 1f * Time.deltaTime;
        if (lifespan >= LifeSpan) {
            Destroy(this.gameObject);
            Debug.Log("Dead");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Shootable")
        {
            Transform enemy = collision.transform;
            enemy.GetComponent<EnemyScript>().takeDamage(damage);
            Destroy(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        
    }

    public void moveProjectile(Vector3 projectileDir) {
        this.direction = projectileDir;
    }
}
