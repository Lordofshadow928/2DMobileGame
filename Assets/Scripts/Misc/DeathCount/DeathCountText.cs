using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour
{
    private int deathCount = 0;

    private TextMeshProUGUI deathCountText;

    private void Awake()
    {
        deathCountText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
    }
    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
    }

    public void SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
    }

    private void OnPlayerDeath()
    {
        deathCount++;   
    }

    private void Update()
    {
        deathCountText.text = "Deaths: " + deathCount;
    }
}
