using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LaserBeamScript : MonoBehaviour {

    bool beamActive = false;
    public bool BeamActive{
        get;
        private set;
    }

    public float hitDistance = 0f;
    [Space, SerializeField] int numLinePositions = 12;
    [SerializeField] float laserMaxDistance = 12f, laserBeamHeight = 2.15f, turnDuration = 20f, hitTurnSpeedMultiplier = 0.25f /* , cameraTargetMoveSpeed = 20f */;

    [SerializeField] Transform laserRotationTransform = null, laserBeamOrigin = null, cameraTargetTransform = null;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] Gradient hitGradient, noHitGradient;
    LineRenderer laserLine;
    // Transform laserTargetTransform, debugCubeTransform;
    CinemachineTargetGroup targetGroup;

    // Use this for initialization
    void Start () {
        if(!hitParticles) hitParticles = GetComponentInChildren<ParticleSystem>();
        laserLine = GetComponentInChildren<LineRenderer>();
        targetGroup = FindObjectOfType<CinemachineTargetGroup>();

        cameraTargetTransform.localPosition = new Vector3(0f, 0f, laserMaxDistance);
        // laserTargetTransform = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        // laserTargetTransform.SetParent(transform);
        // laserTargetTransform.name = "laserTargetTransform";
        // Destroy(laserTargetTransform.GetComponent<Collider>());

        // debugCubeTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        // debugCubeTransform.SetParent(transform);
        // debugCubeTransform.name = "debugCubeTransform";
        // Destroy(debugCubeTransform.GetComponent<Collider>());

        hitParticles.Stop(true);

        SetupLineRenderer();
	}

    public void Update(){
        if(Input.GetKeyDown(KeyCode.F2)){
            StartRotation(turnDuration, Vector3.forward);
        }
    }
	
    public void StartRotation(float turnDuration, Vector3 startLookAtPos)
    {
        if(!beamActive)
            StartCoroutine(FullLaserTurn(turnDuration, startLookAtPos));
    }

    void SetupLineRenderer(){
        // Debug.Log("posCount pre: " + laserLine.positionCount, laserLine);
        laserLine.positionCount = numLinePositions;
        // Debug.Log("posCount post: " + laserLine.positionCount, laserLine);
        float factor = 10f / numLinePositions;
        // Vector3[] positions = new Vector3[numLinePositions];
        for(int i = 0; i < numLinePositions; i++){
            laserLine.SetPosition(i, new Vector3(0f, 0f, factor * i));
        }
        laserLine.enabled = false;
    }

    IEnumerator FullLaserTurn(float turnDuration, Vector3 startLookAtPos){
        startLookAtPos.y = laserRotationTransform.position.y;
        float rotationTime = 0f, yRotationDelta = 360f / turnDuration;

        laserRotationTransform.LookAt(startLookAtPos, Vector3.up);
        laserRotationTransform.transform.position = new Vector3(0f, laserBeamHeight, 0f);

        RaycastHit hit;
        
        Transform lastHitTransform = null;

        beamActive = true;
        laserLine.colorGradient = noHitGradient;
        laserLine.enabled = true;

        targetGroup.AddMember(cameraTargetTransform, 1f, 1f);

        while(beamActive && rotationTime < turnDuration){
            if(Physics.Raycast(laserBeamOrigin.position, laserBeamOrigin.forward, out hit, 1000f)){
                laserRotationTransform.Rotate(0f, yRotationDelta * (Time.deltaTime * hitTurnSpeedMultiplier), 0f, Space.Self);

                if(hit.transform != lastHitTransform){
                    lastHitTransform = hit.transform;
                    Debug.Log(hit.transform.name, hit.transform);
                }

                laserLine.colorGradient = hitGradient;
                hitDistance = Vector3.Distance(laserBeamOrigin.position, hit.point);
                
                SetLaserDistance(hitDistance);

                hitParticles.transform.forward = hit.normal;
                hitParticles.transform.position = hit.point; 

                // laserTargetTransform.position = Vector3.MoveTowards(laserTargetTransform.position, hit.point, Time.deltaTime * cameraTargetMoveSpeed);
                // debugCubeTransform.position = hit.point;

                if(!hitParticles.isPlaying){
                    hitParticles.Play(true);
                }

                rotationTime += (Time.deltaTime * hitTurnSpeedMultiplier);
            } else {
                laserRotationTransform.Rotate(0f, yRotationDelta * Time.deltaTime, 0f, Space.Self);

                SetLaserDistance(laserMaxDistance);
                laserLine.colorGradient = noHitGradient;

                // laserTargetTransform.position = Vector3.MoveTowards(laserTargetTransform.position, laserLine.transform.TransformPoint(laserLine.GetLastPosition()), Time.deltaTime * cameraTargetMoveSpeed);

                if(hitParticles.isPlaying){
                    hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
                rotationTime += Time.deltaTime;
            }
            yield return null;
        }
        beamActive = false;
        hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        laserLine.enabled = false;
        laserRotationTransform.rotation = Quaternion.identity;
        targetGroup.RemoveMember(cameraTargetTransform);

        yield return null;
    }

    void SetLaserDistance(float hitDistance){
        for(int i = 0; i < numLinePositions; i++){
            laserLine.SetPosition(i, new Vector3(0f, 0f, (hitDistance / (numLinePositions - 1)) * i));
        }
    }

    public void StopLaser(){
        if(beamActive) 
            beamActive = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LaserBeamScript))]
public class LaserBeamScriptEditor : Editor {
    Vector3 startPos = new Vector3(-1, 0f, 0f);
    public override void OnInspectorGUI(){
        LaserBeamScript laser = serializedObject.targetObject as LaserBeamScript;
        DrawDefaultInspector();
        startPos = EditorGUILayout.Vector3Field("start lookAt pos", startPos);
        if(laser.BeamActive){
            if(GUILayout.Button("stop laser")){
                laser.StopLaser();
            }
        } else {
            if(GUILayout.Button("test laser")){
                laser.StartRotation(serializedObject.FindProperty("turnDuration").floatValue, startPos);
            }
        }
        
    }
}

#endif