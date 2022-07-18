using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReloadHelper : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ResetSceneController resetSceneController = GameObject.Find("GameManager").GetComponent<ResetSceneController>();
        resetSceneController.ResetAllScripts();
    }

}
