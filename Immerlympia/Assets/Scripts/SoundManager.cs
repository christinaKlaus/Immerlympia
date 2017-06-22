using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource source;
	public List<AudioClip> clipList;

	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	public void playClip(AudioClip clip){
		source.clip = clip;
		source.Play();
	}

}
