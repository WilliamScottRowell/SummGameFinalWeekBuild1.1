using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToHub : MonoBehaviour
{
    public string hubWorldName;

    public void ReturnToHubWorld()
    {
        SceneManager.LoadScene(hubWorldName);
    }
}
