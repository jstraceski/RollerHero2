using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectiveSliderBehavior : MonoBehaviour
{
    // Ordered objective Stage UI elements
    public GameObject[] objectiveStages;

    public GameObject griffenBody;
    public GameObject levelFinishedBanner;

    public Slider slider;
    public string levelName;

    public Sprite incompleteImage;
    public Sprite completeImage;
    public string nextLevel;

    public AudioClip objectiveCompleteSFX;

    public int objectiveCountMin = 1;
    public TriggerScript triggerScript;
    private bool doZoom = true;
    private float finishBannerDelay = 2f;
    
    int currObjective;
    public bool isKeyCollected;
    GameObject header;

    public string yoyoKey;

    // Start is called before the first frame update
    void Start()
    {
        header = GameObject.FindGameObjectWithTag("Player");
        //if (levelName == "Falling")
        griffenBody = GameObject.FindGameObjectWithTag("Player");
        levelFinishedBanner.SetActive(false);
       
        slider.maxValue = Mathf.Max(objectiveCountMin, objectiveStages.Length - 1);
        slider.value = 0;
        currObjective = 0;

        SetObjectiveAppearance();
    }

    // Check for objective completion
    void Update()
    {
        
        
        if (currObjective == 0)
        {
            CheckForFirstObjective();
        } else if (currObjective == 1)
        {
            CheckForSecondObjective();
        }
        
        if (triggerScript && triggerScript.flag) {
            triggerScript.flag = false;
            if (levelName == "Boss") {
                doZoom = false;
                finishBannerDelay = 10f;
            }
            CompleteObjective();
        }

        if (objectiveStages.Length <= currObjective) {
            FinishLevel();
        }

        // If more than 3 objectives are added for a level, add another case here
    }

    public void CompleteObjective()
    {

        AudioSource.PlayClipAtPoint(objectiveCompleteSFX, griffenBody.transform.position);

        if (currObjective >= objectiveStages.Length)
        {
            Debug.Log("?????");
            return;
        }
        
        GameObject targetObjective = objectiveStages[currObjective];
        targetObjective.GetComponent<Image>().sprite = completeImage;
        Debug.Log("Sider");
        slider.value = Mathf.Min(slider.maxValue, slider.value + 1);
        slider.Rebuild(CanvasUpdate.PostLayout);
        objectiveStages[currObjective].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = completeImage;

        currObjective++;
        SetObjectiveAppearance();

        if (objectiveStages.Length == currObjective) {
            levelFinishedBanner.SetActive(true);
            Destroy(levelFinishedBanner, finishBannerDelay);
        }

    }

    void SetObjectiveAppearance()
    {
      //  Debug.Log("Setting appearance of objectives");
      //  for (int ii=0; ii < objectiveStages.Length; ii++)
      //  {
      //      Debug.Log("ii=" + ii + ", currObjective=" + currObjective);
      //      if (ii  < currObjective)
     //       {
     //           Debug.Log("Objective completed, grey out text and set yellow in list");
                //objectiveStages[ii].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = completeImage;
                //objectiveStages[ii].transform.GetChild(0).GetChild(0).GetComponent<Text>().gameObject.SetActive(false);
     //       } 
    //        if (ii == currObjective) {
     //           Debug.Log("Attempting objective, set text visible and still blue in list");
     //           objectiveStages[ii].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = incompleteImage;
                //objectiveStages[ii].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().gameObject.SetActive(false);
    //        } else
    //        {
    //            Debug.Log("Objective not seen yet, grey out text and still blue in list");
     ///           objectiveStages[ii].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = incompleteImage;
                //objectiveStages[ii].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().gameObject.SetActive(true);
     //       }
    ///    }
    }

    /**
     * Calls CompleteObjective upon completion of the first objective for the following levels:
     * - Tutorial: Get to rock area
     * - Falling: 
     **/
    void CheckForFirstObjective()
    {
        if (levelName == "Tutorial")
        {
            GameObject targetRock = GameObject.FindGameObjectWithTag("TargetRock");
            float distanceToRock = Vector3.Distance(griffenBody.transform.position, targetRock.transform.position);
            if (distanceToRock <= 10)
            {
                CompleteObjective();
            }
        } else if (levelName == "Falling")
        {
            if (isKeyCollected)
            {
                CompleteObjective();
            }
        }
        else if (levelName == "PillarsOfKnowledge")
        {
            /*
            if (Input.GetKey(yoyoKey))
            {
                CompleteObjective();
            }
            */

            if (FindObjectOfType<CageBehavior>().cagemove)
            {
                Debug.Log("Pillar Objective");
                CompleteObjective();
            }
        } 
    }

    /**
     * Calls CompleteObjective upon completion of the first objective for the following levels:
     * - Tutorial: Get to rock area
     * - Falling: 
     **/
    void CheckForSecondObjective()
    {
        if (levelName == "Tutorial")
        {
            GameObject targetRock = GameObject.FindGameObjectWithTag("Plank");
            float distanceToRock = Vector3.Distance(griffenBody.transform.position, targetRock.transform.position);
            if (distanceToRock <= 10)
            {
                CompleteObjective();
            }
        }
        else if (levelName == "Falling")
        {
            GameObject targetDoor = GameObject.FindGameObjectWithTag("Exit");
            float distanceToDoor = Vector3.Distance(griffenBody.transform.position, targetDoor.transform.position);
            if (distanceToDoor <= 5)
            {
                CompleteObjective();
            }
        }
        else if (levelName == "PillarsOfKnowledge")
        {
            if (FindObjectOfType<BookTouch>().touchedBook)
            {
                CompleteObjective();
            }
        }
    }

    void LoadNextLevel() {
        SceneManager.LoadScene(nextLevel);
    }

    private bool loadingNextLevel = false;
    void FinishLevel()
    {
        if (doZoom) {
            // "Enter" the front door
            GameObject mainCamera = GameObject.FindGameObjectWithTag("CameraController");
            if (mainCamera != null && mainCamera.activeSelf) {
                CinemachineVirtualCamera camera = mainCamera.GetComponent<CinemachineVirtualCamera>();
                float fov = camera.m_Lens.FieldOfView;
                camera.m_Lens.FieldOfView = Mathf.Lerp(fov, 0, Time.deltaTime / 1.9f);
            }
        }

        if (!loadingNextLevel && nextLevel != null && nextLevel != "") {
            loadingNextLevel = true;
            Invoke("LoadNextLevel", 2);
        }
    }

    public void KeyCollected()
    {
        if (levelName == "Tutorial" || levelName == "Falling" || levelName == "PillarsOfKnowledge")
        {
            Debug.Log("KEY COLLECTED");
            isKeyCollected = true;
        }
    }
}
