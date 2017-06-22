using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource source;

	public List<AudioClip> audioclips;
    public Dictionary<string, AudioClip> clipList;

	void Start () {
		source = GetComponent<AudioSource>();
		clipList = new Dictionary<string, AudioClip>();
		foreach(AudioClip a in audioclips)
			clipList.Add(a.name, a);
	}

	private AudioClip temp;
	public void playClip(string name){
		source.clip = clipList[name];
		source.Play();
	}

	public void playClip(AudioClip clip){
		source.clip = clip;
		source.Play();
	}

}
