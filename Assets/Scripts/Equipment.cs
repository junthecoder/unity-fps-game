using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour
{
	public float ReloadTime;

	public int NumLoaded { get; protected set; }
	public int NumLoadedMax	{ get; private set; }
	public int NumRemains { get; private set; }
	public bool IsReloading { get; protected set; }
	public bool CanControl { get; private set; }
	public bool IsStockable { get; protected set; }
	public GameObject Owner { get; set; }

	// for player only
	public bool HasCrosshair { get; private set; }
	public bool CanZoom { get; private set; }
	public bool IsZooming { get; private set; }
	public GameObject Player { get; private set; }
	public Crosshair Crosshair { get; private set; }

	public delegate void ZoomedEventHandler(Equipment sender, bool zoom);
	public event ZoomedEventHandler OnZoomed;
	public delegate void ReloadEventHandler();
	public event ReloadEventHandler OnReloadStarted;
	public event ReloadEventHandler OnReloadFinished;

	protected void Initialize(GameObject owner, bool canControl, int num, int numLoadedMax)
	{
		Owner = owner;
		NumLoadedMax = numLoadedMax;
		NumLoaded = NumLoadedMax;
		NumRemains = num - NumLoaded;
		CanControl = canControl;
		ReloadTime = GetComponent<EquipmentData>().reloadTime;

		if (CanControl)
		{
			Player = GameObject.FindWithTag("Player");
			Player.GetComponent<Soldier>().OnEquipmentChanged += OnEquipmentChanged;
			if (Player.GetComponent<Soldier>())
				Player.GetComponent<Soldier>().OnStateChanged += OnPlayerStateChanged;
			Crosshair = GameObject.FindWithTag("Player").GetComponent<Crosshair>();
		}
	}

	void OnPlayerStateChanged(RunAndCrouch.State newState, RunAndCrouch.State oldState)
	{
		if (newState == RunAndCrouch.State.Running)
			Zoom(false);
	}

	void OnEquipmentChanged(Equipment newEquipment, Equipment oldEquipment)
	{
		if (oldEquipment == this)
		{
			Zoom(false);
			IsReloading = false;
		}
	}

	public void Zoom(bool zoom)
	{
		if (IsReloading) return;
		if (!CanControl) return;
		if (zoom == IsZooming) return;

		IsZooming = !IsZooming;
		Crosshair.enabled = !IsZooming;

		if (OnZoomed != null) OnZoomed(this, IsZooming);
	}

	public void Reload()
	{
		StartCoroutine(ReloadCoroutine());
	}

	private IEnumerator ReloadCoroutine()
	{
		if (IsReloading || NumLoaded == NumLoadedMax || NumRemains == 0)
			yield break;

		Zoom(false);
		IsReloading = true;
		if (OnReloadStarted != null) OnReloadStarted();

		yield return new WaitForSeconds(ReloadTime);

		IsReloading = false;
		int capacityRemaining = NumLoadedMax - NumLoaded;
		int n = Mathf.Min(NumRemains, capacityRemaining);

		//if (GetComponent<Animation>() != null && NumLoaded == 0)
		//	animation.PlayQueued("Forward");

		NumRemains -= n;
		NumLoaded += n;

		if (OnReloadFinished != null) OnReloadFinished();
	}

	//void Fire()
	//{
	//	if (quantity <= 0) return;

	//	--quantity;
	//}

	//void OnEnabled()
	//{
	//}

	//void OnDisabled()
	//{
	//}
}
