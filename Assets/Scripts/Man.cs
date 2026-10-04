using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Man : MonoBehaviour
{
	void Start()
	{
		//rigidbody.centerOfMass = new Vector3(0, 0, 0);
		animation["Walk2"].wrapMode = WrapMode.Loop;
		animation["Walk2"].speed = 0.7f;
		animation.Play("Walk2");
	}
}
