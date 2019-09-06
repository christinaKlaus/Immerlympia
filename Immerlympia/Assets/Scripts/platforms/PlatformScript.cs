using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement.TweenSystem;
using Pixelplacement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlatformScript : MonoBehaviour {

    public AudioClip[] descendClips;
    public Vector2 lifetimeRange = new Vector2(10, 20);
    public float fallSpeed = 11f;
    public float warningDuration = 5f, disappearDuration = 5f;
    public Vector3 shakiness = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 descendTilt = new Vector3(0f, 0f, -45f);
    public float descendTiltTime = 2.5f;
    public float appearTime = 3f;

    public bool isMoving = true;
    
    [HideInInspector] public bool canFall = true;
    [HideInInspector] public int currentPosition = -1;

    
    private float timer;
    private float duration;
    private bool playedDescendSound = false;
    private bool coinSpawnsActive = false;
    private bool gameEnded = false;

    private static string platformShaderName = "Immerlympia/platforms";
    [SerializeField] Material[] platformMaterials;
    [SerializeField] ParticleSystem holoScanParticles;
    [SerializeField] ParticleSystem[] platformStyleParticles;

    private Vector3[] basePos;
    private AudioSource platformAudio;
    private CoinSpawnPoint[] spawns;
    private platformDirtParticleSystem[] platformEdgeDirt;
    private PlatformSpawn platformSpawn;
    private Coroutine shakeRoutine, deactivationRoutine;
    private TweenBase rotateTween, shakeTween;
    private WaitForSeconds warningDelay, disappearDelay;

    private NavMeshBakeControl navmeshControl;

    void Awake(){
        gameEnded = false;
        PlayerManager.startWinCamEvent += OnWinCamActivated;

        warningDelay = new WaitForSeconds(warningDuration + 0.5f);
        disappearDelay = new WaitForSeconds(disappearDuration + 0.5f);

        if(platformMaterials == null || platformMaterials.Length == 0){
            Debug.LogError("platform materials not set on " + gameObject.name, this);
        }
        //Debug.Log(platformMaterials[0].shader.name);
        navmeshControl = FindObjectOfType<NavMeshBakeControl>() as NavMeshBakeControl;
    }

    void OnEnable () {
        timer = Random.Range(lifetimeRange.x, lifetimeRange.y);
        duration = timer;

        if(platformSpawn == null)
            platformSpawn = GetComponentInParent<PlatformSpawn>();

        if(spawns == null)
            spawns = GetComponentsInChildren<CoinSpawnPoint>();

        if(platformEdgeDirt == null)
            platformEdgeDirt = GetComponentsInChildren<platformDirtParticleSystem>();
        
        if(descendClips.Length == 0) {
            Debug.Log("Platform prefab " + gameObject.name + " is missing descend sounds");
        }
        
        if(platformAudio == null) {
            platformAudio = GetComponent<AudioSource>();
        }

        if (basePos == null) {
            basePos = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                basePos[i] = transform.GetChild(i).localPosition;
        }

        platformAudio.volume = 1;

        foreach(ParticleSystem ps in platformStyleParticles){
            ps.Play(true);
        }

        Update();
    }

    void Update () {
        // Debug.Log("Platform " + gameObject.name + " update");
        if(!isMoving && !coinSpawnsActive){
            foreach(CoinSpawnPoint s in spawns){
                // Debug.Log("Spawn true gesetzt");
                s.canSpawnCoin = true;
            }
            coinSpawnsActive = true;
        }

        if(canFall)
            timer -= Time.deltaTime; // zählt Sekundenweise	

        //shake
        if (timer < warningDuration && canFall && !gameEnded) {
            if(shakeRoutine == null)
                shakeRoutine = StartCoroutine(ShakePlatform());
        }

        if(timer > 0) {
            //moving up
            transform.position = -Vector3.up * Mathf.Min(timer * -1 + duration - appearTime, 0) * Mathf.Min(timer * -1 + duration - appearTime, 0) * fallSpeed;  
        } else {
            //moving down
            transform.position = -Vector3.up * Mathf.Min(timer, 0) * Mathf.Min(timer, 0) * fallSpeed;
        }

        if (timer < -appearTime) {
            //platform fell down and needs to be deactivated
            DeactivatePlatform();
        }

        isMoving = (timer < duration - appearTime && timer > warningDuration) ? false : true;
        if(!isMoving && !platformSpawn.enablePlatformCycle){
            foreach(CoinSpawnPoint s in spawns){
                s.canSpawnCoin = true;
                coinSpawnsActive = true;
            }
            this.enabled = false;
            navmeshControl.UpdateNavmeshControl();
        }
        
    }

    public void StartDeactivation(){
        if(deactivationRoutine == null){
            deactivationRoutine = StartCoroutine(PlatformDeactivation());
        }
    }

    public void RevertDeactivation(){
        foreach(Material m in platformMaterials){
            m.renderQueue = m.shader.renderQueue;
            m.SetFloat("_AlphaStep", 1f);
            m.SetFloat("_HoloMaster", 0f);
            Vector4 glitchControl = m.GetVector("_GlitchControl");
            glitchControl.w = 0f;
            m.SetVector("_GlitchControl", glitchControl);
        }
    }

    IEnumerator ShakePlatform(){
        foreach(CoinSpawnPoint s in spawns){
            //Debug.Log("Spawn false gesetzt");
            s.canSpawnCoin = false;
            coinSpawnsActive = false;
        }

        if(gameEnded) yield break;

        // SetEdgeDirtPlaying(true);
        if (!playedDescendSound) {
            platformAudio.PlayOneShot(descendClips[(int)transform.rotation.y / 120]);
            playedDescendSound = true;
        }
        // Debug.Log("Platform " + gameObject.name + " starts tweens");
        shakeTween = Tween.Shake(transform, transform.position, shakiness, warningDuration, 0f, Tween.LoopType.None, null, null, true);
        rotateTween = Tween.Rotate(transform, descendTilt, Space.Self, descendTiltTime, warningDuration * 0.75f, Tween.EaseInOutBack, Tween.LoopType.None, null, null, true);

        yield return null;
        // while(gameObject.activeSelf){
        //     Vector3 offset = new Vector3(Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness));

        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         if(!transform.GetChild(i).name.Contains("edgeParticle"))
        //             transform.GetChild(i).localPosition = basePos[i] + offset;
        //     }
        // }
    }

    IEnumerator PlatformDeactivation(){
        foreach(ParticleSystem ps in platformStyleParticles){
            ps.Pause(true);
        }

        Debug.Log("Started deactivation of " + gameObject.name, this);

        holoScanParticles.Play(true);
        float fifth = warningDuration * 0.2f;
        Vector4 glitchControlSrc, glitchControlDst;
        foreach(Material m in platformMaterials){
            if(m.shader.name != platformShaderName) continue;
            else {
                m.renderQueue = 3001;
                m.SetFloat("_AlphaStep", 1f);
                Tween.ShaderFloat(m, "_HoloMaster",0 , 0.5f, fifth, 0f, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
                Tween.ShaderFloat(m, "_HoloMaster",0 , 0.5f, fifth, fifth * 4f, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
                glitchControlSrc = m.GetVector("_GlitchControl");
                glitchControlDst = new Vector4(glitchControlSrc.x, glitchControlSrc.y, glitchControlSrc.z, 1f);
                Tween.ShaderVector(m, "_GlitchControl", glitchControlSrc , glitchControlDst, fifth * 2f, 0f, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
            }
        }
        yield return warningDelay;
        Debug.Log("warning finished for " + gameObject.name, this);
        fifth = disappearDuration * 0.2f;
        foreach(ParticleSystem ps in platformStyleParticles){
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // holoScanParticles.Pause(true);
        holoScanParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        foreach(Material m in platformMaterials){
            if(m.shader.name != platformShaderName) continue;
            else {
                Tween.ShaderFloat(m, "_AlphaStep", 1, 0.9f, fifth, 0f, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
                Tween.ShaderFloat(m, "_AlphaStep", 0f, fifth * 4f, fifth, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
                glitchControlSrc = m.GetVector("_GlitchControl");
                glitchControlDst = new Vector4(glitchControlSrc.x, glitchControlSrc.y, glitchControlSrc.z, 0f);
                Tween.ShaderVector(m, "_GlitchControl", glitchControlSrc , glitchControlDst, fifth, 0f, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
            }
        }

        yield return disappearDelay;
        DeactivatePlatform();
        deactivationRoutine = null;
        Debug.Log("Finished deactivation of " + gameObject.name, this);
    }

    void DeactivatePlatform() {
        if(shakeRoutine != null){
            StopCoroutine(shakeRoutine);
            shakeRoutine = null;
        }
        if(deactivationRoutine != null){
            StopCoroutine(deactivationRoutine);
            deactivationRoutine = null;
        }
        transform.GetComponentInParent<PlatformSpawn>().newPlatform(currentPosition);
        currentPosition = -1;
        transform.position = new Vector3(0, -1000, 0);
        playedDescendSound = false;
        coinSpawnsActive = false;
        // SetEdgeDirtPlaying(false);
        gameObject.SetActive(false);
    }

    //if a platform's coin is picked up, it immediately starts descend procedures
    public void CoinPickedUp() {
        canFall = true;
        timer = warningDuration;
    }

    void OnWinCamActivated() {
        gameEnded = true;
        if(shakeTween != null) shakeTween.Cancel();
        if(rotateTween != null) rotateTween.Stop();
        // SetEdgeDirtPlaying(false);
        canFall = false;
        platformAudio.Stop();
        platformAudio.volume = 0;
    }

    void SetEdgeDirtPlaying(bool playState) {
        if(playState && (!platformEdgeDirt[0].IsPlaying() || !platformEdgeDirt[1].IsPlaying()))
        {
            platformEdgeDirt[0].PlayParticleSystem();
            platformEdgeDirt[1].PlayParticleSystem();
        } else if(!playState && (platformEdgeDirt[0].IsPlaying() || platformEdgeDirt[1].IsPlaying()))
        {
            platformEdgeDirt[0].StopParticleSystem();
            platformEdgeDirt[1].StopParticleSystem();
        }
    }

    void OnDestroy(){
        PlayerManager.startWinCamEvent -= OnWinCamActivated;
        RevertDeactivation();
    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(PlatformScript)), CanEditMultipleObjects]
public class PlatformScriptEditor : Editor {
    public override void OnInspectorGUI(){
        PlatformScript platformScript = serializedObject.targetObject as PlatformScript;
        DrawDefaultInspector();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("test deactivation")){
            platformScript.StartDeactivation();
        }
        if(GUILayout.Button("revert deactivation")){
            platformScript.RevertDeactivation();
        }
        GUILayout.EndHorizontal();
    } 
}
#endif
