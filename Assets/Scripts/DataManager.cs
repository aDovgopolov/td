using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{	
	[Serializable]
	public class PlayerData
	{
		public int points;
	}

	private int points = 0;

	private static DataManager _instance;

	public static DataManager Instance
	{
		get
		{
			if (_instance == null)
			{

				Debug.LogError("Error");
			}
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	public void PreloadDataFile()
	{
		string nFile = @"C:/Users/Alex/AppData/LocalLow/xlow/Tetris/playerInfo.data";

		if (File.Exists(nFile))
		{
			Debug.Log("File already exists!");
		}
		else
		{
			Debug.Log("File not exists!");
			FileStream fs = new FileStream(nFile, FileMode.CreateNew, FileAccess.ReadWrite);
			fs.Close();
			Save(0); // default data
		}
	}

	public void Save(int playerCount)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.Open);

		PlayerData data = new PlayerData
		{
			points = playerCount
		};

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		PreloadDataFile();

		if (File.Exists(Application.persistentDataPath + "/playerInfo.data"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.OpenOrCreate);

			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			points = data.points;
		}
		UIManager.Instance.SetTopPlayerCount(points);
	}
}
