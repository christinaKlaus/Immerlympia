using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class StandoffWarningPylons : MonoBehaviour
{

    [Tooltip("The height by which the pylons will be moved."), SerializeField] private float riseValue = 2.15f;
    [SerializeField] float timeToAscend = 1f, timeToDescend = 4f;
    private ParticleSystem[] particleSystems;
    private Vector3 originalPosition;
    private TweenBase riseTween;

    void Awake(){
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        originalPosition = transform.position;
        PlayerManager.startStandoffEvent += OnStandoffStarted;
        PlayerManager.startWinCamEvent += OnWinCamStarted;
    }

    public void OnStandoffStarted(){
        riseTween = Tween.Position(transform, transform.position + (Vector3.up * riseValue), timeToAscend, 0f, Tween.EaseOutStrong, Tween.LoopType.None, null, () => ToggleParticleSystems(true), false);
    }

    void ToggleParticleSystems(bool onOff){
        if(onOff){
            foreach(ParticleSystem ps in particleSystems){
                ps.Play(true);
            }
        } else {
            foreach(ParticleSystem ps in particleSystems){
                ps.Stop(true);
            }
        }
    }

    public void OnWinCamStarted(){
        riseTween = Tween.Position(transform, originalPosition, timeToDescend, 0f, Tween.EaseIn, Tween.LoopType.None, () => ToggleParticleSystems(false), null, false);
    }

    void OnDestroy(){
        PlayerManager.startStandoffEvent -= OnStandoffStarted;
        PlayerManager.startWinCamEvent -= OnWinCamStarted;
    }
}
