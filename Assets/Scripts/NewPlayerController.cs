// Historical animation-controller experiment, despite the original "New" filename.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(Rigidbody))]
public class NewPlayerController : MonoBehaviour
{
	Animator animator;
	private AnimatorStateInfo currentBaseState;

	//static int idleState = Animator.StringToHash("Base Layer.NewIdle");
	static int idleState2 = Animator.StringToHash("Base Layer.NewIdle_001");

	void Start()
	{
		animator = GetComponent<Animator>();
		currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
	}
	
	void FixedUpdate()
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");
		animator.SetFloat("Speed", Mathf.Sqrt(h * h + v * v));

		//if (currentBaseState.nameHash == idleState)
			if (Input.GetKeyDown(KeyCode.G))
				animator.SetBool("RaiseHand", true);
		//Debug.Log(currentBaseState.nameHash);
		if (currentBaseState.nameHash == idleState2)
		{
			animator.SetBool("RaiseHand", false);
		}


	}
}
