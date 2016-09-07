using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

    [Header("Checkpoint Settings")]
    public Transform currentCheckpoint;
    public CameraController mc;
    public Player2DController pc;

    [SerializeField]
    private PlayerManager pm;
    private LevelManager lm;
	private HealthManager hm;
    private float defaultDistance;

    void Start()
    {
        mc = Camera.main.GetComponent<CameraController>();
        pc = GetComponent<Player2DController>();
        defaultDistance = mc.distance;
        pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        lm = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
		hm = GameObject.FindGameObjectWithTag ("Health Manager").GetComponent<HealthManager> ();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Kill Zone"))
        {
            transform.position = currentCheckpoint.position;
            mc.transform.position = transform.position;
            mc.transform.position = new Vector3(transform.position.x + mc.offset, transform.position.y + mc.height, transform.position.z + mc.distance);
			hm.RemoveHealth (2);

            if (!pc.facingRight) {
                pc.FlipPlayer();
            }

        }
        else if (col.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = col.gameObject.transform;
        }

        else if (col.gameObject.CompareTag("End of Level")) {
            lm.NextLevel();
        }
    }
}

