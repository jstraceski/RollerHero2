using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioClip waitingMusicSFX;
    GameObject mainCamera;
    public List<GameObject> menuObjs;
    public List<GameObject> creditsObjs;
    public List<GameObject> howToPlayObjs;
    public List<GameObject> volumeObjs;
    public GameObject currLevel;
    public GameObject currLevelText;

    string levelName;

    // Start is called before the first frame update
    void Start()
    {
        DetermineLevelName();
        Debug.Log("Starting screen loaded!");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        // AudioSource.PlayClipAtPoint(waitingMusicSFX, mainCamera.transform.position);
        mainCamera.GetComponent<AudioSource>().Play();
        StopCredits();
        StopHowToPlay();
        StopVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelName == "Tutorial")
        {
            currLevel.SetActive(false);
        } else
        {
            currLevel.SetActive(true);
        }
    }

    void DetermineLevelName()
    {
        int levelIndex = TutorialManager.level;
        switch (levelIndex)
        {
            case 1:
                levelName = "Tutorial";
                break;
            case 2:
                levelName = "The Mystical Yo";
                break;
            case 3:
                levelName = "Pillars of Knowledge";
                break;
            case 4:
                levelName = "Furniture Corridor";
                break;
            case 5:
                levelName = "Boss Fight";
                break;
        }
        currLevelText.GetComponent<TextMeshProUGUI>().SetText(levelName);
    }

    public void StartGame()
    {
        Debug.Log("Starting game!");
        SceneManager.LoadScene(TutorialManager.level, LoadSceneMode.Single);
    }

    public void StartCredits()
    {
        foreach (GameObject gmo in menuObjs)
        {
            gmo.SetActive(false);
        }
        foreach (GameObject gameo in creditsObjs)
        {
            gameo.SetActive(true);
        }
    }

    public void StopCredits()
    {
        foreach (GameObject gmo in menuObjs)
        {
            gmo.SetActive(true);
        }
        foreach (GameObject gameo in creditsObjs)
        {
            gameo.SetActive(false);
        }
    }

    public void StartVolume()
    {
        foreach (GameObject gmo in menuObjs)
        {
            gmo.SetActive(false);
        }
        foreach (GameObject gameo in volumeObjs)
        {
            gameo.SetActive(true);
        }
    }

    public void StopVolume()
    {
        foreach (GameObject gmo in menuObjs)
        {
            gmo.SetActive(true);
        }
        foreach (GameObject gameo in volumeObjs)
        {
            gameo.SetActive(false);
        }
    }

    public void StartHowToPlay()
    {
        foreach (GameObject gmo in menuObjs)
        {
            gmo.SetActive(false);
        }
        foreach (GameObject gameo in howToPlayObjs)
        {
            gameo.SetActive(true);
        }
    }

    public void StopHowToPlay()
    {
        foreach (GameObject gmo in menuObjs)
        {
            gmo.SetActive(true);
        }
        foreach (GameObject gameo in howToPlayObjs)
        {
            gameo.SetActive(false);
        }
    }
}
