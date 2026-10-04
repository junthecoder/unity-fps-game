using UnityEngine;
using System.Collections;

public class Options
{
	public float sensitivity = 5;
	public float fov = 60;
	public bool showHelp = true;

	public int[] currentEquipmentIndeces = new int[] { 0, 0, 0, 1, 1, 1 };

	static Options options;
	public static Options main
	{
		get
		{
			if (options == null) Load();
			return options;
		}
	}

	static string OptionPath
	{
		get
		{
			return System.IO.Path.Combine(System.Environment.CurrentDirectory, "options.xml");
		}
	}

	static void Load()
	{
		var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Options));
		try
		{
			using (var fs = new System.IO.FileStream(OptionPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
			{
				options = serializer.Deserialize(fs) as Options;
				if (options != null)
					return;

				throw new System.InvalidOperationException("options.xml did not contain an Options value.");
			}
		}
		catch (System.IO.FileNotFoundException ex)
		{
			Debug.LogWarning("options.xml was not found; using default options. " + ex.Message);
		}
		catch (System.IO.DirectoryNotFoundException ex)
		{
			Debug.LogWarning("The options directory was not found; using default options. " + ex.Message);
		}
		catch (System.Xml.XmlException ex)
		{
			Debug.LogWarning("options.xml is malformed; using default options. " + ex.Message);
		}
		catch (System.InvalidOperationException ex)
		{
			Debug.LogWarning("options.xml could not be read; using default options. " + ex.Message);
		}
		catch (System.IO.IOException ex)
		{
			Debug.LogWarning("options.xml could not be opened; using default options. " + ex.Message);
		}
		catch (System.UnauthorizedAccessException ex)
		{
			Debug.LogWarning("options.xml is not readable; using default options. " + ex.Message);
		}
		catch (System.Security.SecurityException ex)
		{
			Debug.LogWarning("options.xml cannot be accessed; using default options. " + ex.Message);
		}

		options = new Options();
		options.Save();
	}

	public void Save()
	{
		System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Options));
		try
		{
			using (var sw = new System.IO.StreamWriter(OptionPath, false, System.Text.Encoding.UTF8))
				serializer.Serialize(sw, this);
		}
		catch (System.IO.IOException ex)
		{
			Debug.LogWarning("Could not save options.xml; current settings remain available for this session. " + ex.Message);
		}
		catch (System.UnauthorizedAccessException ex)
		{
			Debug.LogWarning("Could not save options.xml because settings storage is not writable; current settings remain available for this session. " + ex.Message);
		}
		catch (System.Security.SecurityException ex)
		{
			Debug.LogWarning("Could not save options.xml because settings storage is not writable; current settings remain available for this session. " + ex.Message);
		}
	}

	public bool ShowGUI()
	{
		var itemHeight = 0.08f;
		var y = 0.1f;
		var labelRect = new Rect(0.1f, y, 0.2f, itemHeight);
		var sliderRect = new Rect(labelRect.xMax, y, 0.5f, itemHeight);
		var numberRect = new Rect(sliderRect.xMax + 0.05f, y, 0.05f, itemHeight);
		labelRect = GameGUI.ConvertRect(labelRect);
		sliderRect = GameGUI.ConvertRect(sliderRect);
		numberRect = GameGUI.ConvertRect(numberRect);

		GUI.Label(labelRect, "SENSITIVITY");
		options.sensitivity = GUI.HorizontalSlider(sliderRect, options.sensitivity, 0, 30);
		GUI.Label(numberRect, string.Format("{0:f1}", options.sensitivity));

		labelRect.y += labelRect.height;
		sliderRect.y += sliderRect.height;
		numberRect.y += numberRect.height;

		GUI.Label(labelRect, "FOV");
		options.fov = GUI.HorizontalSlider(sliderRect, options.fov, 60, 90);
		GUI.Label(numberRect, "" + (int)options.fov);

		if (GameGUI.BackButton())
		{
			Save();
			return true;
		}
		
		return false;
	}
}
