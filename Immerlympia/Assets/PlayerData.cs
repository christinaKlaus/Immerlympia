using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	[SerializeField] private Projector circleProjector;
	[SerializeField] private SkinnedMeshRenderer playerMesh;
	[SerializeField] private TrailRenderer trail;


	public void SetupPlayerVisuals(HeroPick pickedHero){
		playerMesh.material = pickedHero.heroMaterial;
		
		float H, S, V;
		Color.RGBToHSV(pickedHero.heroColor, out H, out S, out V);
		// need to set edge color to full saturation and value for visibility
		playerMesh.material.SetColor("_EdgeColor", Color.HSVToRGB(H, 1, 1));

		pickedHero.trailMaterial.color = pickedHero.heroColor;
		pickedHero.trailMaterial.SetColor("_EmisColor", pickedHero.heroColor);
		trail.material = pickedHero.trailMaterial;

		circleProjector.material = pickedHero.projectorCircleMaterial;
		circleProjector.material.color = pickedHero.heroColor;
	}

}
