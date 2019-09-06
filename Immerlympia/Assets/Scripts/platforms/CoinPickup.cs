using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoinPickup : MonoBehaviour {

    // public delegate void CoinSpawnDelegate(CoinPickup thisPickup);
    // public static event CoinSpawnDelegate coinSpawnEvent;

    [SerializeField] float cullingSphereRadius = 0.5f;
    [SerializeField] ParticleSystem coinParticles;

    int scoreIncrease = 1;
    bool registered = false;
    CoinSpawnManager coinSpawnManager;
    PlatformScript currentPlatform;
    CoinSpawnPoint currentSpawnPoint;
    ScreenTargetArrow screenTargetArrow;
    public BoundingSphere boundingSphere;
    public CullingGroup cullingGroup;
    public MeshRenderer coinRenderer;

    void Awake() {
        coinRenderer = GetComponentInChildren<MeshRenderer>();
        coinSpawnManager = GetComponentInParent<CoinSpawnManager>();

        screenTargetArrow = FindObjectOfType<ScreenTargetArrow>();

        cullingGroup = new CullingGroup();
        cullingGroup.SetBoundingSpheres(new BoundingSphere[1]{new BoundingSphere()});
        cullingGroup.SetBoundingSphereCount(1);
        if(!coinParticles.isPlaying) coinParticles.Play(true);
    }

    public void Start(){
        if(!coinParticles.isPlaying) coinParticles.Play(true);
    }

    public void OnEnable(){
        if(!coinParticles.isPlaying) coinParticles.Play(true);
    }

    public void Setup(PlatformScript platformScript, CoinSpawnPoint spawnPoint){
        if(!registered) registered = screenTargetArrow.RegisterRenderer(this);

        Debug.Log("Setup called on coin", this);
        cullingGroup.enabled = true;
        cullingGroup.SetBoundingSpheres(new BoundingSphere[1]{new BoundingSphere(coinRenderer.transform.position, cullingSphereRadius)});

        currentPlatform = platformScript;
        currentSpawnPoint = spawnPoint;

        screenTargetArrow.CoinSpawned();

        if(!coinParticles.isPlaying) coinParticles.Play(true);

        Invoke("PlayParticles", 0.5f);

        AgentTesting.SetDestination(transform.position);
    }

    public void PlayParticles(){
        coinParticles.Stop(true);
        coinParticles.Play(true);
    }

    void OnTriggerEnter(Collider collision) {
        Debug.Log("Something collided with coin");
        GameObject obj = collision.gameObject;
        PlayerControlling pc = obj.GetComponent<PlayerControlling>();

        if(pc != null) {
            pc.ChangeScore(scoreIncrease);
            currentSpawnPoint.CoinCollected();
            Collect();
        } else {
            NavMeshAgent agent = obj.transform.parent.GetComponent<NavMeshAgent>();
            if(agent != null){
                currentSpawnPoint.CoinCollected();
                Collect();
            }
        }
    }

    public void Collect () {
        cullingGroup.enabled = false;
        coinSpawnManager.ResetTimer();
        coinSpawnManager.coinActive = false;
        transform.parent.parent.GetComponent<PlatformScript>().CoinPickedUp();
        transform.parent = null;
        gameObject.SetActive(false);
        screenTargetArrow.CoinCollected();
    }

    void OnDestroy(){
        cullingGroup.Dispose();
        cullingGroup = null;
    }

    void OnDrawGizmos(){
        if(coinRenderer == null) coinRenderer = GetComponentInChildren<MeshRenderer>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(coinRenderer.transform.position, 1);
        Gizmos.DrawSphere(boundingSphere.position, cullingSphereRadius * 10f);
        // Gizmos.DrawSphere(coinRenderer.transform.position, cullingSphereRadius);
    }

#if UNITY_EDITOR

    [MenuItem("Tools/Collect current coin")]
    public static void CollectCurrentCoin(){
        CoinPickup coinPickup = FindObjectOfType(typeof(CoinPickup)) as CoinPickup;
        if(coinPickup != null){
            coinPickup.currentSpawnPoint.CoinCollected();
            coinPickup.Collect();
        }
    }

#endif
}


