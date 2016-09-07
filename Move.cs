using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour 
{
	public GameObject otto;

	void Update()
	{
		transform.Translate (0, 0, 1.5f * Time.deltaTime);
	}

}
