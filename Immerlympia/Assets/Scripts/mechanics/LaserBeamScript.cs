using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LaserBeamScript : MonoBehaviour {

    bool beamActive = false;

    int numLinePositions;

    [SerializeField] Transform laserBeamOrigin = null;
    ParticleSystem hitParticles;
    LineRenderer laserLine;

    // Use this for initialization
    void Start () {
        hitParticles = GetComponentInChildren<ParticleSystem>();
        laserLine = GetComponentInChildren<LineRenderer>();
        numLinePositions = laserLine.positionCount;
        laserLine.enabled = false;

        hitParticles.Stop(true);
	}
	
    public void StartRotation(float turnDuration, Vector3 startLookAtPos)
    {
        if(!beamActive)
            StartCoroutine(FullLaserTurn(turnDuration, startLookAtPos));
    }

    IEnumerator FullLaserTurn(float turnDuration, Vector3 startLookAtPos){
        startLookAtPos.y = 0f;
        float time = 0f, yRotationStart = 0f, yRotationDelta = 360f / turnDuration;
        Debug.Log(turnDuration + " -> " + yRotationDelta);
        transform.LookAt(startLookAtPos, Vector3.up);
        yRotationStart = transform.eulerAngles.y;

        RaycastHit hit;

        beamActive = true;
        laserLine.enabled = true;

        for(; time < turnDuration; time = time + Time.deltaTime){
            transform.Rotate(0f, yRotationDelta * Time.deltaTime, 0f, Space.Self);
            if(Physics.Raycast(laserBeamOrigin.position, laserBeamOrigin.forward, out hit, 1000f)){
                laserLine.material.color = Color.red;
                float hitDistance = Vector3.Distance(laserBeamOrigin.position, hit.point);
                
                laserLine.SetPosition(numLinePositions - 1, new Vector3(0f, 0f, hitDistance));

                for(int i = 1; i < numLinePositions - 1; i++){
                    laserLine.SetPosition(i, new Vector3(0f, 0f, (hitDistance / numLinePositions) * i));
                }
            
                hitParticles.transform.forward = hit.normal;
                if(hitParticles.isPlaying){
                   hitParticles.transform.position = hit.point; 
                } else {
                    hitParticles.transform.position = hit.point;
                    hitParticles.Play(true);
                }

            } else {
                laserLine.material.color = Color.black;
                if(hitParticles.isPlaying){
                    hitParticles.Stop(true);
                }
            }

            yield return null;
        }
        beamActive = false;
        hitParticles.Stop(true);
        laserLine.enabled = false;
        yield return null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LaserBeamScript))]
public class LaserBeamScriptEditor : Editor {
    Vector3 startPos = new Vector3(-1, 0f, 0f);
    float turnDuration = 20f;
    public override void OnInspectorGUI(){
        LaserBeamScript laser = serializedObject.targetObject as LaserBeamScript;
        DrawDefaultInspector();
        turnDuration = EditorGUILayout.FloatField("turn duration", turnDuration);
        startPos = EditorGUILayout.Vector3Field("start pos", startPos);
        if(GUILayout.Button("test laser")){
            laser.StartRotation(turnDuration, startPos);
        }
    }
}

#endif