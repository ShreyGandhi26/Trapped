using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftSystem : MonoBehaviour
{
    public BoxCollider bc;

    private void Start()
    {
        bc.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (PlayerController.Interacted == true)
        {
            bc.isTrigger = true;
        }
        else
        {
            bc.isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            SceneManager.LoadScene("Level 2");
        }
    }

}
