using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;

[RequireComponent(typeof(AudioSource))]
public class Gun : Equipment
{
	public delegate void FiredEventHandler();
	public event FiredEventHandler OnFired;

	private float lastShotTime;
	private int currentFireModeIndex = 0;
	private float nextFireTime;
	private bool burstInProgress;

	public GunData Data { get; private set; }
	private float minSpread;
	private Optics sight;
	private Attachment accessory;
	private Vector3 currentRecoil;
	private TransformAnimation transformAnimation;
    private Transform defaultParent;
	private float recoilZ = 0.1f;

	public void Initialize(GameObject owner, bool canControl)
	{
		Data = GetComponent<GunData>();

		int numMagazines;
		if (Data.magazineCapacity <= 30)
			numMagazines = 10;
		else
			numMagazines = 4;

		var max = Data.magazineCapacity + (Data.isChamberStockable ? 1 : 0);
		IsStockable = Data.isChamberStockable;

		base.Initialize(owner, canControl, numMagazines * max, max);

		if (Data.opticsPrefab)
		{
			sight = Instantiate(Data.opticsPrefab) as Optics;
			sight.transform.parent = transform;
			sight.transform.localPosition = Data.SightPosition;
			sight.Initialize(this);
		}
		if (Data.accessoryPrefab)
		{
			accessory = Instantiate(Data.accessoryPrefab) as Attachment;
			accessory.transform.parent = transform;
			accessory.transform.localPosition = Data.AccessoryPosition;
			accessory.Initialize(this);
		}

		if (CanControl)
			if (Player.GetComponent<RunAndCrouch>())
				Player.GetComponent<RunAndCrouch>().OnStateChanged += OnPlayerStateChanged;
			else
				Player.GetComponent<Soldier>().OnStateChanged += OnPlayerStateChanged;

		OnZoomed += Gun_OnZoomed;
		OnReloadStarted += () => { Play("Reload"); };
		OnReloadFinished += () => { if (Player.GetComponent<Soldier>().CurrentState == RunAndCrouch.State.Running) OnPlayerStateChanged(RunAndCrouch.State.Running, RunAndCrouch.State.None); };

        defaultParent = Owner.transform.FindChild("Camera").transform;
		Play("Default");

		// ターゲット位置/回転補正
		var leftHand = transform.FindChild("point_lefthand");
		leftHand.position -= transform.right * 0.05f;
		leftHand.position -= transform.up * 0.085f;
		leftHand.localEulerAngles = new Vector3(0, 0, 220);

		var rightHand = transform.FindChild("point_righthand");
		rightHand.position += transform.right * 0.03f;
		rightHand.position -= transform.forward * 0.04f;
		rightHand.localEulerAngles = new Vector3(270, 270, 0);
	}

	void Start()
	{
		nextFireTime = Time.time; // これが0だと補正値が非常に大きくなってしまう
		minSpread = Data.weight * 1.5f;
	}

	Sequence currentSequence;

	void Play(string name, bool append = false)
	{
		if (IsReloading && name != "Reload")
			return;

        if (!append)
        {
            if (currentSequence != null)
                currentSequence.Kill();

            currentSequence = new Sequence();//new SequenceParms().Loops(-1, LoopType.Yoyo));
        }

        if (name != "Running" || IsReloading)
            transform.parent = defaultParent;
		
		if (name == "Default")
		{
			//currentSequence.Kill();
			//currentSequence = new Sequence(new SequenceParms().Loops(-1, LoopType.Yoyo));

			//var localPosition = new Vector3(0.13f, -0.15f - Data.StockPosition.y, -0.1f - Data.StockPosition.z);

			//transform.localPosition = localPosition;
			//transform.localRotation = Quaternion.identity;

			currentSequence.Append(HOTween.To(transform, 0.2f, new TweenParms()
				.Prop("localPosition", new Vector3(0.13f, -0.15f - Data.StockPosition.y, -0.1f - Data.StockPosition.z))
				.Prop("localRotation", Quaternion.identity)
				));

			//currentSequence.Append(HOTween.To(transform, 0.2f, new TweenParms()
			//	 .Prop("localPosition", new Vector3(0.15f, -0.15f - Data.StockPosition.y, -0.1f - Data.StockPosition.z))
			//	 .Prop("localRotation", Quaternion.identity)
			//	 ));
		}
		//else if (name == "Walking")
		//if (name == "Default")
		//{
		//	currentSequence = new Sequence(new SequenceParms().Loops(-1, LoopType.Yoyo));

		//	var w = 0.02f;
		//	currentSequence.Append(HOTween.To(transform, 0.2f, new TweenParms()
		//		.Prop("localPosition", new Vector3(0.13f + w, -0.15f - Data.StockPosition.y, -0.1f - Data.StockPosition.z), false)
		//		.Prop("localRotation", Quaternion.identity)
		//		));

		//	currentSequence.Append(HOTween.To(transform, 0.2f, new TweenParms()
		//					.Prop("localPosition", new Vector3(0.13f - w, -0.15f - Data.StockPosition.y, -0.1f - Data.StockPosition.z), false)
		//					.Prop("localRotation", Quaternion.identity)
		//					));
		//}
		else if (name == "Running")
		{
			currentSequence.AppendInterval(0.7f);

			currentSequence.AppendCallback(() => {
				currentSequence = new Sequence();
				transform.parent = Owner.transform;

			currentSequence.Append(HOTween.To(transform, 1f, new TweenParms()
				.Prop("localPosition", new Vector3(0, 1.2f, 0.6f))
				.Prop("localRotation", Quaternion.Euler(new Vector3(10, -75, 10)))
				));

			currentSequence.Play();
			});
		}
		else if (name == "Zoom")
		{
			var zoomPosition = new Vector3(0, 0, 0.08f);

			if (sight)
			{
				var center = sight.transform.FindChild("point_eye");
				if (center)
					zoomPosition -= transform.InverseTransformPoint(center.transform.position);
			}
			else
				zoomPosition -= Data.SightPosition;

			currentSequence.Append(HOTween.To(transform, 0.1f, new TweenParms()
				.Prop("localPosition", zoomPosition)
				.Prop("localRotation", Quaternion.identity)
				));
		}
		else if (name == "Reload")
		{
			var magazine = transform.FindChild("Magazine");
			var magazinePos = magazine.localPosition;

			var target = new GameObject("MagazineTarget");
			target.transform.parent = magazine;
			target.transform.position = magazine.position - transform.right * 0.05f - transform.forward * 0.1f;
			target.transform.eulerAngles = transform.eulerAngles + new Vector3(270, 0, 90);

			var ikLeftArm = Owner.GetComponent<Soldier>().ikLeftArm;
			var previousTarget = ikLeftArm.target;
			ikLeftArm.target = target.transform;

			var reloadTime = Data.reloadTime;

			reloadTime -= 0.1f;
			currentSequence.Append(HOTween.To(transform, 0.1f, new TweenParms()
				   .Prop("localPosition", new Vector3(0.13f, -0.15f - Data.StockPosition.y, -0.1f - Data.StockPosition.z), false)
				   .Prop("localRotation", Quaternion.Euler(-20, 0, -35), false)
				   ));

			currentSequence.Append(HOTween.To(magazine, reloadTime / 2, new TweenParms()
					.Prop("localPosition", magazine.localPosition + Vector3.down * 0.4f, false)
					));
			reloadTime -= reloadTime / 2;

			currentSequence.Append(HOTween.To(magazine, reloadTime / 2, new TweenParms()
					.Prop("localPosition", magazinePos, false)
					.OnComplete(() => { IsReloading = false;  ikLeftArm.target = previousTarget; Play("Default"); })
					));
			reloadTime -= reloadTime / 2;
		}
		else
			throw new NotImplementedException();

		currentSequence.Play();
	}

	void Gun_OnZoomed(Equipment sender, bool zoom)
	{
		if (zoom)
		{
			Play("Zoom");

			var soldier = Player.GetComponent<Soldier>();
			if (soldier.CurrentState == RunAndCrouch.State.Running)
				soldier.ChangeState(RunAndCrouch.State.None);
		}
		else
			Play("Default");
	}

	void OnPlayerStateChanged(RunAndCrouch.State newState, RunAndCrouch.State oldState)
	{
		if (newState == RunAndCrouch.State.Running)
		{
			Zoom(false);
			//var localPosition = Camera.main.transform.InverseTransformPoint(transform.localPosition);
			//transform.parent = Owner.transform
            Play("Default");
            Play("Running", true);
			//transform.position = Camera.main.transform.TransformPoint(localPosition);
		}
		else if (newState != RunAndCrouch.State.Jumping)
		{
			//transform.parent = Camera.main.transform;
			Play("Default");
		}
	}

	void Update()
	{
		if (!Data) throw new InvalidOperationException("You must call Gun.Initialize() after instantiation.");
		if (Time.timeScale == 0) return;
		if (!CanControl) return;

		DebugText.main.Set("Recoil", currentRecoil);

		Crosshair.main.Spread = minSpread +
			currentRecoil.magnitude / 20 * Mathf.Min(5, Data.weight);

		var velocity = new Vector2(Player.rigidbody.velocity.z, Player.rigidbody.velocity.x);
		Crosshair.main.Spread += velocity.magnitude * 0.5f;

		DebugText.main.Set("spread", Crosshair.main.Spread);

		// Fire()より前に行うこと
		if (Input.GetMouseButtonDown(0) && NumLoaded == 0)
			Reload();

		if (!burstInProgress)
		{
			switch (CurrentFireMode)
			{
				case GunData.FireMode.Auto:
					if (Input.GetMouseButton(0))
						Fire();
					break;
				case GunData.FireMode.Double:
					if (Input.GetMouseButtonDown(0))
						StartCoroutine(FireBurst(2));
					break;
				case GunData.FireMode.Triple:
					if (Input.GetMouseButtonDown(0))
						StartCoroutine(FireBurst(3));
					break;
				default:
					if (Input.GetMouseButtonDown(0))
						Fire();
					break;
			}
		}

		if (Input.GetButtonDown("Reload"))
		{
			Reload();
		}

		// TODO: equipmentに移動
		if (Player.GetComponent<Soldier>().CurrentState != RunAndCrouch.State.Running)
            if (Input.GetButtonDown("Zoom"))
                Zoom(true);
            else if (Input.GetButtonUp("Zoom"))
                Zoom(false);

		//if (!Input.GetMouseButton(0))
		ReduceRecoil();

		if (Input.GetButtonDown("FireMode"))
			currentFireModeIndex = (currentFireModeIndex + 1) % Data.fireModes.Length;
	}

	public GunData.FireMode CurrentFireMode
	{
		get { return Data.fireModes[currentFireModeIndex]; }
	}

	private IEnumerator FireBurst(int shotCount)
	{
		burstInProgress = true;
		for (int shot = 0; shot < shotCount; ++shot)
		{
			if (!Fire())
				break;

			if (shot + 1 < shotCount)
				yield return new WaitForSeconds(Data.FireInterval);
		}

		burstInProgress = false;
	}

	private void ReduceRecoil()
	{
		if (currentRecoil.magnitude == 0) return;

		var oldRecoil = currentRecoil;

		var recoilReduction = Vector3.one * (3000000 / Data.bulletEnergy / Data.damage * Time.deltaTime);//Vector3.one * 120 / Data.weight * Time.deltaTime;
		if (Input.GetButton("Fire1"))
			recoilReduction = Vector3.zero;
		var roundsPerSecond = Data.fireRate / 60f;

		if (name.StartsWith("m98b"))
			recoilReduction.z = recoilZ * 3 * Time.deltaTime;
		else
			recoilReduction.z = recoilZ * roundsPerSecond * Time.deltaTime;
		
		currentRecoil.x = Mathf.Min(0, currentRecoil.x + recoilReduction.x);
		if (currentRecoil.y < 0)
			currentRecoil.y = Mathf.Min(0, currentRecoil.y + recoilReduction.y);
		else
			currentRecoil.y = Mathf.Max(0, currentRecoil.y - recoilReduction.y);

		currentRecoil.z = Mathf.Min(0, currentRecoil.z + recoilReduction.z);

		// TODO: AddRecoilのコードと重複
		Vector3 dd = oldRecoil - currentRecoil;
		Vector2 d = new Vector2(dd.x, dd.y);
		Camera.main.transform.localEulerAngles -= new Vector3(d.x, d.y, 0);

		var d2 = oldRecoil.z - currentRecoil.z;
		transform.localPosition -= new Vector3(0, 0, d2);
	}

	private bool Fire()
	{
		if (IsReloading)
			return false;

		if (NumLoaded == 0)
			return false;

		if (Time.time < nextFireTime)
			return false;

		if (Player.GetComponent<RunAndCrouch>() && Player.GetComponent<RunAndCrouch>().state == RunAndCrouch.State.Running)
			return false;

		var newPlayer = Player.GetComponent<Soldier>();
		if (newPlayer && newPlayer.CurrentState == RunAndCrouch.State.Running)
			newPlayer.ChangeState(RunAndCrouch.State.None);

		nextFireTime = Time.time + Data.FireInterval;

		audio.PlayOneShot(Data.fireSound);
		--NumLoaded;

		var bolt = transform.FindChild("Bolt");
		if (bolt)
		{
			if (bolt.animation)
			{
				bolt.animation.Play("Back");

				if (NumLoaded != 0)
				{
					var state = bolt.animation.PlayQueued("Back");
					state.time = state.length - 0.1f;
					state.speed = -1;
				}
			}
		}
		
		var direction = Camera.main.transform.forward;
		if (!IsZooming)
		{
			var spreadX = Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 4) * Crosshair.main.Spread * (UnityEngine.Random.value < 0.5f ? -1 : 1);
			var spreadY = Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 4) * Crosshair.main.Spread * (UnityEngine.Random.value < 0.5f ? -1 : 1);
			direction = Quaternion.Euler(spreadX, spreadY, 0) * direction;
		}

		var bulletObject = Instantiate(Data.bullet, Camera.main.transform.position, Quaternion.LookRotation(direction)) as GameObject;
		var bullet = bulletObject.GetComponent<Bullet>();
		bullet.shooterTag = Player.gameObject.tag;
		bullet.lineOffset = Data.MuzzlePosition.z;
		bullet.speed = Data.bulletVelocity;
		bullet.damage = Data.damage;

		Instantiate(Data.muzzleFlash, transform.TransformPoint(Data.MuzzlePosition), Camera.main.transform.rotation);

		StartCoroutine(EjectShellCase());

		Recoil();

		if (OnFired != null) OnFired();
		return true;
	}

	private IEnumerator EjectShellCase()
	{
		if (!Data.shellCase) yield break;

		//yield return new WaitForSeconds(animation ? animation["Back"].length : 0);
		yield return 0;

		var shellCase = Instantiate(Data.shellCase, transform.TransformPoint(Data.EjectionPosition), Camera.main.transform.rotation) as GameObject;

		var force = new Vector3(3, 0, 1);
		if (Data.EjectionPosition.x < 0)
			force.x *= -1;

		shellCase.rigidbody.AddRelativeForce(force);
	}

	private void Recoil()
	{
		var minVertRecoil = Data.bulletEnergy / 3000;//0.7f + (Data.weight - 5) * 0.05f;
		var maxVertRecoil = minVertRecoil + 0.5f;
		
		recoilZ = Data.bulletEnergy / 200000;
		
		var recoil = new Vector3(UnityEngine.Random.Range(-minVertRecoil, -maxVertRecoil), UnityEngine.Random.Range(-0.3f, 0.3f), -recoilZ);

		if (currentRecoil.x < -15)
			recoil.x -= UnityEngine.Random.Range(-minVertRecoil, -maxVertRecoil);

		currentRecoil += recoil;

		Camera.main.transform.localEulerAngles += new Vector3(recoil.x, recoil.y, 0);
		transform.localPosition += new Vector3(0, 0, recoil.z);
	}
}
