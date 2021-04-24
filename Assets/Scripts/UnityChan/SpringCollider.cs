//
//SpringCollider for unity-chan!
//
//Original Script is here:
//ricopin / SpringCollider.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//

using System;
using UnityEngine;
using System.Collections;

namespace UnityChan
{
	public class SpringCollider : MonoBehaviour
	{
		//半径
		
		public float radius = 0.5f;
		public float scaleRadius = 0.06f;
		public bool dynamicRadius = true;
		
		private void OnValidate() {
			if (dynamicRadius) {
				radius = scaleRadius * (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3.0f;
			}
		}

		private void OnDrawGizmosSelected ()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (transform.position, radius);
		}
	}
}