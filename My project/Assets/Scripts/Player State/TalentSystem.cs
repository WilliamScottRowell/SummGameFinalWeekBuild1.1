using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentSystem : MonoBehaviour
{
    public int numTalentPoints;
    int maxTalentPoints;

    public TalentUIManager UIManager;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            UIManager.TurnOffTalentUI();
        }

        Cursor.lockState = CursorLockMode.Confined;

    }

    public void AddTalentPoints(int points)
    {
        numTalentPoints += points;
        maxTalentPoints += points;
    }
    public bool HaveTalentPoints()
    {
        return numTalentPoints > 0;
    }
    public int GetNumTalentPoints()
    {
        return numTalentPoints;
    }
    public void SpendTalentPoints(int num)
    {
        numTalentPoints -= num;
        if(numTalentPoints < 0)
        {
            Debug.Log("Issue spending more talent points than you hold!");
        }
        UpdateTalentUIData();
    }
    // Talent Skill Tree Code

    // UI Components
    public TMP_Text numTalentsText;
    public TMP_Text manaText;
    public TMP_Text magicDmgText;
    public TMP_Text phyDmgText;
    public TMP_Text hpText;

    public Button manaUpButton;
    public Button magicDmgButton;
    public Button phyDmgButton;
    public Button hpUpButton;

    // Talent data
    string speedUpBaseText = "Speed increase: ";
    string jumpsUpBaseText = "Num jumps: ";
    string hpUpBaseText = "Total hp increase: ";

    public int manaProgressionState;
    public int magicDmgProgressionState;
    public int phyDmgProgressionState;
    public int hpTalentProgressionState;

    private void OnEnable()
    {
        UpdateTalentUIData();
    }

    public void UpdateTalentUIData()
    {
        SetTalentInfoCards();
        SetSkillTreeInfoCard();
        SetButtonInteractability();
    }
    // Reset UI text to base states
    void SetTalentInfoCards()
    {
        numTalentsText.text = "" + numTalentPoints;
        manaText.text = "" + manaProgressionState;
        magicDmgText.text = "" + magicDmgProgressionState;
        phyDmgText.text = "" + phyDmgProgressionState;
        hpText.text = "" + hpTalentProgressionState;
    }
    void SetButtonInteractability()
    {
        if(numTalentPoints < 1)
        {
            manaUpButton.interactable = false;
        }
        else
        {
            manaUpButton.interactable = true;
        }
        if(numTalentPoints < 1)
        {
            magicDmgButton.interactable = false;
        }
        else
        {
            magicDmgButton.interactable = true;
        }
        if(numTalentPoints < 1)
        {
            phyDmgButton.interactable = false;
        }
        else
        {
            phyDmgButton.interactable = true;
        }
        if (numTalentPoints < 1)
        {
            hpUpButton.interactable = false;
        }
        else
        {
            hpUpButton.interactable = true;
        }
        if(numTalentPoints < 10 || skillTreeProgression != 0) {
            dashButton.interactable = false;
        }
        else dashButton.interactable = true;
        if(numTalentPoints < 10 || skillTreeProgression != 1) {
            iceButton.interactable = false;
        }
        else iceButton.interactable = true;
        if(numTalentPoints < 10 || skillTreeProgression != 2) {
            damageReduction1Button.interactable = false;
        }
        else damageReduction1Button.interactable = true;
        if(numTalentPoints < 15 || skillTreeProgression != 3) {
            windButton.interactable = false;
        }
        else windButton.interactable = true;
        if(numTalentPoints < 10 || skillTreeProgression != 4) {
            damageReduction2Button.interactable = false;
        }
        else damageReduction2Button.interactable = true;
        if(numTalentPoints < 15 || skillTreeProgression != 5) {
            lightningButton.interactable = false;
        }
        else lightningButton.interactable = true;
        if(numTalentPoints < 15 || skillTreeProgression != 6) {
            extraLifeButton.interactable = false;
        }
        else extraLifeButton.interactable = true;
        if((numTalentPoints < 7 || skillTreeProgression < 3) || cool1) {
            cooldownReduction1Button.interactable = false;
        }
        else cooldownReduction1Button.interactable = true;
        if((numTalentPoints < 7 || skillTreeProgression < 5) || cool2) {
            cooldownReduction2Button.interactable = false;
        }
        else cooldownReduction2Button.interactable = true;
    }
    public void PurchaseManaUpgrade()
    {
        //numTalentPoints -= speedTalentProgressionState + 1;
        numTalentPoints--;
        manaProgressionState++;
        UpdateTalentUIData();
    }
    public void PurchaseMagicUpgrade()
    {
        //numTalentPoints -= jumpTalentProgressionState + 1;
        numTalentPoints--;
        magicDmgProgressionState++;
        UpdateTalentUIData();
    }
    public void PurchasePhysicalUpgrade()
    {
        //numTalentPoints -= hpTalentProgressionState + 1;
        numTalentPoints--;
        phyDmgProgressionState++;
        UpdateTalentUIData();
    }
    public void PurchaseHPUpgrade()
    {
        //numTalentPoints -= hpTalentProgressionState + 1;
        numTalentPoints--;
        hpTalentProgressionState++;
        UpdateTalentUIData();
    }
    public void ResetTalentPoints()
    {
        numTalentPoints = maxTalentPoints;
        UpdateTalentUIData();
        ResetProgressionStates();
        SetTalentInfoCards();
    }
    void ResetProgressionStates()
    {
        manaProgressionState = 0;
        magicDmgProgressionState = 0;
        phyDmgProgressionState = 0;
        hpTalentProgressionState = 0;
    }
    public TMP_Text dashUnlockText;
    public TMP_Text iceSpikesUnlockText;
    public TMP_Text windBladesUnlockText;
    public TMP_Text lightningUnlockText;
    public TMP_Text dmgRed1UnlockText;
    public TMP_Text dmgRed2UnlockText;
    public TMP_Text cool1UnlockText;
    public TMP_Text cool2UnlockText;
    public TMP_Text extraLife1UnlockText;

    public Button dashButton;
    public Button iceButton;
    public Button windButton;
    public Button lightningButton;
    public Button damageReduction1Button;
    public Button damageReduction2Button;
    public Button cooldownReduction1Button;
    public Button cooldownReduction2Button;
    public Button extraLifeButton;

    public int size;
    public float cooldownReduction, damageReduction;

    int skillTreeProgression;

    //vairbales for non linear branch nodes
    bool cool1, cool2;
    
    void SetSkillTreeInfoCard() {
        //main branch
        if(skillTreeProgression > 0) {
            dashUnlockText.text = "DASH UNLOCKED";
        }
        else dashUnlockText.text = "Required Points: 10";
        if(skillTreeProgression > 1) {
            iceSpikesUnlockText.text = "ICE UNLOCKED";
        }
        else iceSpikesUnlockText.text = "Required Points: 10";
        if(skillTreeProgression > 2) {
            dmgRed1UnlockText.text = "DMG RED 1 UNLOCKED";
        }
        else dmgRed1UnlockText.text = "Required Points: 10";
        if(skillTreeProgression > 3) {
            windBladesUnlockText.text = "WIND UNLOCKED";
        }
        else windBladesUnlockText.text = "Required Points: 15";
        if(skillTreeProgression > 4) {
            dmgRed2UnlockText.text = "DMG RED 2 UNLOCKED";
        }
        else dmgRed2UnlockText.text = "Required Points: 10";
        if(skillTreeProgression > 5) {
            lightningUnlockText.text = "LIGHTNING UNLOCKED";
        }
        else lightningUnlockText.text = "Required Points: 15";
        if(skillTreeProgression > 6) {
            extraLife1UnlockText.text = "EXTRA LIFE UNLOCKED";
        }
        else extraLife1UnlockText.text = "Required Points: 15";
        //side branch
        if(cool1) {
            cool1UnlockText.text = "COOLDOWN 1 UNLOCKED";
        }
        else cool1UnlockText.text = "Required Points: 7";
        if(cool2) {
            cool2UnlockText.text = "COOLDOWN 2 UNLOCKED";
        }
        else cool2UnlockText.text = "Required Points: 7";
    }
    //Main Branch
    public void PurchaseDash() {
        if(skillTreeProgression != 0) {
            return;
        }
        skillTreeProgression++;
        numTalentPoints -= 10;
        UpdateTalentUIData();
    }
    public void PurchaseIceSpikes() {
        if(skillTreeProgression != 1) {
            return;
        }
        skillTreeProgression++;
        numTalentPoints -= 10;
        UpdateTalentUIData();
    }
    public void PurchaseDamageReduction1() {
        if(skillTreeProgression == 2) {
            skillTreeProgression++;
            numTalentPoints -= 10;
            UpdateTalentUIData();
        }
    }
    public void PurchaseWindBlades() {
        if(skillTreeProgression != 3) {
            return;
        }
        skillTreeProgression++;
        numTalentPoints -= 15;
        UpdateTalentUIData();
    }
    public void PurchaseDamageReduction2() {
        if(skillTreeProgression == 4) {
            skillTreeProgression++;
            numTalentPoints -= 10;
            UpdateTalentUIData();
        }
    }
    public void PurchaseLightning() {
        if(skillTreeProgression != 5) {
            return;
        }
        skillTreeProgression++;
        numTalentPoints -= 15;
        UpdateTalentUIData();
    }
    public void PurchaseExtraLife1() {
        if(skillTreeProgression == 6) {
            skillTreeProgression++;
            numTalentPoints -= 15;
            UpdateTalentUIData();
        }
    }

    //Side branch
    public void PurchaseCooldown1() {
        if(skillTreeProgression > 2) {
            numTalentPoints -= 7;
            cool1 = true;
            UpdateTalentUIData();
        }
    }
    public void PurchaseCooldown2() {
        if(skillTreeProgression > 4) {
            numTalentPoints -= 7;
            cool2 = true;
            UpdateTalentUIData();
        }
    }
}
