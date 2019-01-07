using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSubEmitterLock : MonoBehaviour
{
    void Awake(){
        if(GetComponentsInChildren<ParticleSystem>().Length < 2) return;
        ParticleSystem parent = GetComponent<ParticleSystem>();
        ParticleSystem subEmitter = GetComponentsInChildren<ParticleSystem>()[1];
        ParticleSystem.SubEmittersModule subEmittersModule = parent.subEmitters;
        subEmittersModule.SetSubEmitterSystem(0, subEmitter);
    }

}
