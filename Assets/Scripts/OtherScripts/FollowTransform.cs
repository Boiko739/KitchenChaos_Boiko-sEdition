using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform TargetTransform { get; set; }

    private void LateUpdate()
    {
        if (TargetTransform == null)
        {  
            return; 
        }

        transform.position = TargetTransform.position;
        transform.rotation = TargetTransform.rotation;
    }
}