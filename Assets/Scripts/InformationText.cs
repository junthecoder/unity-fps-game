using UnityEngine;
using System.Collections;

public class InformationText : MonoBehaviour {

	string text =
			"W S A D :  移動\n" +
			"SHIFT : ダッシュ\n" +
			"左クリック : 発射\n" +
			"右クリック : ズーム\n" +
			"R : リロード\n" +
			"1 2 3 4: 武器切り替え\n" +
			"G : グレネード\n" +
			"F : ナイフ\n" +
			"V : 射撃モード切り替え\n" +
			"X : しゃがむ\n" +
			"Z : ほふく前進\n" +
			"ESC: ポーズメニュー\n" +
			"F1:";

	void Start()
	{
		guiText.enabled = Options.main.showHelp;
		guiText.text = text;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Options.main.showHelp = !Options.main.showHelp;
			guiText.enabled = Options.main.showHelp;
			Options.main.Save();
		}

		guiText.text = string.Format("{0:f1}\n", 1 / Time.deltaTime) + text;
	}
}
