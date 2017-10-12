using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleSystemAfterComplete : CustomBehaviour {
    private void Start() {
        var ps = GetComponent<ParticleSystem>();

        var duration = ps.main.duration + ps.main.startLifetime.constantMax;
        Invoke("DestroySelf", duration);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
