using UnityEngine;
using System.Collections;
using Holoville.HOTween;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Soldier : MonoBehaviour
{
	public static EquipmentData[] staticEquipmentDatas;

	public EquipmentData[] defaultEquipmentDatas;
	public HitBlood hitBloodPrefab;
	public Transform head;
	public IKLimb ikRightArm, ikLeftArm;

	public delegate void StateChangedHandler(RunAndCrouch.State newState, RunAndCrouch.State oldState);
	public event StateChangedHandler OnStateChanged;
	public delegate void DamagedHandler(Damage damage);
	public event DamagedHandler OnDamaged;
	public event DamagedHandler OnDied;

    public bool IsDead { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsGrounded { get; private set; }
    public RunAndCrouch.State CurrentState { get; private set; }
	public Animator Animator { get; private set; }

	private float speedRight;
	private float targetSpeed;
	private bool isCrouching;
	private bool isPlayer;
	protected GameObject Player { get; private set; }
	private GrenadeSet grenadeSet;
	public GrenadeSetData grenadeSetData;
	public int NumGrenades
	{
		get
		{
			return grenadeSet ? (grenadeSet.NumRemains + grenadeSet.NumLoaded) : 0;
		}
	}

	public Equipment CurrentEquipment { get { return currentEquipmentIndex == -1 ? null : Equipments[currentEquipmentIndex]; } }

	public delegate void EquipmentChangedHandler(Equipment newEquipment, Equipment oldEquipment);
	public event EquipmentChangedHandler OnEquipmentChanged;

    protected Equipment[] Equipments { get; private set; }
	protected int currentEquipmentIndex = -1;

	public float Health { get; protected set; }

    public Transform gunPosition;

	protected void Start()
	{
		isPlayer = gameObject.tag == "Player";
		Animator = GetComponent<Animator>();
		Health = 100;
		Player = GameObject.FindWithTag("Player");

		rigidbody.centerOfMass = new Vector3(0, 0.8f, 0);

		if (grenadeSetData)
		{
			grenadeSet = grenadeSetData.Instantiate().AddComponent<GrenadeSet>();
			grenadeSet.Initialize(gameObject, isPlayer);
		}

		for (int i = 1; i < Animator.layerCount; ++i)
			Animator.SetLayerWeight(i, 1);

		EquipmentData[] equipmentDatas;
		if (staticEquipmentDatas == null)
			equipmentDatas = defaultEquipmentDatas;
		else
			equipmentDatas = staticEquipmentDatas;

		Equipments = new Equipment[equipmentDatas.Length];

		for (int i = 0; i < equipmentDatas.Length; ++i)
		{
			var gunObject = equipmentDatas[i].Instantiate();
            //gunObject.transform.parent = gunPosition;// Animator.GetBoneTransform(HumanBodyBones.RightHand).FindChild("GunPosition").transform;

			var gun = gunObject.AddComponent<Gun>();
            
			gun.Initialize(gameObject, isPlayer);
			gunObject.SetActive(false);
			Equipments[i] = gun;
		}

		OnStateChanged += Player_OnStateChanged;
		SwitchEquipment(0);
	}

    protected void Jump()
    {
        if (IsGrounded)
        {
            IsJumping = true;
            rigidbody.AddForce(transform.up * 5.5f, ForceMode.VelocityChange);
        }
    }

	void Player_OnStateChanged(RunAndCrouch.State newState, RunAndCrouch.State oldState)
	{
	}

	public void SwitchEquipment(int index)
	{
		if (index == currentEquipmentIndex)
			return;

		var oldIndex = currentEquipmentIndex;

		if (oldIndex >= 0)
			CurrentEquipment.gameObject.SetActive(false);

		currentEquipmentIndex = index;
		CurrentEquipment.gameObject.SetActive(true);

		// TODO: Gunに移動(イベント使用)
		ikLeftArm.target = CurrentEquipment.gameObject.transform.FindChild("point_lefthand");
		ikRightArm.target = CurrentEquipment.gameObject.transform.FindChild("point_righthand");
		

		if (OnEquipmentChanged != null)
			OnEquipmentChanged(CurrentEquipment, oldIndex < 0 ? null : Equipments[oldIndex]);
	}

	void Hit(Damage damage)
	{
		if (IsDead) return;

		Health = Mathf.Max(0, Health - damage.damage);

		if (damage.damage > 0 && OnDamaged != null)
			OnDamaged(damage);

		if (Health == 0)
		{
			IsDead = true;

			if (OnDied != null) OnDied(damage);

			// TODO: use mecanim animation
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;


			rigidbody.AddForceAtPosition(damage.direction * 10000, damage.point);
			//rigidbody.AddForce((damage.damager.transform.position - transform.position).normalized * 20000);
		}
	}

	protected void FixedUpdate()
	{
		// 着地チェック
		var isHittingGround = Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, 0.1f);
		if (IsGrounded && !isHittingGround)
		{
			IsGrounded = false;
		}
		else if (!IsGrounded && isHittingGround)
		{
			IsJumping = false;
			IsGrounded = true;
		}
		
		DebugText.main.Set("isGrounded", IsGrounded);
	}

	public void ChangeState(RunAndCrouch.State newState)
	{
		if (CurrentState == newState)
			return;

		var oldState = CurrentState;

		CurrentState = newState;
		//switch (newState)
		//{
		//	case State.None: speed = walkSpeed; cameraAnimation.Play("Stand"); break;
		//	case State.Running: speed = runSpeed; cameraAnimation.Play("Stand"); break;
		//	case State.Crouching: speed = crouchSpeed; cameraAnimation.Play("Crouch"); break;
		//	case State.Proning: speed = proneSpeed; cameraAnimation.Play("Prone"); break;
		//	default: throw new System.NotImplementedException();
		//}

		//chMotor.jumping.enabled = state == State.None || state == State.Running;
		//chMotor.movement.maxForwardSpeed = speed;
		
		if (OnStateChanged != null) OnStateChanged(newState, oldState);
	}
}
