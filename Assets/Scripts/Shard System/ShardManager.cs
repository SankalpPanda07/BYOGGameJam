using System;
using UnityEngine;

public class ShardManager : MonoBehaviour
{

    public static ShardManager instance;


    public static event Action<int> OnShardsChanged;

    // --- Shard Properties ---
    private int currentShards;
    private const int MaxShards = 5; // The maximum number of shards the player can hold.
    private const int StartingShards = 3; // The default number of shards for a new game.
    private const string ShardsSaveKey = "PlayerShardCount"; // The key used to save/load from PlayerPrefs.


    void Awake()
    {
        // Implement the Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene
        }
        else
        {
            // If another instance already exists, destroy this one.
            Destroy(gameObject);
            return;
        }

        // Load the saved shard data when the game starts.
        LoadShards();
    }

    void Update()
    {
        // For testing: Press 'R' to add a shard.
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddShards(1);
        }

        // For testing: Press 'E' to use/remove a shard.
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseShards(1);
        }
    }

    private void LoadShards()
    {
        // PlayerPrefs.GetInt will return the default value (StartingShards) if the key doesn't exist.
        currentShards = PlayerPrefs.GetInt(ShardsSaveKey, StartingShards);
        // Trigger the event to make sure any UI is updated on load.
        OnShardsChanged?.Invoke(currentShards);
    }

    private void SaveShards()
    {
        PlayerPrefs.SetInt(ShardsSaveKey, currentShards);
        PlayerPrefs.Save(); // Writes all modified preferences to disk.
    }


    public void AddShards(int amount)
    {
        currentShards += amount;
        // Ensure the shard count doesn't exceed the max.
        currentShards = Mathf.Clamp(currentShards, 0, MaxShards);

        Debug.Log($"Added {amount} shard(s). Current shards: {currentShards}");

        SaveShards();
        OnShardsChanged?.Invoke(currentShards);
    }

    public bool UseShards(int amount)
    {
        if (currentShards >= amount)
        {
            currentShards -= amount;
            // The minimum is already handled by the check above, so no need to clamp to 0.
            Debug.Log($"Used {amount} shard(s). Current shards: {currentShards}");

            SaveShards();
            OnShardsChanged?.Invoke(currentShards);
            return true;
        }

        Debug.Log($"Not enough shards! Current: {currentShards}, Tried to use: {amount}");
        return false;
    }

    public int GetCurrentShards()
    {
        return currentShards;
    }
}