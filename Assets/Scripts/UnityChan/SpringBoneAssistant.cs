using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoneAssistant : MonoBehaviour
{
    public UnityChan.SpringManager mSpringManager;

    public Vector3 mTargetBoneAxis = new Vector3(0f, 1f, 0f);

    public void MarkChildren()
    {
        //List<SpringBoneMarker> boneMarkers = new List<SpringBoneMarker>();

        // get spring bones set
        SpringBoneMarker[] boneMarkers = GetComponentsInChildren<SpringBoneMarker>();

        // mark children
        for (int i = 0; i < boneMarkers.Length; i++)
        {
            boneMarkers[i].MarkChildren();
        }

        // get again, will include children
        boneMarkers = GetComponentsInChildren<SpringBoneMarker>();
        List<UnityChan.SpringBone> springBones = new List<UnityChan.SpringBone> { };
        for (int i = 0; i < boneMarkers.Length; i++)
        {
            // add spring bone
            springBones.Add(boneMarkers[i].AddSpringBone());

            // set vector
            springBones[i].boneAxis = mTargetBoneAxis;

            // unmark object
            boneMarkers[i].UnmarkSelf();
        }

        mSpringManager.springBones = springBones.ToArray();
    }

    public void CleanUp()
    {
        UnityChan.SpringBone[] springBones = FindObjectsOfType<UnityChan.SpringBone>();
        List<GameObject> gameObjects = new List<GameObject>();
        
        for (int i = 0; i < springBones.Length; i++)
        {
            if (springBones[i].transform.parent.GetComponentInParent<UnityChan.SpringBone>() == null)
            {
                gameObjects.Add(springBones[i].gameObject);
            } 
        }
        
        for (int i = 0; i < springBones.Length; i++)
        {
            DestroyImmediate(springBones[i]);
        }
        
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].AddComponent<SpringBoneMarker>();
        }

    }
    
    public void CleanUpAll()
    {
        UnityChan.SpringBone[] springBones = FindObjectsOfType<UnityChan.SpringBone>();
        SpringBoneMarker[] boneMarkers = FindObjectsOfType<SpringBoneMarker>();

        for (int i = 0; i < boneMarkers.Length; i++)
        {
            DestroyImmediate(boneMarkers[i]);
        }
        
        for (int i = 0; i < springBones.Length; i++)
        {
            DestroyImmediate(springBones[i]);
        }

        

    }


}
