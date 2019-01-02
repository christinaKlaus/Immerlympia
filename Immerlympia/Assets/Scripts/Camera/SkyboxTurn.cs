using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))]
public class SkyboxTurn : MonoBehaviour
{
    [SerializeField] float turnSpeed = 1f;
    [SerializeField, Range(0f, 360f)] float startRotation = 0;
    float currentRotation;
    Material skyboxMaterial;
    void Awake(){
        skyboxMaterial = GetComponent<Skybox>().material;
        currentRotation = startRotation;
        skyboxMaterial.SetFloat("_Rotation", currentRotation);
    }

    void Update(){
        currentRotation += turnSpeed * Time.deltaTime;
        skyboxMaterial.SetFloat("_Rotation", currentRotation % 360);
    }
}
