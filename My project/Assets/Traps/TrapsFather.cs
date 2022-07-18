using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsFather : MonoBehaviour //this is for damage player
{
    public GameObject ouchUi;
    public float countdownTime = 3.0f;
    private bool offUi = false;


    // Start is called before the first frame update
     void Start()
     {
        ouchUi.SetActive(false);
     }
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            ouchUi.SetActive(true);
            offUi = true;
            
        }
    }

    protected void CountDownUi()
    {
        if(offUi == true)
        {
            countdownTime -= Time.deltaTime;
        }
    }

    void RemoveUi()
    {
        if (countdownTime <= 0)
        {
            Debug.Log("ok1");
            ouchUi.SetActive(false);
            offUi = false;
            countdownTime = 3.0f;
        }
    }
    // Update is called once per frame
    protected void Update()
    {
        CountDownUi();
        RemoveUi();
    }
}
