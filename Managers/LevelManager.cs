using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public int currentLevelIndex;

    public bool levelChangeDetected = false;
    public Scene scene;

    public bool levelControls = false;

    void Awake() {
        scene = SceneManager.GetActiveScene();
        currentLevelIndex = scene.buildIndex;
    }

    void Start()
    {
        StartCoroutine(LoadMainMenu());
    }

    void Update()
    {
        if (levelChangeDetected)
        {
            UpdateCurrentLevelIndex();
			GameObject.Find ("Health Manager").GetComponent<HealthManager> ().initialisedPlayerMesh = false;
            levelChangeDetected = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && levelControls)
        {
            LoadLevel(1);
			LevelChanged();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && levelControls)
        {
            LoadLevel(2);
			LevelChanged();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && levelControls)
        {
            LoadLevel(3);
			LevelChanged();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && levelControls)
        {
            LoadLevel(4);
			LevelChanged();
        }
    }

    void UpdateCurrentLevelIndex() {
        scene = SceneManager.GetActiveScene();
        currentLevelIndex = scene.buildIndex;
    }

    public void LevelChanged() {
        levelChangeDetected = true;
		GameObject.Find ("Health Manager").GetComponent<HealthManager> ().initialisedPlayerMesh = false;
    }

    public void NextLevel() {
        UpdateCurrentLevelIndex();

		LoadLevel(currentLevelIndex + 1);
		LevelChanged();
    }

    public void PrevLevel() {
        UpdateCurrentLevelIndex();
        if (currentLevelIndex > 1) {
            LoadLevel(currentLevelIndex - 1);
            LevelChanged();
        }
    }

    public void LoadLevel(int index) {
        SceneManager.LoadScene(index);
        LevelChanged();
    }

    public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
        LevelChanged();
    }

    public void ReloadLevel() {
        UpdateCurrentLevelIndex();
        SceneManager.LoadScene(currentLevelIndex);
		LevelChanged();
    }

    public void RestartGame()
    {
        LoadLevel(1);
		LevelChanged();
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(2f);
        levelControls = true;
        LoadLevel(1);
        LevelChanged();
    }
}
