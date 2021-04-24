using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LayerLab
{
    public class GameStartButton : MonoBehaviour
    {
        
        public void OnEnable()
        {
            Debug.Log("Enabled!");
           
        }

        public void OnDisable()
        {
        // GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().StartGame();
        }
    }
}
