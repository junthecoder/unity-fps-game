using UnityEngine;
using System.Collections;

public struct Damage {
	public GameObject damager;
	public string ownerTag;
	public float damage;
	public Vector3 point, direction;
	public float DirectionYaw
	{
		get
		{
			var incomingDirection = direction;
			if (incomingDirection.sqrMagnitude == 0 && damager)
				incomingDirection = damager.transform.forward;

			if (incomingDirection.sqrMagnitude == 0)
				return 0;

			return Mathf.Atan2(incomingDirection.x, incomingDirection.z) * Mathf.Rad2Deg;
		}
	}

	public Damage(GameObject damager, string ownerTag, float damage)
	{
		this.damager = damager;
		this.ownerTag = ownerTag;
		this.damage = damage;
		this.point = Vector3.zero;
		this.direction = Vector3.zero;
	}
}
