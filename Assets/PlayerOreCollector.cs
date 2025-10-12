using UnityEngine;

public class PlayerOreCollector : MonoBehaviour
{
    public int collectedOres = 0;

    public void CollectOre()
    {
        collectedOres++;
        Debug.Log("Collected Ore! Total: " + collectedOres);
    }
}
