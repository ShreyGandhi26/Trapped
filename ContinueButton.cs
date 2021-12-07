using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    private string scenename;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        Scene currentscene = SceneManager.GetActiveScene();
        scenename = currentscene.name;
    }

    public void Continue()
    {
        if (scenename == "Chapter_1")
        {
            SceneManager.LoadScene("Chapter_2");
        }
        else if (scenename == "Chapter_2")
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void back()
    {
        if (scenename == "Chapter_1")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (scenename == "Chapter_2")
        {
            SceneManager.LoadScene("Chapter_1");
        }
    }
}
