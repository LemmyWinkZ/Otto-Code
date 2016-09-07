using UnityEngine;
using System.Collections;

public class PlantProjectileBehaviour : MonoBehaviour
{
    public enum ProjectileDirection
    {
        Left,
        Right,
        Up,
        Down
    };

    [Header("Projectile Options")]
    public float projectileSpeed = 2f;
    public float projectileFireRate = 2f;
	public float projectileRateOfFire;
	[Range(-2f, 2)]
	public float yOffset = 0f;
    public GameObject projectileObject;
	private GameObject projectileInst;
	public ProjectileDirection pDirection;
    public bool active = true;

	private Quaternion projectileDirection;


    [Header("Private Variables")]
    private float fireRateCount = 0f;

    void Update()
    {
        UpdateFireRateCount();
		projectileRateOfFire = Random.Range (projectileFireRate, projectileFireRate + 1);

		if (active && fireRateCount >= projectileRateOfFire)
        {
            StartCoroutine(FireProjectile());
        }
    }

    void UpdateFireRateCount()
    {
        fireRateCount += Time.deltaTime;
    }

    IEnumerator FireProjectile()
    {
        fireRateCount = 0;

		switch (pDirection) {
		case ProjectileDirection.Right:
			projectileDirection = Quaternion.Euler (0f, 0f, 0f);
			break;

		case ProjectileDirection.Left:
			projectileDirection = Quaternion.Euler (0f, 0f, 180f);
			break;

		case ProjectileDirection.Up:
			projectileDirection = Quaternion.Euler (0f, 0f, 90f);
			break;

		case ProjectileDirection.Down:
			projectileDirection = Quaternion.Euler (0f, 0f, 270f);
			break;
		}

		projectileInst = (GameObject)Instantiate(projectileObject, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z) , projectileDirection);
		projectileInst.transform.parent = transform;
		projectileInst.GetComponent<ProjectileBehaviour> ().parent = gameObject;
        projectileInst.GetComponent<ProjectileBehaviour> ().speed = projectileSpeed;

        yield return new WaitForSeconds(projectileFireRate);
    }
}
