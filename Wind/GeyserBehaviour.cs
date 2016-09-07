using UnityEngine;
using System.Collections;

public class GeyserBehaviour : MonoBehaviour {

    private ParticleSystem ps;
    private PlayerManager pm;
    private ParticleSystem.EmissionModule em;
    private Player2DController pc;

    private bool geyserBlocked = false;

    void Start() {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<Player2DController>();
        pm = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
        em.enabled = true;
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player") && pm.GetRockCount() > 0) {
            geyserBlocked = true;
            em.enabled = false;
            transform.parent.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            pm.RemoveRock();

        } else if (col.gameObject.CompareTag("Player") && pm.GetRockCount() <= 0 && !geyserBlocked) {
            // Call Damage & Knockback functions
            col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 5, ForceMode.Impulse);
        }
    }
}
