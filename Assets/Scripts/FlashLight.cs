using UnityEngine;
using System.Collections;

public class FlashLight : Attachment
{
	Light light;

	void Start()
	{
		light = GetComponentInChildren<Light>();
	}

	void Update()
	{
		if (Gun && Gun.CanControl && Input.GetButtonDown("Laser"))
			light.enabled = !light.enabled;
	}
}
