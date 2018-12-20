using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class XRayReplacement : MonoBehaviour
{
    [ReadOnly(false)]
    public Camera parent;

    [ReadOnly(false)]
    public Camera cam;
    public Shader XRayShader;

    void OnEnable()
    {
        parent = transform.parent.GetComponent<Camera>();
        cam = GetComponent<Camera>();
        cam.SetReplacementShader(XRayShader, "XRay");
    }

    public void Update(){
        if(parent.fieldOfView != cam.fieldOfView)
            cam.fieldOfView = parent.fieldOfView;
    }


}