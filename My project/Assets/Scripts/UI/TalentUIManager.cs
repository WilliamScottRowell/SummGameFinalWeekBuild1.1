using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentUIManager : MonoBehaviour
{
    public Canvas talentSystemUI;
    public Canvas pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        talentSystemUI.gameObject.SetActive(false);
    }

    public void TurnOnTalentUI()
    {
        talentSystemUI.gameObject.SetActive(true);
        pauseMenuUI.gameObject.SetActive(false);
    }

    public void TurnOffTalentUI()
    {
        talentSystemUI.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(true);
    }
}
