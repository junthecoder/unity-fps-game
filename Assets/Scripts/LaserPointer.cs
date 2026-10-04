using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : Attachment
{
	private LineRenderer laser;
	public Light pointLight;
	private bool isOn = true;

	void Start()
	{
		laser = GetComponent<LineRenderer>();
		laser.castShadows = laser.receiveShadows = false;
		laser.useWorldSpace = false;

		pointLight = Instantiate(pointLight) as Light;
		pointLight.transform.parent = transform;
		pointLight.enabled = false;
	}
	
	void Update()
	{
		if (Gun && Gun.CanControl)
			if (Input.GetButtonDown("Laser"))
			{
				isOn = !isOn;

				laser.enabled = isOn;
				pointLight.enabled = isOn;
			}

		if (!isOn)
			return;

		var maxLength = 300;
		RaycastHit hit;

		Vector3 origin;

		if (Gun)
			if (Gun.CanControl)
				origin = Camera.main.transform.position;
			else
			{
				var animator = Gun.Owner.GetComponent<Soldier>().Animator;
				var l = animator.GetBoneTransform(HumanBodyBones.LeftEye).position;
				var r = animator.GetBoneTransform(HumanBodyBones.RightEye).position;
				l.x = (l.x + r.x) / 2;

				origin = animator.transform.TransformPoint(l);
			}
		else
			origin = transform.position;

		origin += transform.forward * 0.8f;

		if (Physics.Raycast(origin, transform.forward, out hit, maxLength))
		{
			laser.SetPosition(1, transform.InverseTransformPoint(hit.point));

			pointLight.transform.position = hit.point - transform.forward * pointLight.range / 2;
			pointLight.transform.rotation = Quaternion.LookRotation(-hit.normal);
			pointLight.enabled = true;
		}
		else
		{
			laser.SetPosition(1, transform.InverseTransformPoint(transform.TransformPoint(Vector3.forward * maxLength)));
			pointLight.enabled = false;
		}
	}
}
