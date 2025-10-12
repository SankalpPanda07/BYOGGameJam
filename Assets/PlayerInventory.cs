using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ForgeBox : MonoBehaviour
{
    public int oresNeededForSword = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerOreCollector collector = other.GetComponent<PlayerOreCollector>();
            if (collector != null)
            {
                if (collector.collectedOres >= oresNeededForSword)
                    Debug.Log("Sword has been forged!");
                else
                    Debug.Log("Collect more ores!");
            }
        }
    }
}
