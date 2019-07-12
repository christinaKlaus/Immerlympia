﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

	public static void SetBoolTrue(this Animator animator, string s){
		animator.SetBool(s, true);
	}

	public static void SetBoolFalse(this Animator animator, string s){
		animator.SetBool(s, false);
	}

	public static Vector3 GetLastPosition(this LineRenderer lineRenderer){
		return lineRenderer.GetPosition(lineRenderer.positionCount - 1);
	}
}
