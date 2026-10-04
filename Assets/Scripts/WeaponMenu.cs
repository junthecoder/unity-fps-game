using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponMenu : MonoBehaviour {

	class SelectableLabel
	{
		public string Title;
		object[] items;
		public int CurrentIndex { get; set; }
		public bool IsActive = false;
		private Rect rect;
		public object id;

		public SelectableLabel(string title, object[] items, object id = null)
		{
			this.Title = title;
			this.items = items;
			this.id = id;
			CurrentIndex = 0;
		}

		public void Draw(Rect rect)
		{
			this.rect = rect;
			var preColor = GUI.contentColor;
			//GUI.contentColor = IsActive ? Color.white : Color.grey;
			GUI.Button(rect, "" + items[CurrentIndex]);
			GUI.contentColor = preColor;
		}

		public object[] Items
		{
			get { return items; }
		}

		public object CurrentItem
		{
			get { return items[CurrentIndex]; }
		}

		public void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				var mousePos = Input.mousePosition;
				mousePos.y = Screen.height - mousePos.y;

				if (rect.Contains(mousePos))
				{
					CurrentIndex = (CurrentIndex + 1) % items.Length;
					OnItemChanged(id);
					IsActive = true;
				}
			}

			//if (!IsActive) return;

			//if (Input.GetKeyDown(KeyCode.RightArrow))
			//{
			//	currentIndex = (currentIndex + 1) % items.Length;
			//	OnItemChanged();
			//}
			//else if (Input.GetKeyDown(KeyCode.LeftArrow))
			//{
			//	--currentIndex;
			//	if (currentIndex < 0) currentIndex = items.Length - 1;
			//	OnItemChanged();
			//}
		}

		public delegate void ItemChangedHandler(object id);
		public event ItemChangedHandler OnItemChanged;
	}

	public GunData[] guns;
	public GunData[] pistols;
	public int currentIndex;
	public Optics[] sights;
	public Attachment[] accessories;
	public GameObject[] grenades;
	public GunData currentGun;
	public GUISkin skin;
	List<SelectableLabel> labels = new List<SelectableLabel>();
	int group = 0;
	public static string level = "Station";

	void Start()
	{
		// TODO: add iron sight
		//var list = new List<Attatchment>(attatchments);
		//list.Insert(0, null);
		//attatchments = list.ToArray();
		//var muzzles = new string[] {"Suppressor", "Flash Supressor", "Heavy Barrel"};
		labels.Add(new SelectableLabel("Primary", guns, 0));
		labels.Add(new SelectableLabel("Sight", sights, 0));
		labels.Add(new SelectableLabel("Accessory", accessories, 0));
		labels.Add(new SelectableLabel("Secondary", guns, 1));
		labels.Add(new SelectableLabel("Sight", sights, 1));
		labels.Add(new SelectableLabel("Accessory", accessories, 1));
		//labels.Add(new SelectableLabel("Pistol", pistols));
		//labels.Add(new SelectableLabel("Sight", sights));
		//labels.Add(new SelectableLabel("Attatchments", attatchments));
		//labels.Add(new SelectableLabel("Grenade", grenades));

		labels[0].IsActive = true;

		foreach (var label in labels)
			label.OnItemChanged += id => { group = (int)id; Refresh(); };

		if (Options.main.currentEquipmentIndeces != null)
			for (int i = 0; i < Mathf.Min(labels.Count, Options.main.currentEquipmentIndeces.Length); ++i)
				labels[i].CurrentIndex = Options.main.currentEquipmentIndeces[i];

		Refresh();
	}
	
	void Update()
	{
		foreach (var label in labels)
			label.Update();
	}

	void Refresh()
	{
		if (currentGun)
		{
			Destroy(currentGun.gameObject);
			currentGun = null;
		}

		if (currentIndex <= 8)
		{
			var gunLabelIndex = group * 3;

			currentGun = Instantiate(labels[gunLabelIndex].CurrentItem as GunData, new Vector3(0.5f, -0.1f, 0), Quaternion.AngleAxis(220, Vector3.up)) as GunData;

			var sight = Instantiate(labels[gunLabelIndex + 1].CurrentItem as Attachment) as Attachment;

			sight.transform.parent = currentGun.transform;
			sight.transform.localRotation = Quaternion.identity;
			sight.transform.localPosition = currentGun.SightPosition;

			var attatchment = Instantiate(labels[gunLabelIndex + 2].CurrentItem as Attachment) as Attachment;
			attatchment.transform.parent = currentGun.transform;
			attatchment.transform.localRotation = Quaternion.identity;
			attatchment.transform.localPosition = currentGun.AccessoryPosition;
		}
		else
			Instantiate(labels[currentIndex].CurrentItem as GameObject, new Vector3(0.5f, -0.1f, 0), Quaternion.AngleAxis(220, Vector3.up));
	}

	void OnGUI()
	{
		GUI.skin = skin;

		int y = 20;
		int height = 100;

		Drawing.DrawRect(new Rect(20, 20 + height * group, 340, height), Color.red);

		for (int i = 0; i < 6;)
		{
			labels[i++].Draw(new Rect(20, y, 200, height));
			labels[i++].Draw(new Rect(220, y, 140, height / 2));
			labels[i++].Draw(new Rect(220, y + height / 2, 140, height / 2));
			y += height;
		}

		if (GUI.Button(GameGUI.ConvertRect(new Rect(0.8f, 0.8f, 0.14f, 0.06f)), "DEPLOY"))
		{
			Soldier.staticEquipmentDatas = new EquipmentData[2];
			
			for (int i = 0; i < 2; ++i)
			{
				var gunData = labels[i * 3].CurrentItem as GunData;
				gunData.opticsPrefab = labels[i * 3 + 1].CurrentItem as Optics;
				gunData.accessoryPrefab = labels[i * 3 + 2].CurrentItem as Attachment;
				Soldier.staticEquipmentDatas[i] = gunData;
			}

			Options.main.currentEquipmentIndeces = new int[labels.Count];
			for (int i = 0; i < labels.Count; ++i)
				Options.main.currentEquipmentIndeces[i] = labels[i].CurrentIndex;

			Options.main.Save();

			Application.LoadLevel(level);
		}
	}
}
