using UnityEngine;
using System.Collections;

public class GrenadeSetData : EquipmentData
{
	public Grenade grenadePrefab;

	void Start()
	{
		reloadTime = 0;
	}
}
