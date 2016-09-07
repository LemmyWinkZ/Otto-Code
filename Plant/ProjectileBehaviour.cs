using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour
{
	[HideInInspector]
	public float speed = 1f;
	[SerializeField]
	private PlantProjectileBehaviour.ProjectileDirection direction;
	public GameObject parent;

	private HealthManager hm;

	void Start() {
		hm = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		direction = parent.GetComponent<PlantProjectileBehaviour> ().pDirection;
	}

	void Update()
	{
		MoveProjectile();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player Collision w/ Plant Projectile Detected");
			hm.RemoveHealth ();
			Destroy(this.gameObject);
		}
		else if (col.gameObject.CompareTag("Ground"))
		{
			Destroy(this.gameObject);
		}
		else if (col.gameObject.CompareTag("Wall"))
		{
			Destroy(this.gameObject);
		}
	}
	
	void MoveProjectile()
	{
		switch (direction) {
		case PlantProjectileBehaviour.ProjectileDirection.Left:
			transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
			break;

		case PlantProjectileBehaviour.ProjectileDirection.Right:
			transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
			break;

		case PlantProjectileBehaviour.ProjectileDirection.Up:
			transform.Translate (Vector3.up * speed * Time.deltaTime, Space.World);
			break;

		case PlantProjectileBehaviour.ProjectileDirection.Down:
			transform.Translate (Vector3.down * speed * Time.deltaTime, Space.World);
			break;
		}

	}
}
