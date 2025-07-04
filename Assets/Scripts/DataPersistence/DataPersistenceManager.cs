using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
	public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            //Instance = this;
            //DontDestroyOnLoad(gameObject);
            Debug.LogError("Found more than one DataPersistenceManager in the scene.");
		}
        Instance = this;
	}

	private void Start()
	{
        LoadGame();
	}
	public void Newgame()
    {
        this.gameData = new GameData();
	}

    public void LoadGame()
    {
        if (this.gameData == null)
        {
            Debug.Log("No data was found.");
            Newgame();
        }
        
	}

    public void SaveGame()
    {

    }

    private void OnApplicationQuit()
    {
        SaveGame();
	}
}
