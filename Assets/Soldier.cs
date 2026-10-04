// Historical animation-layer setup experiment. The SoldierA class is distinct from Assets/Scripts/Soldier.cs.

using UnityEngine;
using System.Collections;

public class SoldierA : MonoBehaviour
{
	Animator Animator;

	void Start()
	{
		Animator = GetComponent<Animator>();

		for (int i = 1; i < Animator.layerCount; ++i)
			Animator.SetLayerWeight(i, 1);
	}
	
	void Update()
	{
	}
}
