using UnityEngine;
using System.Collections;

public class GunData : EquipmentData {

	public enum FireMode
	{
		Auto = 4, Triple = 3, Double = 2, Single = 1
	}

	public string Name;
	public FireMode[] fireModes;
	public int fireRate; // RPM
	public int magazineCapacity;
	public float bulletVelocity;
	public float bulletEnergy;
	public float damage;
	public float weight;
	public bool isChamberStockable;
	public GameObject shellCase;
	public GameObject bullet;
	public GameObject muzzleFlash;
	public AudioClip fireSound;
	public AudioClip reloadSound;
	public Optics opticsPrefab;
	public Attachment accessoryPrefab;

	public void Awake()
	{
		SightPosition = transform.FindChild("point_sight").transform.localPosition;
		AccessoryPosition = transform.FindChild("point_attatchment").transform.localPosition;
		EjectionPosition = transform.FindChild("point_ejection").transform.localPosition;
		MuzzlePosition = transform.FindChild("point_muzzle").transform.localPosition;
		StockPosition = transform.FindChild("point_stock").transform.localPosition;
	}

	public override string ToString()
	{
		return Name;
	}

	public float FireInterval
	{
		get { return 1 / (fireRate / 60f); }
	}

	public Vector3 SightPosition
	{
		get;
		private set;
	}

	public Vector3 AccessoryPosition
	{
		get; private set;
	}

	public Vector3 EjectionPosition
	{
		get;
		private set;
	}

	public Vector3 MuzzlePosition
	{
		get;
		private set;
	}

	public Vector3 StockPosition
	{
		get;
		private set;
	}
}
