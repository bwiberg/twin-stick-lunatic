using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TopDownPlayerWeaponController : MonoBehaviour {
    [SerializeField] private Collider Ground;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform BulletSpawn;
    [SerializeField, Range(0, 10)] private float TimeBetweenShots = 1;

    private uint numFiredBullets = 0;
    private float lastTimeFired = 0.0f;

    IEnumerator FireWeapon(uint bulletIndex) {
        if (Time.time - lastTimeFired > TimeBetweenShots) {
            GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation) as GameObject;
            ParticleSystem particles = bullet.GetComponent<ParticleSystem>(); 
            particles.Play();
            Debug.LogFormat("Bullet #{0} instantiated.", bulletIndex, particles.main.loop);

            lastTimeFired = Time.time;
            
            yield return new WaitForSeconds(particles.main.duration + 0.5f);
            
            Destroy(bullet);
            Debug.LogFormat("Bullet #{0} destroyed.", bulletIndex);  
        }
    }
    
    void Update() {
        LookAtMouse();
        if (Input.GetButtonDown("Fire1")) {
            StartCoroutine(FireWeapon(numFiredBullets++));
        }
    }

    private void LookAtMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Ground.Raycast(ray, out hit, 1000)) {
            transform.LookAt(hit.point);
        }
    }
}