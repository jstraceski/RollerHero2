
using UnityEngine;

public class LightBehavior : MonoBehaviour {
    public Light pointLight; 
    public Light spotLight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        pointLight.color = Color.Lerp(Color.cyan, Color.yellow, Mathf.PingPong(Time.time, 1));
        spotLight.intensity = 0.5f + Mathf.PingPong(Time.time, 2);
    }
}
