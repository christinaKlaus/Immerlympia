using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformDirtParticleSystem : MonoBehaviour {

    ParticleSystem particleSys;

	// Use this for initialization
	void Start () {

        particleSys = GetComponent<ParticleSystem>();

        if(particleSys == null)
        {
            Debug.Log("No Particle System found on child of platform " + transform.parent.name);
        }

        particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}
	
	public void PlayParticleSystem()
    {
        particleSys.Play(true);
    }

    public void StopParticleSystem()
    {
        particleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public bool IsPlaying()
    {
        if(particleSys == null) particleSys = GetComponent<ParticleSystem>();
        return particleSys.isPlaying;
    }
}
