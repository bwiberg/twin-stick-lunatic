using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TopDownPlayerWeaponController : MonoBehaviour {
    [SerializeField] private Collider Ground;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform BulletSpawn;
    [SerializeField, Range(0, 10)] private float TimeBetweenShots = 1;

    private uint numFiredBullets;
    private float lastTimeFired;

    IEnumerator FireWeapon(uint bulletIndex) {
        if (Time.time - lastTimeFired > TimeBetweenShots) {
            var bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation) as GameObject;
            var particles = bullet.GetComponent<ParticleSystem>(); 
            particles.Play();

            lastTimeFired = Time.time;
            
            yield return new WaitForSeconds(particles.main.duration + 0.5f);
            
            Destroy(bullet);  
        }
    }
    
    private void Update() {
        LookAtMouse();
        if (Input.GetButton("Fire1")) {
            StartCoroutine(FireWeapon(numFiredBullets++));
        }
    }

    private void LookAtMouse() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (!Ground.Raycast(ray, out hit, 1000)) return;
        
        var lookat = hit.point;
        lookat.y = transform.position.y;
        transform.LookAt(lookat);
    }
}
