using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

    public GameObject player;
    Text TextComponent;

    private void Start() {
        TextComponent = GetComponent<Text>();
        TextComponent.text = player.ToString() + ": ";
        TextComponent.text += "" + player.GetComponent<Playercontroller>().score;
    }

    // Update is called once per frame
    void Update () {

        TextComponent.text = player.ToString() + ": ";
        TextComponent.text += "" + player.GetComponent<Playercontroller>().score;
    }
}
