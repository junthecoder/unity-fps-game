using UnityEngine;
using System.Collections;


class LerpTransform : MonoBehaviour
{
	bool repeat;
	float startTime;
	float transformTime;
	Vector3 pos1, pos2;
	Quaternion rot1, rot2;

	public bool IsTransforming { get; private set; }

	void Start()
	{
		IsTransforming = false;
		pos1 = pos2 = transform.localPosition;
		rot1 = rot2 = transform.localRotation;
	}

	public void Set(Quaternion localRotation, float transformTime, bool repeat = false)
	{
		Set(transform.localPosition, localRotation, transformTime, repeat);
	}

	public void Set(Vector3 localPosition, float transformTime, bool repeat = false)
	{
		Set(localPosition, transform.localRotation, transformTime, repeat);
	}

	public void Set(Vector3 localPosition, Quaternion localRotation, float transformTime, bool repeat = false)
	{
		this.transformTime = transformTime;
		this.repeat = repeat;
		
		startTime = Time.time;
		pos1 = transform.localPosition;
		rot1 = transform.localRotation;
		pos2 = localPosition;
		rot2 = localRotation;
		IsTransforming = true;
	}

	public void Update()
	{
		if (!IsTransforming) return;

		var t = (Time.time - startTime) / transformTime;

		transform.localPosition = Vector3.Lerp(pos1, pos2, t);
		transform.localRotation = Quaternion.Lerp(rot1, rot2, t);

		if (t >= 1.0)
		{
			if (repeat)
			{
				var tempPos = pos1;
				pos1 = pos2;
				pos2 = tempPos;

				var tempRot = rot1;
				rot1 = rot2;
				rot2 = tempRot;

				startTime = Time.time;
			}
			else
				IsTransforming = false;
		}
	}
}