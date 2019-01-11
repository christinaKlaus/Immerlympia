using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTargetArrow : MonoBehaviour
{

    [SerializeField] Camera gameCam;
    [SerializeField, ReadOnly(false)] MeshRenderer currentCoinRenderer;
    [SerializeField] Image arrowImage, iconImage;
    

    bool currentlyVisible = false;
    Vector2 screenSize;
    Vector2 currentScreenPosition;
    Vector3 arrowRotation = Vector3.zero;

    void Awake(){
        CoinPickup.coinSpawnEvent += RegisterRenderer;
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        if(currentCoinRenderer == null || !currentCoinRenderer.gameObject.activeSelf) {
            SwitchSprite(null);
            currentlyVisible = false;
            return;
        } else if (currentlyVisible) {
            Reposition();
        }
    }

    void Reposition(){
        currentScreenPosition = gameCam.WorldToScreenPoint(currentCoinRenderer.transform.position);

        currentScreenPosition.x = Mathf.Clamp(currentScreenPosition.x, 0f, screenSize.x);
        currentScreenPosition.y = Mathf.Clamp(currentScreenPosition.y, 0f, screenSize.y);

        transform.position = currentScreenPosition;

        arrowRotation.z = Vector2.SignedAngle(Vector2.up, currentScreenPosition - (screenSize * 0.5f));

        transform.localEulerAngles = arrowRotation;
    }

    void OnCoinCullingStateChanged(CullingGroupEvent cullEvent){
        // Debug.Log("Cull state of coin has changed");
        SwitchSprite(cullEvent.isVisible);
    }

    public void RegisterRenderer(CoinPickup coin){
        currentCoinRenderer = coin.coinRenderer;
        coin.cullingGroup.targetCamera = gameCam;
        coin.cullingGroup.onStateChanged = OnCoinCullingStateChanged;
        Reposition();
        SwitchSprite(coin.coinRenderer.isVisible);
    }

    /// <summary>Show icon (true), arrow (false) or nothing (null) </summary>
    void SwitchSprite(bool? showIcon){
        if(showIcon == null){
            iconImage.enabled = false;
            arrowImage.enabled = false;
        } else if((bool)showIcon){
            currentlyVisible = true;
            iconImage.enabled = true;
            arrowImage.enabled = false;
        } else {
            currentlyVisible = true;
            arrowImage.enabled = true;
            iconImage.enabled = false;
        }
    }

    void OnDestroy(){
        CoinPickup.coinSpawnEvent -= RegisterRenderer;
    }

}
