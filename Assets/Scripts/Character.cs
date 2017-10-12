using UnityEngine;

public class Character : CustomBehaviour {
    [SerializeField, Range(1, 100)] private float MaxHealth;

    public float Health { get; private set; }

    public void Damage(float damage) {
        Health -= damage;
        if (Health < 0) {
            Kill();
        }
    }

    private void Kill() {
        Destroy(gameObject);
    }

    private void Start() {
        Health = MaxHealth;
    }

    private void OnGUI() {
        if (tag == "Player") {
            GUILayout.Label("Player health: " + Health);
        }
    }
}
