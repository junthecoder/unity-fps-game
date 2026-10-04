// Earlier movement-controller experiment; retained separately from the later Player.cs implementation.

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GUIText debugText;

	Vector3 velocity;
	//float maxSpeed = 8;
	Vector2 sensitivity = new Vector2(3, 3);
	public Gun gun;

	enum State
	{
		Normal,
		Sprint,
		Crouch,
		Prone
	};
	State state;
	float[] heights = new float[] { 1.0f, 1.0f, 0.2f, -0.6f };
	float[] speeds = new float[] { 5.0f, 8.0f, 2.0f, 1.0f };

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
		var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity.x;
		var rotationY = -transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity.y;
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

		debugText.text = transform.forward.ToString();

		if (Input.GetAxis("Vertical") != 0)
			transform.position += Input.GetAxis("Vertical") * AdvanceForward * speeds[(int)state] * Time.deltaTime;
		if (Input.GetAxis("Horizontal") != 0)
			transform.position += Input.GetAxis("Horizontal") * AdvanceRight * speeds[(int)state] * Time.deltaTime;

		if (gun.IsZooming)
			Camera.main.fieldOfView = 50;
		else
			Camera.main.fieldOfView = 60;

		UpdateState();
	}

	//void FixedUpdate()
	//{


	//	if (Input.GetAxis("Vertical") != 0)
	//		rigidbody.MovePosition(rigidbody.position + Input.GetAxis("Vertical") * AdvanceForward * speeds[(int)state] * Time.deltaTime);
	//	if (Input.GetAxis("Horizontal") != 0)
	//		rigidbody.MovePosition(rigidbody.position + Input.GetAxis("Horizontal") * AdvanceRight * speeds[(int)state] * Time.deltaTime);
	//}

	//void OnCollisionEnter(Collision collision)
	//{
	//	Debug.Log("collide!!");
	//	}

	void UpdateState()
	{
		if (Input.GetButtonDown("Sprint"))
			state = State.Sprint;

		if (Input.GetButtonUp("Sprint") && state == State.Sprint)
			state = State.Normal;

		if (Input.GetButtonUp("Jump"))
			state = State.Normal;

		if (Input.GetButtonDown("Crouch"))
		{
			if (state == State.Crouch)
				state = State.Normal;
			else
				state = State.Crouch;
		}

		if (Input.GetButtonDown("Prone"))
		{
			if (state == State.Prone)
				state = State.Normal;
			else
				state = State.Prone;
		}

		//var cameraPosition = Camera.main.transform.localPosition;
		var position = transform.localPosition;
		position.y = heights[(int)state];
		transform.localPosition = position;
	}

	public Vector3 AdvanceForward
	{
		get
		{
			return new Vector3(transform.forward.x, 0, transform.forward.z);
		}
	}

	public Vector3 AdvanceRight
	{
		get
		{
			var forward = AdvanceForward;
			return new Vector3(forward.z, 0, -forward.x);
		}
	}

	public float Height
	{
		get { return heights[(int)state]; }
	}

	public Vector3 HeadPosition
	{
		get
		{
			var result = transform.position;
			// TODO:
			result.y = 2;
			return result;
		}
	}
}
