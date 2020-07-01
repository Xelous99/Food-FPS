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
    public GameObject projectile;

    [SerializeField]
    public float fireRate = 1f;
    private float fireWait = 0f;

    public bool IsEquipted { 
        get { return isEquipted; }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void equipt(GameObject player) {

        Transform playerCam = player.transform.Find("PlayerView").transform;
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
        if (Time.time >= fireWait) {
            Transform barrel = gameObject.transform.Find("Barrel").transform;
            Transform gun = gameObject.transform.Find("GunPos").transform;
            particles[0].Play();
            particles[1].Play();
            GameObject bullet = projectile;
            GameObject tempbullet = Instantiate(bullet, barrel.position, Quaternion.identity);
            tempbullet.transform.GetComponent<Projectile>().moveProjectile((barrel.position - gun.position).normalized);
            fireWait = Time.time + 1f / fireRate;
        }
    }
}
