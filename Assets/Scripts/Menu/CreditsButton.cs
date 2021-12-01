using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    
    public void LoadCredits()
    {
        SceneManager.LoadScene(4);
    }

}
