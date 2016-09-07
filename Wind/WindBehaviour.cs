using UnityEngine;
using System.Collections;

public class WindBehaviour : MonoBehaviour
{
    public enum wDirection
    {
        Up,
        Right,
        Down,
        Left
    };

    [Header("Wind Options")]
	public bool enabled = true;
	public bool constant = true;
	[Space(5)]
	[Header("If Constant is false")]
	public float duration = 4;
	public float cooldown = 2;
	[SerializeField]
	private bool active = false;
	[SerializeField]
	private bool onCooldown = false;
	[Space(10)]
    public wDirection windDirection = wDirection.Up;
    [Range(0, 5)]
    public float strength = 2f;
    [Space(10)]
    public bool flipPlayer = false;

    [Header("Private Variables for debugging")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Vector3 playerVelocity;
    [SerializeField]
    private bool playerInWind;
    [SerializeField]
    private Camera mc;
    [SerializeField]
    private float defaultHeightSmoothing;
    [SerializeField]
    private bool playerFlipped = false;
	[SerializeField]
	private ParticleSystem ps;
	[SerializeField]
	private ParticleSystem.EmissionModule em;
	[SerializeField]
	private bool psStarted = false;
	[SerializeField]
	private Animation anim;
	[SerializeField]
	private bool windAnimation;
	[SerializeField]
	private AnimationClip[] clips;

    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        mc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        defaultHeightSmoothing = mc.GetComponent<CameraController>().heightSmoothing;
		ps = GetComponent<ParticleSystem> ();

		// Check if gameobject has a child
		if (transform.childCount > 0) {
			clips = new AnimationClip[2];
			windAnimation = true;
			anim = transform.GetChild (0).GetComponent<Animation> ();
			clips[0] = anim.GetClip ("Wind");
			clips [1] = anim.GetClip ("Wind_Idle");
		}
    }

    void Update()
    {
        if (playerInWind && enabled && active)
        {
            WindMovePlayer();
        }

		if (enabled && !active && !constant && !onCooldown) {
			StartCoroutine (PlayWind ());
		}
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerInWind = true;
            mc.GetComponent<CameraController>().heightSmoothing = 5f;
            if (flipPlayer && !playerFlipped)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player2DController>().FlipPlayer();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerInWind = false;
            StartCoroutine(RevertHeightSmoothing());
            if (playerFlipped && flipPlayer) {
                playerFlipped = false;
            }
        }
    }

    void WindMovePlayer()
    {
        switch (windDirection) {
            case wDirection.Up:
                rb.velocity = new Vector3(0, strength * 2, 0);
                break;

            case wDirection.Down:
                rb.velocity = new Vector3(0, -(strength * 2), 0);
                break;

            case wDirection.Left:
                rb.velocity = new Vector3(-(strength * 2), 0, 0);
                break;

            case wDirection.Right:
                rb.velocity = new Vector3(strength * 2, 0, 0);
                break;
        }
    }

	IEnumerator PlayWind() {
		psStarted = true;
		em = ps.emission;
		em.enabled = true;
		active = true;

		if (windAnimation) {
			anim.clip = clips [0];
			anim.Play ();
		}

		yield return new WaitForSeconds (duration);
		psStarted = false;
		em = ps.emission;
		em.enabled = false;
		active = false;
		onCooldown = true;

		if (windAnimation) {
			anim.Stop ();
		}

		yield return new WaitForSeconds (cooldown);
		if (windAnimation) {
			anim.clip = clips [1];
			anim.Play ();
		}
		onCooldown = false;
	}

    IEnumerator RevertHeightSmoothing() {
        yield return new WaitForSeconds(3f);
        mc.GetComponent<CameraController>().heightSmoothing = defaultHeightSmoothing;
    }
}
