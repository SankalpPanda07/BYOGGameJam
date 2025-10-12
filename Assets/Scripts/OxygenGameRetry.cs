using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OxygenGameRetry : MonoBehaviour
{

    public void RetryOxygen()
    {
        ShardManager.instance.UseShards(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
