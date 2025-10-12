using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace

/// <summary>
/// Updates a TextMeshPro UI element to display the current shard count.
/// This script subscribes to the ShardManager's OnShardsChanged event.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class ShardDisplayUI : MonoBehaviour
{
    private TMP_Text shardText;

    /// <summary>
    /// Called when the script is first loaded.
    /// </summary>
    void Awake()
    {
        // Get the reference to the TextMeshPro component on this GameObject.
        shardText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        // Subscribe our UpdateShardText method to the OnShardsChanged event.
        // Now, whenever the event is triggered, our method will be called.
        ShardManager.OnShardsChanged += UpdateShardText;

        // Also, update the text immediately with the current value when the scene loads.
        if (ShardManager.instance != null)
        {
            UpdateShardText(ShardManager.instance.GetCurrentShards());
        }
    }

    /// <summary>
    /// Called when the object becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        // IMPORTANT: Unsubscribe from the event to prevent errors and memory leaks
        // when the object is destroyed or the scene changes.
        ShardManager.OnShardsChanged -= UpdateShardText;
    }

    /// <summary>
    /// Updates the text display with the new shard count.
    /// This method is called by the OnShardsChanged event from the ShardManager.
    /// </summary>
    /// <param name="shardCount">The new number of shards.</param>
    private void UpdateShardText(int shardCount)
    {
        if (shardText != null)
        {
            // You can format the text however you like.
            shardText.text = $"Shards: {shardCount}";
        }
    }
}