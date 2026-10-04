using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	enum State { Up, Down };
	State state = State.Down;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Up()
	{
		if (state == State.Up)
			return;

		state = State.Up;
		animation.Play("Up");
	}

	public void Hit()
	{
		if (state == State.Down)
			return;

		state = State.Down;
		animation.Play("Down");
	}
}
