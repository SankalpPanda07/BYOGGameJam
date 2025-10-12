using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathh : MonoBehaviour
{

    public GameObject DeathPanel;

    private void Start()
    {
        DeathPanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            DeathPanel.SetActive(true);
        }
    }
}
