using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartOrLoad : MonoBehaviour
{
    [SerializeField]
    string start;
    [SerializeField]
    string load;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().SetText(TutorialManager.level == 1 ? start : load);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
