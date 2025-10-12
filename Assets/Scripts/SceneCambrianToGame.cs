using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCambrianToGame : MonoBehaviour
{
    public SceneTransitionManager sceneTransitionManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sceneTransitionManager.ChangeScene("Level2");
        }

    }
}
