using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Custom/HeroPick", fileName="newHeroPick")]
public class HeroPick : ScriptableObject{
	public bool isPicked = false;
	public int currentPlayer = -1;
	public string heroName = "Hero";
	public Color heroColor = Color.white;
	public Sprite heroSprite;
	[Header("Materials")]
	public Material heroMaterial;
	public Material trailMaterial;
	public Material projectorCircleMaterial;

	
}