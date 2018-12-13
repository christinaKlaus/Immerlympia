using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute {
	public bool activatable;
	public ReadOnlyAttribute(bool canBeActivated){
		activatable = canBeActivated;
	}
}