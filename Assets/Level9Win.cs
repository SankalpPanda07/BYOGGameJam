using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9Win : MonoBehaviour
{

    public GameObject winPanel;

    private void Start()
    {
        winPanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            winPanel.SetActive(true);
        }
    }
}
