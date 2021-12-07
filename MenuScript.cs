using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject dialougeSystem;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialougeManager.isDialougeOver == true)
        {
            // Time.timeScale = 1f;
            dialougeSystem.SetActive(false);
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

}
