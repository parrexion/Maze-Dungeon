using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class SaveController : MonoBehaviour {

	#region Singleton
	private static SaveController _instance;

	private void Start() {
		if (_instance != null) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			_instance = this;
			Initialize();
		}
	}
	#endregion

	[Header("Save Data")]
	public BoolVariable isSinglePlayer;
	public IntVariable p1Index;
	public IntVariable p2Index;
	public IntVariable bestScore;

	private string _savePath = "";
	private string _backupSavePath = "";
	private SavePackage saveFileData;

	public UnityEvent loadFinishedEvent;


	private void Initialize() {
		_savePath = Application.persistentDataPath+"/saveData.xml";
		_backupSavePath = Application.streamingAssetsPath+"/saveData.xml";
		PreLoad();
		// EmptySave();
	}

	public void EmptySave() {
		saveFileData = new SavePackage();

		// Setup save data
		saveFileData.p1Index = p1Index.value = 0;
		saveFileData.p1Index = p2Index.value = 1;

		//Write to file
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
		XmlSerializer serializer = new XmlSerializer(typeof(SavePackage));
		using (XmlWriter xmlWriter = XmlWriter.Create(_savePath, xmlWriterSettings)) {
			serializer.Serialize(xmlWriter, saveFileData);
		}
		Debug.Log("Successfully saved new save data!\n" + _savePath);
	}


	public void Save() {
		// Write values
		saveFileData.isSinglePlayer = isSinglePlayer.value;
		saveFileData.p1Index = p1Index.value;
		saveFileData.p2Index = p2Index.value;
		saveFileData.bestScore = bestScore.value;
		
		//Write to file
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
		XmlSerializer serializer = new XmlSerializer(typeof(SavePackage));
		using (XmlWriter xmlWriter = XmlWriter.Create(_savePath, xmlWriterSettings)) {
			serializer.Serialize(xmlWriter, saveFileData);
		}
		Debug.Log("Successfully saved the save data!\n" + _savePath);
	}


	/// <summary>
	/// Does a pre load which is used to show which save files are used to
	/// give the player a sense of which save file to load.
	/// </summary>
	public void PreLoad() {
		string path = _savePath;
		if (!File.Exists(_savePath)){
			path = _backupSavePath;
			Debug.LogWarning("No save file found: " + path);
			EmptySave();
		}
		else {
			XmlSerializer serializer = new XmlSerializer(typeof(SavePackage));
			FileStream file = File.Open(path,FileMode.Open);
			saveFileData = serializer.Deserialize(file) as SavePackage;
			file.Close();
		}

		if (saveFileData == null) {
			Debug.LogError("Could not open the file: " + path);
			return;
		}

		// Read values
		isSinglePlayer.value = saveFileData.isSinglePlayer;
		p1Index.value = saveFileData.p1Index;
		p2Index.value = saveFileData.p2Index;
		bestScore.value = saveFileData.bestScore;
		
		Debug.Log("Successfully pre-loaded the save data!");
	}
}


[System.Serializable]
public class SavePackage {
	public bool isSinglePlayer;
	public int p1Index;
	public int p2Index;
	public int bestScore;
}