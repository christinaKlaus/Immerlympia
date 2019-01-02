using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackWave : MonoBehaviour {

    CapsuleCollider capsCol;
    ParticleSystem particleSys;
    ParticleSystem.TriggerModule trigMod;
    //List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    float minRadius = 4.0f;
    float timer = 0;
    string immunePlayer = "";
    string playersHit = "";

    public AnimationCurve growthRate;
    public bool waveRolling = false;
    public float maxRadius;
    public float journeyTime;

    
    


	// Use this for initialization
	void Start () {
        particleSys = GetComponent<ParticleSystem>();
        capsCol = GetComponent<CapsuleCollider>();
        capsCol.radius = minRadius;
        capsCol.enabled = false;
        ParticleSystem.VelocityOverLifetimeModule waveVelocity = particleSys.velocityOverLifetime;
        ParticleSystem.MinMaxCurve velocityRate = new ParticleSystem.MinMaxCurve(1, growthRate);
        waveVelocity.x = velocityRate;
        waveVelocity.y = velocityRate;
        waveVelocity.z = velocityRate;
        trigMod = particleSys.trigger;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (capsCol.radius < maxRadius && waveRolling)
        {
            timer += Time.deltaTime;
            //float radiusDiff = Mathf.Lerp(capsCol.radius, maxRadius, timer / journeyTime) * (1 + growthRate.Evaluate(timer/journeyTime));
            capsCol.radius += maxRadius * growthRate.Evaluate(timer / journeyTime);
            //capsCol.radius = radiusDiff;
            //gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(1 + growthRate.Evaluate(timer / journeyTime), 1, 1 + growthRate.Evaluate(timer / journeyTime)));
        } else
        {
            trigMod.enabled = false;
            waveRolling = false;
            capsCol.radius = minRadius;
            capsCol.enabled = false;
            immunePlayer = "";
            playersHit = "";
            //gameObject.transform.localScale = Vector3.one;
        }
	}

    public void StartWave(string playerToIgnore)
    {
        immunePlayer = playerToIgnore;
        capsCol.enabled = true;
        waveRolling = true;
        timer = 0;
        particleSys.Play();
        trigMod.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider otherObject = other;
        string otherName = otherObject.name;

        if(otherName != immunePlayer && !otherObject.GetComponent<Animator>().GetBool("isJumping"))
        {
            if (!playersHit.Contains(otherName))
            {
                otherObject.GetComponent<Dummy>().DamageByWave();
                playersHit += otherName;
                Debug.Log(otherName + " at " + (int)Time.time);
                
            } else
            {
                Debug.Log(otherName + " already hit during this wave!");
            }
            
        }
           

    }

}
