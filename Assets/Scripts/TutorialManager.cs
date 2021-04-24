using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RestartLevel()
    {
        Debug.Log("?");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
    }
}
