using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GunScript : MonoBehaviour
{

    //General Variables
    private bool isEquipted = false;

    public ParticleSystem[] particles;

    //Gun Variables
    [SerializeField]
    public float damage = 5f;

    Vector3 scale;

    public bool IsEquipted { 
        get { return isEquipted; } 
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void equipt(GameObject player) {

        Transform playerCam = player.transform.Find("PlayerView").transform;
        scale = gameObject.transform.localScale;
        gameObject.transform.localPosition = playerCam.GetComponent<Camera>()
            .transform.Find("Grip").transform.position;
        
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        gameObject.transform.SetParent(playerCam.transform, true);
        gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

    }

    public void switchWeapon(bool equipt) {
        isEquipted = equipt;
        gameObject.SetActive(equipt);
    }

    public void dropWeapon() {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.SetParent(null, true);
        isEquipted = false;
    }

    public void shoot() {
        Transform barrel = gameObject.transform.Find("Barrel").transform;
        particles[0].Play();
        particles[1].Play();
        RaycastHit target;
        if (Physics.Raycast(barrel.position, barrel.forward, out target, 5f)) {
            EnemyScript enemy = target.transform.GetComponent<EnemyScript>();
            enemy.takeDamage(damage);
        }
    }
}
