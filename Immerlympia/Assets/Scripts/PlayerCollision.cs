using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	public Vector2 forward = Vector2.zero;	
	public float yVelocity;	
	Rigidbody rigid;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		yVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        Vector3 direction = new Vector3(forward.x, yVelocity, forward.y) * Time.fixedDeltaTime;
		RaycastHit hit;
		bool hitBool = Physics.CapsuleCast(transform.position + (Vector3.up * 1.5f), transform.position + (Vector3.up * 0.5f), 0.4f, direction, out hit, 1f);
//		Debug.Log(direction);
		if(!hitBool){
			rigid.MovePosition(rigid.position + direction);
			
		}else{
			Vector3 hitNorm = hit.normal;
			Vector3 temp = Vector3.Cross(hitNorm, direction);
			Vector3 slideDir = Vector3.Cross(hitNorm, temp);
			float angle = Vector3.Angle(hitNorm, slideDir); 
			float slideLength = Mathf.Cos(angle) * direction.magnitude;
			slideDir = slideDir.normalized * slideLength;
			rigid.MovePosition(rigid.position + slideDir);
			yVelocity = 0;
		}

	}

}
