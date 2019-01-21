using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType{
		Hit, Jump, Swing, Steps, DoubleJump, Death
}
public class SoundManager : MonoBehaviour {

	private AudioSource source;

	public AudioClip steps; //steps clip is different for all four players
    public AudioClip swing;
    public AudioClip hit;
    public AudioClip jump;
    public AudioClip doubleJump;
    public AudioClip coinCollect;
    public AudioClip death;

    void Start () {
		source = GetComponent<AudioSource>();
	}

	public void playClip(AudioClip clip){
		source.clip = clip;
		source.Play();
	}

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
			case SoundType.Swing:
                //	if(!source.isPlaying)
                source.PlayOneShot(swing);
				break;
			case SoundType.Steps:
                //	if(!source.isPlaying)
                source.PlayOneShot(steps, 0.33f);
				break;
            case SoundType.DoubleJump:
                source.PlayOneShot(doubleJump);
                break;
            case SoundType.Death:
                    GameObject tempAudioSource = new GameObject();
                    AudioSource deathS = tempAudioSource.AddComponent<AudioSource>();
                    deathS.maxDistance = source.maxDistance;
                    deathS.minDistance = source.minDistance;
                    deathS.spatialBlend = source.spatialBlend;
                    tempAudioSource.transform.position = gameObject.transform.position;
                    deathS.outputAudioMixerGroup = source.outputAudioMixerGroup;
                    deathS.PlayOneShot(death, 0.75f);
                    Destroy(tempAudioSource, death.length + 1.0f);
                break;
			default:
				return;

		}
		
	}
}
