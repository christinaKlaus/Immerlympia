using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FieldOfViewLock : MonoBehaviour
{
    public Camera fieldOfViewSource;
    [SerializeField, ReadOnly(false)] private Camera thisCam;
    
    public void OnEnable(){
        if(fieldOfViewSource == null) Destroy(this);
        else{ 
            thisCam = GetComponent<Camera>();
            thisCam.fieldOfView = fieldOfViewSource.fieldOfView;
        }
    }

    public void Update(){
        if(fieldOfViewSource.fieldOfView != thisCam.fieldOfView)
            thisCam.fieldOfView = fieldOfViewSource.fieldOfView;
    }
}
