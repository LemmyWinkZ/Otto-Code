using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GUIManager : MonoBehaviour {

	public Text orbText, rockText;

	public bool orbCountChange = false;
	public bool rockCountChange = false;
	public int orbCount = 0;
	public int rockCount = 0;

	public string platform;
	public Text platformText;

	//private bool mobileControlsToggled = false;
	//private bool mobileControlsInit = false;
	[SerializeField]
	private bool showMobileControls = false;
	[SerializeField]
	private bool showPlatformInfo = false;
	[SerializeField]
	private bool displayingPlatformText = false;
    [SerializeField]
	private bool textsInit = false;
	[SerializeField]
	private int health;

	[SerializeField]
	private GameObject[] hearts;
	[SerializeField]
	private Sprite[] heartSprites;
	private GameObject mobileControls;

	private LevelManager lm;
	private HealthManager hm;
	private PlayerManager pm;

	// Use this for initialization
	void Start () {
		lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		hm = GameObject.FindGameObjectWithTag("Health Manager").GetComponent<HealthManager> ();
		pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
		if (Application.platform.ToString() == "WindowsEditor" && !showMobileControls && lm.currentLevelIndex > 0) {
			GameObject.Find("MobileControls").SetActive(false);
		}
		health = hm.GetHealth ();

        orbText = GameObject.Find("Orb Text").GetComponent<Text>();
        rockText = GameObject.Find("Rock Text").GetComponent<Text>();
		platformText = GameObject.Find ("Platform Info").GetComponent<Text> ();
        orbText.text = orbCount.ToString();
        rockText.text = rockCount.ToString();

        if (lm.currentLevelIndex == 0)
            GameObject.Find ("Canvas").SetActive (false); 

		hearts = new GameObject[3];
    }

	void Update () {
		if (showPlatformInfo && !displayingPlatformText)
		{
			platformText.text = "Platform: " + Application.platform.ToString();
			displayingPlatformText = true;
			Debug.Log ("Showing Platform Info");
		} else if (!showPlatformInfo && displayingPlatformText) {
			platformText.text = "";
			displayingPlatformText = false;
			Debug.Log ("Hiding Platform Info");
		}

		if (hm.healthChange) {
			health = hm.GetHealth ();
		}

		if (lm.currentLevelIndex >= 1) {
			// init heart array
			GameObject.Find ("Canvas").SetActive (true);
			hearts[0] = GameObject.Find ("Health Bar 1");
			hearts[1] = GameObject.Find ("Health Bar 2");
			hearts[2] = GameObject.Find ("Health Bar 3");
		}

        if (orbText == null || rockText == null) {
            orbText = GameObject.Find("Orb Text").GetComponent<Text>();
            rockText = GameObject.Find("Rock Text").GetComponent<Text>();
        }

        orbText.text = orbCount.ToString();
        rockText.text = rockCount.ToString();

        DisplayHearts ();
	}

	#region Orb Count Methods

	public void UpdateOrbCount() {
        orbCount = pm.GetOrbCount();
	}

	#endregion

	#region Rock Count Methods

	public void UpdateRockCount()
	{
		rockCount = pm.GetRockCount();
	}

	#endregion

	#region Heart Count Methods

	public void UpdateHeartCount()
	{
		health = hm.GetHealth ();
	}

	public void SetHeartCount(int healthPoints)
	{
		health = healthPoints;
	}

	public void DisplayHearts() {
        switch (health)
        {
            case 0:
                hearts[0].GetComponent<Image>().sprite = heartSprites[0];
                hearts[1].GetComponent<Image>().sprite = heartSprites[0];
                hearts[2].GetComponent<Image>().sprite = heartSprites[0];
                break;

            case 1:
                hearts[0].GetComponent<Image>().sprite = heartSprites[1];
                hearts[1].GetComponent<Image>().sprite = heartSprites[0];
                hearts[2].GetComponent<Image>().sprite = heartSprites[0];
                break;

            case 2:
                hearts[0].GetComponent<Image>().sprite = heartSprites[2];
                hearts[1].GetComponent<Image>().sprite = heartSprites[0];
                hearts[2].GetComponent<Image>().sprite = heartSprites[0];
                break;

            case 3:
                hearts[0].GetComponent<Image>().sprite = heartSprites[2];
                hearts[1].GetComponent<Image>().sprite = heartSprites[1];
                hearts[2].GetComponent<Image>().sprite = heartSprites[0];
                break;

            case 4:
                hearts[0].GetComponent<Image>().sprite = heartSprites[2];
                hearts[1].GetComponent<Image>().sprite = heartSprites[2];
                hearts[2].GetComponent<Image>().sprite = heartSprites[0];
                break;

            case 5:
                hearts[0].GetComponent<Image>().sprite = heartSprites[2];
                hearts[1].GetComponent<Image>().sprite = heartSprites[2];
                hearts[2].GetComponent<Image>().sprite = heartSprites[1];
                break;
        }
	}

	#endregion
}