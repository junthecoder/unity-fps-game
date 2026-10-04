using UnityEngine;
using System.Collections;

public class WeaponGUI : MonoBehaviour
{

	public Font font;
	public Texture backgroundTexture;
	Soldier player;
	GUIStyle style;

	void Start()
	{
		player = GetComponent<Soldier>();
		style = new GUIStyle();
		style.font = font;
	}

	void OnGUI()
	{
		var equipment = player.CurrentEquipment;
		if (!equipment) return;

		var rect = new Rect(Screen.width - 300, Screen.height - 130, 280, 110);
		GUI.DrawTexture(rect, backgroundTexture);

		style.fontSize = 58;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		GUI.Label(new Rect(rect.x + 20, rect.y + 10, 100, 100), string.Format("{0,3}/", equipment.NumLoaded), style);
		style.fontSize = 35;
		GUI.Label(new Rect(rect.x + 170, rect.y + 10, 100, 100), string.Format("{0,3}", equipment.NumRemains), style);

		if (equipment is Gun)
		{
			var gun = equipment as Gun;
			style.fontSize = 19;
			GUI.Label(new Rect(rect.x + 160, rect.y + 45, 100, 100), "[ " + gun.CurrentFireMode.ToString().ToUpper() + " ]", style);
		}

		GUI.Label(new Rect(rect.x + 20, rect.y + 80, 100, 100), "Gx" + player.NumGrenades + (equipment.IsReloading ? "   RELOADING" : ""), style);
		GUI.Label(new Rect(rect.x + 200, rect.y + 80, 100, 100), string.Format("{0,3:0}%", player.Health), style);
	}

	Rect ConvertRect(Rect rect)
	{
		rect.x *= Screen.width;
		rect.width *= Screen.width;
		rect.y *= Screen.height;
		rect.height *= Screen.height;
		return rect;
	}
}
