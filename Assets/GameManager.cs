using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] public GameObject BGM;
    [SerializeField] public UIManager ui;


    private int honeyJars = 0;
    private int level = 1;

    private void Awake()
    {
        // singleton object
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        
    }

    public void addHoneyJars(int num = 1)
    {
        honeyJars += num;
        ui.AddHoney(honeyJars);
        Debug.Log("HoneyJars = " + honeyJars);
    }

    public void clearHoneyJars()
    {
        honeyJars = 0;
        ui.ResetUI(level);
    }

}
