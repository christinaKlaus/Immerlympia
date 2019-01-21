using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTargetArrow : MonoBehaviour
{

    public bool MarkerVisible {
        get { return (iconImage.enabled || arrowImage.enabled);}
    }

    [SerializeField] Camera gameCam;
    [SerializeField, ReadOnly(false)] MeshRenderer currentCoinRenderer;
    [SerializeField, ReadOnly(false)] CoinPickup currentCoin;
    [SerializeField] Image arrowImage, iconImage;
    

    bool markerVisible = false;
    Vector2 screenSize;
    Vector2 currentScreenPosition;
    Vector3 arrowRotation = Vector3.zero;

    void Awake(){
        // CoinPickup.coinSpawnEvent += RegisterRenderer;
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update() {
        if(MarkerVisible)
            Reposition();

        // if(Time.frameCount % 300 == 0){
        //     Debug.Log("Coin " + (currentCoin.gameObject.activeSelf ? "active" : "inactive"));
        //     Debug.Log("CoinRenderer " + (currentCoinRenderer.isVisible ? "visible" : "not visible"));
        //     Debug.Log("bounding sphere 0 " + (currentCoin.cullingGroup.IsVisible(0) ? "visible" : "not visible"));
        //     Debug.Log("Marker " + (MarkerVisible ? "visible" : "not visible"));
        // }
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
        // Debug.Log("culling group target cam: " + currentCoin.cullingGroup.targetCamera.name, currentCoin.cullingGroup.targetCamera);
        // Debug.Log("bounding sphere pos: " + currentCoin.boundingSphere.position, this);
        //Debug.Break();
        SwitchSprite(cullEvent.isVisible);
    }

    public void RegisterRenderer(CoinPickup coin){
        // Debug.Log("Renderer registered to target arrow for " + coin.name, this);
        currentCoin = coin;
        currentCoinRenderer = currentCoin.coinRenderer;
        currentCoin.cullingGroup.targetCamera = gameCam;
        currentCoin.cullingGroup.onStateChanged = OnCoinCullingStateChanged;
    }

    public void CoinSpawned(){
        SwitchSprite(currentCoin.cullingGroup.IsVisible(0));
    }

    public void CoinCollected(){
        SwitchSprite(null);
    }

    /// <summary>Show icon (true), arrow (false) or nothing (null) </summary>
    public void SwitchSprite(bool? showIcon){
        if(showIcon == null){
            iconImage.enabled = false;
            arrowImage.enabled = false;
        } else if((bool)showIcon){
            iconImage.enabled = true;
            arrowImage.enabled = false;
        } else {
            arrowImage.enabled = true;
            iconImage.enabled = false;
        }
    }

    void OnDestroy(){
        // CoinPickup.coinSpawnEvent -= RegisterRenderer;
    }

}
