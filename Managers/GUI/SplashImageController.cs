using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplashImageController : MonoBehaviour {

    public GameObject cool, classy, nerdy;

	void Start () {
        int index = Random.Range(0, 3);

        if (index == 0) {
            cool.SetActive(true);
        } else if (index == 1) {
            classy.SetActive(true);
        } else if (index == 2) {
            nerdy.SetActive(true);
        }
	}
}
