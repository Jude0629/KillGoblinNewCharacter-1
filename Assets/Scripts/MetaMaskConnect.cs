using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MetaMaskConnect : MonoBehaviour
{
    public string sceneName;
    public void OpenMetaMask()
    {
        SceneManager.LoadScene(sceneName);
    }
}
