using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    public delegate void CoinSpawnDelegate(CoinPickup thisPickup);
    public static event CoinSpawnDelegate coinSpawnEvent;

    [SerializeField] float cullingSphereRadius = 0.5f;

    int scoreIncrease = 1;
    AudioSource coinSoundSource;
    CoinSpawnManager coinSpawnManager;
    PlatformScript currentPlatform;
    public AudioClip spawn;
    [HideInInspector] public CullingGroup cullingGroup;
    [HideInInspector] public MeshRenderer coinRenderer;

    void Awake()
    {
        coinRenderer = GetComponentInChildren<MeshRenderer>();
        coinSpawnManager = GetComponentInParent<CoinSpawnManager>();
        currentPlatform = GetComponentInParent<PlatformScript>();
        if(coinSoundSource == null)
            coinSoundSource = GetComponent<AudioSource>();

        coinSoundSource.PlayOneShot(spawn);

        cullingGroup = new CullingGroup();
        BoundingSphere sphere = new BoundingSphere(coinRenderer.transform.position, cullingSphereRadius);
        cullingGroup.SetBoundingSpheres(new BoundingSphere[1]{sphere});
        cullingGroup.SetBoundingSphereCount(1);
    }

    void Start(){
         if(coinSpawnEvent != null) coinSpawnEvent(this);
    }

	// Update is called once per frame
	void Update () {
		if(transform.position.y < -20) {
            coinSpawnManager.coinActive = false;
            currentPlatform.canFall = true;
            Destroy(gameObject);
            //Debug.Log("Coin too low");
        }
	}

    void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;
        PlayerControlling pc = obj.GetComponent<PlayerControlling>();

        if(pc != null) {
            pc.GetComponent<SoundManager>().playClip(SoundType.Collect);
            pc.ChangeScore(scoreIncrease);
            Collect();
        }

    }

    public void Collect () {
        coinSpawnManager.ResetTimer();
        coinSpawnManager.coinActive = false;
        transform.parent.parent.GetComponent<PlatformScript>().CoinPickedUp();
        Destroy(gameObject);
    }

    void OnDestroy(){
        cullingGroup.Dispose();
        cullingGroup = null;
    }

    void OnDrawGizmosSelected(){
        if(coinRenderer == null) coinRenderer = GetComponentInChildren<MeshRenderer>();
        Gizmos.DrawWireSphere(coinRenderer.transform.position, cullingSphereRadius);
    }

    /*void PosCheck(){
        RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 22)){
            transform.position = hit.point + Vector3.up;
        }else{
            Die();
        }
    }*/
}
