using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PlayerSortingLayerFix : MonoBehaviour {

	public GameObject[] meshes;

	void Start () {
		for (int i = 0; i < meshes.Length; i++) {
			meshes [i].GetComponent<Renderer> ().sortingLayerName = "Cave Orbs";
			//meshes [i].GetComponent<Renderer> ().sortingLayerID = 2;
		}
	}
}
