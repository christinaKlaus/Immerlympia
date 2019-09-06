using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))]
public class SkyboxTurn : MonoBehaviour
{
    [SerializeField] float turnSpeed = 1f;
    [SerializeField, Range(0f, 360f)] float startRotation = 0;
    [SerializeField] GameObject objectSky;
    float currentRotation, materialRotationAtStart;
    Material skyboxMaterial;
    void Awake(){
        skyboxMaterial = GetComponent<Skybox>().material;
        currentRotation = startRotation;
        materialRotationAtStart = skyboxMaterial.GetFloat("_Rotation");
        skyboxMaterial.SetFloat("_Rotation", currentRotation);
        if(objectSky == null)
            objectSky = new GameObject("objectSky empty");
    }

    void Update(){
        currentRotation += turnSpeed * Time.deltaTime;
        objectSky.transform.eulerAngles = new Vector3(0, currentRotation, 0);
        skyboxMaterial.SetFloat("_Rotation", currentRotation % 360);
    }

    void OnDestroy(){
        skyboxMaterial.SetFloat("_Rotation", materialRotationAtStart);
    }
}
