using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	[SerializeField]
    private int orbCount = 0;
    [SerializeField]
    private int rockCount = 0;

    [SerializeField]
    private GUIManager gm;
	//private HealthManager hm;

	void Awake () {
        DontDestroyOnLoad(transform.parent);
    }

    void Start() {
		gm = GameObject.Find ("GUI Manager").GetComponent<GUIManager> ();
		//hm = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
    }

    #region Getters

    public int GetOrbCount()
    {
        return orbCount;
    }

    public int GetRockCount() {
        return rockCount;
    }

    #endregion

    #region Adders

    public void AddOrb()
    {
        orbCount++;
        gm.UpdateOrbCount();
    }

    public void AddOrbs(int amount)
    {
        orbCount += amount;
        gm.UpdateOrbCount();
    }

    public void AddRock() {
        rockCount++;
        gm.UpdateRockCount();
    }

    public void AddRocks(int amount) {
        rockCount += amount;
        gm.UpdateRockCount();
    }

    #endregion

    #region removers

    public void RemoveOrb() {
        orbCount--;
        gm.UpdateOrbCount();
    }

    public void RemoveOrbs(int amount) {
        if (orbCount - amount < 0)
        {
            orbCount = 0;
        }
        else {
            orbCount -= amount;
        }
        gm.UpdateOrbCount();
    }

    public void RemoveAllOrbs() {
        orbCount = 0;
        gm.UpdateOrbCount();
    }

    public void RemoveRock() {
        rockCount--;
        gm.UpdateRockCount();
    }

    public void RemoveRocks(int amount) {
        if (rockCount - amount < 0)
        {
            rockCount = 0;
        }
        else {
            rockCount -= amount;
        }
        gm.UpdateRockCount();
    }

    public void RemoveAllRocks() {
        rockCount = 0;
        gm.UpdateRockCount();
    }

    #endregion

}
