using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
	public static DataPersistenceManager Instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;

    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if (Instance == null)
        {
            Debug.LogError("Found more than one DataPersistenceManager in the scene.");
		}
        Instance = this;
	}

	private void Start()
	{
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
	}
	public void Newgame()
    {
        this.gameData = new GameData();
	}

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data was found.");
            Newgame();
        }
        
        foreach (IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
        Debug.Log("Loaded deathcount: " + gameData.deathCount);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }
        Debug.Log("Saved deathcount: " + gameData.deathCount);
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
	}

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
