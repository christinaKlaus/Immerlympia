using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType{
		Hit, Jump, Punch, Steps
	}
public class SoundManager : MonoBehaviour {

	private AudioSource source;

	//public List<AudioClip> audioclips;

	public AudioClip hit;
	public AudioClip jump;
	public AudioClip punch;
	public AudioClip steps;
	void Start () {
		source = GetComponent<AudioSource>();
	}

	public void playClip(AudioClip clip){
		source.clip = clip;
		source.Play();
	}

	public void playClip(SoundType type){
		switch (type) {
			case (SoundType.Hit) :
				if(!source.isPlaying)
					source.clip = hit;
				break;
			case SoundType.Jump:
			if(!source.isPlaying)
					source.clip = jump;
				break;
			case SoundType.Punch:
				if(!source.isPlaying)
					source.clip = punch;
				break;
			case SoundType.Steps:
				if(!source.isPlaying)
					source.clip = steps;
				break;
			default:
				return;

		}
		source.Play();
	}
}
