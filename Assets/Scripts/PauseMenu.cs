using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public bool IsPausing { get; private set; }
	public GUISkin skin;

	enum State
	{
		Main,
		Options,
		QuitConfirm
	}

	State state = State.Main;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			Pause(!IsPausing);
	}

	void Pause(bool pause = true)
	{
		IsPausing = pause;
		Time.timeScale = IsPausing ? 0 : 1;

		foreach (var component in GetComponents<MonoBehaviour>())
			if (component != this)
				component.enabled = !IsPausing;

		Camera.main.GetComponent<MyMouseLook>().enabled = !IsPausing;
		Screen.lockCursor = !IsPausing;

		state = State.Main;
	}

	void OnGUI()
	{
		if (!IsPausing) return;

		GUI.skin = skin;

		var rect = ConvertRect(new Rect(0.1f, 0.4f, 0.4f, 0.1f));

		if (state == State.Main)
		{
			if (GUI.Button(rect, "RESUME"))
				Pause(false);

			rect.y += rect.height;
			if (GUI.Button(rect, "OPTIONS"))
				state = State.Options;

			rect.y += rect.height;
			if (GUI.Button(rect, "QUIT"))
				state = State.QuitConfirm;
		}
		else if (state == State.Options)
		{
			// TODO: すぐ設定項目を更新するようにする
			if (Options.main.ShowGUI())
				state = State.Main;
		}
		else if (state == State.QuitConfirm)
		{
			GUI.Label(ConvertRect(new Rect(0.2f, 0.4f, 0.6f, 0.1f)), "Are you sure you want to quit?");

			if (GUI.Button(ConvertRect(new Rect(0.2f, 0.5f, 0.3f, 0.1f)), "YES"))
			{
				Pause(false);
				Screen.lockCursor = false;
				Application.LoadLevel("Start");
			}

			if (GUI.Button(ConvertRect(new Rect(0.5f, 0.5f, 0.3f, 0.1f)), "NO"))
				state = State.Main;
		}
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
