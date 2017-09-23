using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType{
		Hit, Jump, Punch, Steps, DoubleJump, Collect
}
public class SoundManager : MonoBehaviour {

	private AudioSource source;

	//public List<AudioClip> audioclips;

	public AudioClip steps; //steps clip is different for all four players
    public AudioClip punch;
    public AudioClip hit;
    public AudioClip jump;
    public AudioClip doubleJump;
    public AudioClip coinCollect;

    //public float defaultVolume;

    void Start () {
		source = GetComponent<AudioSource>();
	}

	public void playClip(AudioClip clip){
		source.clip = clip;
		source.Play();
	}

	/*public void startFootsteps(){
		InvokeRepeating("playFootsteps", 0.0f, 0.3f);
	}

	public void playFootsteps(){
		source.PlayOneShot(steps, 0.5f);
	}

	public void stopFootsteps(){
		CancelInvoke("playFootsteps");
	}*/

	public void playClip(SoundType type){
        source.pitch = Random.Range(0.85f, 1.15f);
        //source.volume = defaultVolume;
		switch (type) {
			case (SoundType.Hit) :
                //	if(!source.isPlaying)
                source.PlayOneShot(hit);
				break;
			case SoundType.Jump:
                //if(!source.isPlaying)
                source.PlayOneShot(jump);
				break;
			case SoundType.Punch:
                //	if(!source.isPlaying)
                source.PlayOneShot(punch);
				break;
			case SoundType.Steps:
                //	if(!source.isPlaying)
                source.PlayOneShot(steps);
				break;
            case SoundType.DoubleJump:
                source.PlayOneShot(doubleJump);
                break;
            case SoundType.Collect:
                source.PlayOneShot(coinCollect);
                break;
			default:
				return;

		}
		
	}
}
