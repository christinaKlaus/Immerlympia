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

        particleSys.Stop();
	}
	
	public void PlayParticleSystem()
    {
        particleSys.Play();
    }

    public void StopParticleSystem()
    {
        particleSys.Stop();
    }

    public bool IsPlaying()
    {
        return particleSys.isPlaying;
    }
}
