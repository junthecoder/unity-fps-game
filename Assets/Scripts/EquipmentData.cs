using UnityEngine;
using System.Collections;

public class EquipmentData : MonoBehaviour
{
	public float reloadTime;

	public GameObject Instantiate()
	{
		// TODO: 実装
		return (Instantiate(this) as EquipmentData).gameObject;
	}
}
