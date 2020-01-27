using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject[] levels;
    public int currentLevel = 0;

    public void ChangeTileLevel()
    {
        if (++currentLevel >= levels.Length) currentLevel = 0;
        ActiveLevel(currentLevel);
    }

    void ActiveLevel(int level)
    {
        if (levels.Length > level)
        {
            for (int i=0; i<levels.Length; i++)
            {
                if (i == level) levels[i].SetActive(true);
                else levels[i].SetActive(false);
            }
        }
    }
}
