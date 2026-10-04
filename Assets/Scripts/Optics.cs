using UnityEngine;
using System.Collections;

public class Optics : Attachment
{
	public float magnification;

	protected bool HideOnZoom { get; set; }
	protected Soldier Player { get; private set; }
	protected bool IsZooming { get; private set; }

	private Renderer[] renderers;

	public new void Initialize(Gun gun)
	{
		base.Initialize(gun);
		Player = GameObject.FindWithTag("Player").GetComponent<Soldier>();
		gun.OnZoomed += OnGunZoomed;
		gun.OnFired += OnGunFired;
	}

	//void Update()
	//{
	//	if (!gun) throw new System.InvalidOperationException("Sight.Initialize() must be called after instantiation.");
	//}

	protected virtual void OnGunFired()
	{
	}

	private void OnGunZoomed(Equipment sender, bool zoom)
	{
		IsZooming = zoom;

		if (HideOnZoom)
		{
			if (renderers == null)
				renderers = Gun.GetComponentsInChildren<Renderer>();

			foreach (var renderer in renderers)
				renderer.enabled = !IsZooming;
		}

		if (zoom)
		{
			Camera.main.fov = 1.333333f * Mathf.Rad2Deg * Mathf.Atan(1 / magnification);
			Camera.main.GetComponent<MyMouseLook>().sensitivityY = Player.GetComponent<MyMouseLook>().sensitivityX = Options.main.sensitivity / magnification;
		}
		else
		{
			Camera.main.fov = Options.main.fov;
			Camera.main.GetComponent<MyMouseLook>().sensitivityY = Player.GetComponent<MyMouseLook>().sensitivityX = Options.main.sensitivity;
		}
	}
}
