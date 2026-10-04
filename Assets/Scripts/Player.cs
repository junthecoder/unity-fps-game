using UnityEngine;
using System.Collections;

public class Player : Soldier
{
    private bool isAutoSprint;
    private float speed;

	new void Start()
	{
        base.Start();

        Screen.lockCursor = true;

		OnDamaged += Player_OnDamaged;
		OnDied += Player_OnDied;
	}

	void Player_OnDamaged(Damage damage)
	{
		var blood = (Instantiate(hitBloodPrefab) as HitBlood).GetComponent<HitBlood>();
		blood.bulletAngle = damage.DirectionYaw;
		blood.playerTransform = transform;
	}

	void Player_OnDied(Damage damage)
	{
		// TODO: グレネードのヒット時のbloodの向きを治す
		StartCoroutine(DiedCoroutine());
	}

	IEnumerator DiedCoroutine()
	{
		yield return new WaitForSeconds(2);
		Application.LoadLevel("Start");
	}

	void LateUpdate()
	{
		Camera.main.transform.position = head.position + head.forward * 0.20f; // transform.Find("Head").position;
	}

    void FixedUpdate()
    {
		base.FixedUpdate();

        if (IsJumping)
        {
        }
        else
        {
            var velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (velocity.magnitude > 0)
            {
                if (Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x))
                {
                    if (velocity.y > 0)
                    {
                        speed = CurrentState == RunAndCrouch.State.Running ? 4.5f : 2;
                        velocity.y = speed / 2;
                    }
                    else
                    {
                        speed = 1.5f;
                        velocity *= speed;
                    }
                }
                else
                {
                    ChangeState(RunAndCrouch.State.None);
                    speed = 1.5f;
                    velocity.y = 0;
                    velocity *= speed;
                }

                // TODO: 上下を向いていても同じスピードになるように
                var y = rigidbody.velocity.y;
                rigidbody.AddRelativeForce(new Vector3(velocity.x, 0, velocity.y) * 1000);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, y, rigidbody.velocity.z);
            }
            else
            {
                ChangeState(RunAndCrouch.State.None);
                speed = 0;
            }

            rigidbody.velocity = TruncateVelocity(rigidbody.velocity, speed);

            DebugText.main.Set("front ", velocity.y);
            DebugText.main.Set("right ", velocity.x);
            Animator.SetFloat("SpeedRight", velocity.x);
        }

        var cameraV = Camera.main.transform.localEulerAngles.x;
        if (cameraV > 180) cameraV -= 360;

        DebugText.main.Set("rigidbody.velocity.magnitude", rigidbody.velocity.magnitude);
        DebugText.main.Set("CameraV", cameraV);
		DebugText.main.Set("isSprinting", CurrentState == RunAndCrouch.State.Running);
		DebugText.main.Set("CurrentState", CurrentState);
		DebugText.main.Set("IsJumping", IsJumping);

        Animator.SetBool("Sprint", CurrentState == RunAndCrouch.State.Running);
        Animator.SetFloat("Speed", speed);
        Animator.SetFloat("CameraV", cameraV);
        Animator.SetBool("Jump", IsJumping);

        Animator.SetLookAtWeight(1);
        Animator.SetLookAtPosition(Camera.main.transform.InverseTransformPoint(Vector3.forward));
    }

    Vector3 TruncateVelocity(Vector3 velocity, float speed)
    {
        var velocity2D = new Vector2(velocity.x, velocity.z);

        if (velocity2D.magnitude > speed)
        {
            velocity2D = velocity2D.normalized * speed;
            // 重力方向は変化させない
            return new Vector3(velocity2D.x, velocity.y, velocity2D.y);
        }
        else
            return velocity;
    }

	void Update()
    {
        UpdateState();

		var headLookController = GetComponent<HeadLookController>();
		headLookController.target = Camera.main.transform.TransformPoint(Vector3.forward);

        for (int i = 0; i < Equipments.Length; ++i)
            if (Input.GetKeyDown("" + (i + 1)))
                SwitchEquipment(i);

        UpdateDebugText();
	}

    void UpdateDebugText()
    {
        DebugText.main.Set("Auto Sprint", isAutoSprint);
    }

    void UpdateState()
    {
        if (CurrentState == RunAndCrouch.State.Jumping)
            return;

        if (Input.GetButton("Sprint") && CurrentState != RunAndCrouch.State.Running)
        {
            isAutoSprint = !Input.GetKey(KeyCode.W);
            ChangeState(RunAndCrouch.State.Running);
        }

        if (Input.GetButtonUp("Sprint") && !isAutoSprint)// || (state == State.Running && ch.velocity.magnitude < 0.1f))
            ChangeState(RunAndCrouch.State.None);

        if (Input.GetButtonDown("Crouch"))
            ChangeState(CurrentState == RunAndCrouch.State.Crouching ? RunAndCrouch.State.None : RunAndCrouch.State.Crouching);

        if (Input.GetButtonDown("Hold Crouch"))
            ChangeState(RunAndCrouch.State.Crouching);
        if (Input.GetButtonUp("Hold Crouch"))
            ChangeState(RunAndCrouch.State.None);

        if (Input.GetButtonDown("Prone"))
            ChangeState(CurrentState == RunAndCrouch.State.Proning ? RunAndCrouch.State.None : RunAndCrouch.State.Proning);

        if (Input.GetButtonDown("Jump"))
            if (CurrentState == RunAndCrouch.State.Crouching || CurrentState == RunAndCrouch.State.Proning)
                ChangeState(RunAndCrouch.State.None);
            else
                Jump();
    }
}
