﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerData : MonoBehaviour {

	[SerializeField] private Projector circleProjector = null;
	[SerializeField] private SkinnedMeshRenderer playerMesh = null;
	[SerializeField] private TrailRenderer trail = null;
	public CinemachineSmoothPath winCamTrack = null;


	public void SetupPlayerVisuals(HeroPick pickedHero){
		playerMesh.sharedMaterial = pickedHero.heroMaterial;
		
		float H, S, V;
		Color.RGBToHSV(pickedHero.heroColor, out H, out S, out V);
		// need to set edge color to full saturation and value for visibility
		playerMesh.sharedMaterial.SetColor("_EdgeColor", Color.HSVToRGB(H, 1, 1));
		playerMesh.sharedMaterial.SetColor("_OutlineColor", pickedHero.heroColor);

		pickedHero.trailMaterial.color = pickedHero.heroColor;
		pickedHero.trailMaterial.SetColor("_EmisColor", pickedHero.heroColor);

		trail.sharedMaterial = pickedHero.trailMaterial;

		circleProjector.material = pickedHero.projectorCircleMaterial;
		circleProjector.material.color = pickedHero.heroColor;
	}

}
