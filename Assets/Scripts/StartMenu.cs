using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{

	enum State
	{
		Start,
		Campaign,
		Multiplayer,
		Options
	}

	State state = State.Start;
	public string[] campaigns;
	public GUISkin skin;

	void Start()
	{
		Screen.lockCursor = false;
	}

	void OnGUI()
	{
		GUI.skin = skin;

		if (state == State.Start)
		{
			var rect = GameGUI.ConvertRect(new Rect(0.1f, 0.4f, 0.4f, 0.1f));

			if (GUI.Button(rect, "CAMPAIGN"))
				state = State.Campaign;

			rect.y += rect.height;
			if (GUI.Button(rect, "MULTIPLAYER"))
				state = State.Multiplayer;

			rect.y += rect.height;
			if (GUI.Button(rect, "OPTIONS"))
				state = State.Options;

			rect.y += rect.height;
			if (GUI.Button(rect, "QUIT"))
				Application.Quit();
		}
		else if (state == State.Campaign)
		{
			var rect = GameGUI.ConvertRect(new Rect(0.1f, 0.4f, 0.4f, 0.1f));

			foreach (var campaign in campaigns)
			{
				if (GUI.Button(rect, campaign))
				{
					WeaponMenu.level = campaign;
					Application.LoadLevel("WeaponSelect");
				}
				rect.y += rect.height;
			}

			if (GameGUI.BackButton())
				state = State.Start;
		}
		else if (state == State.Multiplayer)
		{
			GUI.Label(GameGUI.ConvertRect(new Rect(0.45f, 0.45f, 0.2f, 0.4f)), "Coming soon...");
			//Application.LoadLevel("MultiplayerTest");

			if (GameGUI.BackButton())
				state = State.Start;
		}
		else if (state == State.Options)
		{
			if (Options.main.ShowGUI())
				state = State.Start;
		}
	}
}
