using UnityEngine;
using System.Collections;
 
public class RunAndCrouch : MonoBehaviour 
{
	public float walkSpeed = 7; // regular speed
	public float crouchSpeed = 3; // crouching speed
	public float proneSpeed = 1;
	public float runSpeed = 15; // run speed

	public delegate void StateChangedHandler(State newState, State oldState);
	public event StateChangedHandler OnStateChanged;

	public enum State
	{
		None, Running, Crouching, Proning, Jumping
	}
	public State state;

	CharacterController ch;
	TransformAnimation cameraAnimation;

	// Use this for initialization
	void Start()
	{
		ch = GetComponent<CharacterController>();

		cameraAnimation = Camera.main.gameObject.AddComponent<TransformAnimation>();
		cameraAnimation.Add("Stand", new TransformAnimation.Transform(Camera.main.transform.localPosition, 0.2f));
		cameraAnimation.Add("Crouch", new TransformAnimation.Transform(Vector3.up * 0.1f, 0.2f));
		cameraAnimation.Add("Prone", new TransformAnimation.Transform(Vector3.up * -0.5f, 0.2f));
	}

	void ChangeState(State newState)
	{
		if (this.state == newState)
			return;

		var oldState = this.state;
		float speed;

		this.state = newState;
		switch (newState)
		{
			case State.None: speed = walkSpeed; cameraAnimation.Play("Stand"); break;
			case State.Running: speed = runSpeed; cameraAnimation.Play("Stand"); break;
			case State.Crouching: speed = crouchSpeed; cameraAnimation.Play("Crouch"); break;
			case State.Proning: speed = proneSpeed; cameraAnimation.Play("Prone"); break;
			default: throw new System.NotImplementedException();
		}

        //chMotor.jumping.enabled = state == State.None || state == State.Running;
        //chMotor.movement.maxForwardSpeed = speed;
		if (OnStateChanged != null) OnStateChanged(newState, oldState);
	}

	void Update()
	{
        //if (Input.GetButton("Sprint") && chMotor.grounded && ch.velocity.magnitude > 0.1f)
        //    ChangeState(State.Running);
		if (Input.GetButtonUp("Sprint") || (state == State.Running && ch.velocity.magnitude < 0.1f))
			ChangeState(State.None);

		if (Input.GetButtonDown("Crouch"))
			ChangeState(state == State.Crouching ? State.None : State.Crouching);

		if (Input.GetButtonDown("Hold Crouch"))
			ChangeState(State.Crouching);
		if (Input.GetButtonUp("Hold Crouch"))
			ChangeState(State.None);

		if (Input.GetButtonDown("Prone"))
			ChangeState(state == State.Proning ? State.None : State.Proning);

		if (Input.GetButtonUp("Jump") && (state == State.Crouching || state == State.Proning))
			ChangeState(State.None);
	}
}