using UnityEngine;
using System.Collections;

public class TempLevelManager : MonoBehaviour {

    public LevelManager lm;

    void Start() {
        lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();
    }
}
