using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text level;
    [SerializeField] private Text honeyAmt;
    [SerializeField] private GameObject[] honeyJars;
    [SerializeField] private GameObject pauseBtn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // returns false if we've exceeded the max
    public bool AddHoney(int total)
    {
        honeyAmt.text = total.ToString();
        int jarsToVisible = Mathf.Min(total, honeyJars.Length);

        for (int i=0; i<jarsToVisible; i++)
        {
            honeyJars[i].SetActive(true);
        }

        if (total > honeyJars.Length) return false;
        return true;
    }

    public void ResetUI(int lvl, int maxHoney = 12)
    {
        level.text = lvl.ToString();
        // max currently cant be modified
        if (maxHoney != 12) Debug.LogError("Values other than 12 are currently not supported for max honey.");

        honeyAmt = 0.ToString();
        // clear the honey jars
        for (int i = 0; i < honeyJars.Length; i++)
        {
            honeyJars[i].SetActive(false);
        }
    }
}
