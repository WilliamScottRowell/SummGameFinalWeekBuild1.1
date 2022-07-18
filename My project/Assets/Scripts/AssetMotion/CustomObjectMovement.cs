using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CustomObjectMovement : MonoBehaviour
{
    /*
        Just attach this script to any game object to allow for custom movement, provided it has a trigger and that
        the player enters and presses a key (key is customizable)

        Script can be used for infinite movement, however, if toggled all children must have it toggled on or else there will be asynchronous motion

        To be honest, though powerful, it may be a hassle for other to incorporate and may just end up making others confused.
        So, I say do not make this a core script, I will just use this in my own dev team
    */

    public bool IsControlMaster = true;
    public bool IsMoving = true;
    public bool OnlyTriggerOnce = false;
    public TMP_Text floatingText;
    public bool ProximityBased = false;
    public string InputKey;
    public string ObjectName;
    public float MovementIncrement = 0.01f;
    public bool MoveForever = false;
    public float MovementTime = 2.0f;
    public bool RotateBackwards = false;
    public bool ObjectMovementReset = false;
    public float RotateXValue, RotateYValue, RotateZValue, LeftValue, RightValue, UpValue, DownValue;
    public CustomObjectMovement triggerObjectMethodNameTRIGGERONCE;
    public CustomObjectMovement triggerObject2MethodNameTRIGGERONCE;

    public bool inRangeToMove = false;
    bool animationPlaying = false;
    bool resetAfter = false;
    bool moveForever = false;
    bool isKeyDown = false;
    CustomObjectMovement[] movObjs;

    private void Start()
    {
        movObjs = GetComponentsInChildren<CustomObjectMovement>();
        if(floatingText != null)
        {
            Debug.Log("LOL");
            floatingText.text = "Press '" + InputKey.ToUpper() + "' to interact with " + ObjectName + " !";
            floatingText.enabled = false;
        }
        if(ObjectMovementReset)
        {
            resetAfter = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRangeToMove = true;
            if (floatingText != null)
            {
                floatingText.enabled = true;
            }
            if(ProximityBased)
            {
                if (IsMoving)
                {
                    if (MoveForever)
                    {
                        moveForever = !moveForever;
                    }
                    StartCoroutine(PlayMoveTranslation(resetAfter));
                    resetAfter = !resetAfter;
                    if (floatingText != null)
                    {
                        floatingText.enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < movObjs.Length; i++)
                    {
                        if (movObjs[i].GetMasterAndMove())
                        {
                            if (movObjs[i].GetMoveForever())
                            {
                                movObjs[i].ChangeMoveForever();
                            }
                            else if (!(movObjs[i].GetAnimationPlaying()))
                            {
                                movObjs[i].ThirdPartyAccess();
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRangeToMove = false;
            if (floatingText != null)
            {
                floatingText.enabled = false;
            }
            if(ProximityBased && MoveForever)
            {
                ChangeMoveForever();
            }
        }
    }
    public bool GetMasterAndMove()
    {
        return (!IsControlMaster) && IsMoving && (!animationPlaying || moveForever);
    }
    public bool GetMoveForever()
    {
        return moveForever;
    }
    public bool GetAnimationPlaying()
    {
        return animationPlaying;
    }
    public void ChangeMoveForever()
    {
        moveForever = !moveForever;
        animationPlaying = false;
        if (inRangeToMove)
        {
            if (floatingText != null)
            {
                floatingText.enabled = false;
            }
        }
    }
    void Update()
    {
        if(inRangeToMove)
        {
            if (!InputKey.Equals(null) && animationPlaying)
            {
                if (Input.GetKeyDown(InputKey))
                {
                    isKeyDown = true;
                }
            }
            if ((Input.GetKeyDown(InputKey) && !animationPlaying) && IsControlMaster)
            {
                if(IsMoving)
                {
                    if(MoveForever)
                    {
                        moveForever = !moveForever;
                    }
                    StartCoroutine(PlayMoveTranslation(resetAfter));
                    resetAfter = !resetAfter;
                    if (floatingText != null)
                    {
                        floatingText.enabled = false;
                    }
                }
                else
                {                   
                    for (int i = 0; i < movObjs.Length; i++)
                    {
                        if (movObjs[i].GetMasterAndMove())
                        {
                            if(movObjs[i].GetMoveForever())
                            {
                                movObjs[i].ChangeMoveForever();
                            }
                            else if(!(movObjs[i].GetAnimationPlaying()))
                            {
                                movObjs[i].ThirdPartyAccess();
                            }
                        }
                    }
                }
            }
        }
    }
    public void ThirdPartyAccess()
    {
        if (MoveForever)
        {
            moveForever = !moveForever;
        }
        StartCoroutine(PlayMoveTranslation(resetAfter));
        resetAfter = !resetAfter;
        if (floatingText != null)
        {
            floatingText.enabled = false;
        }
    }
    public void TriggerOnce()
    {
        StartCoroutine(PlayMoveTranslation(false));
    }
    IEnumerator PlayMoveTranslation(bool resetAfter)
    {
        animationPlaying = true;
        float timer = 0f;
        float timeLimit = MovementTime / MovementIncrement;
        float slidingLeftDelay = LeftValue / timeLimit;
        float slidingRightDelay = RightValue / timeLimit;
        float slidingUpDelay = UpValue / timeLimit;
        float slidingDownDelay = DownValue / timeLimit;
        float rotateXDelay = RotateBackwards ? -1f : 1f;
        float rotateYDelay = RotateBackwards ? -1f : 1f;
        float rotateZDelay = RotateBackwards ? -1f : 1f;
        rotateXDelay = rotateXDelay * RotateXValue / timeLimit;
        rotateYDelay = rotateYDelay * RotateYValue / timeLimit;
        rotateZDelay = rotateZDelay * RotateZValue / timeLimit;
        float backTrack = 1f;
        if(ObjectMovementReset && !resetAfter)
        {
            backTrack = -1f;
        }
        while (timer < timeLimit || moveForever)
        {
            timer++;
            this.transform.Rotate(new Vector3(rotateXDelay * backTrack, rotateYDelay * backTrack, rotateZDelay * backTrack), Space.World);
            this.transform.Translate(new Vector3((-slidingLeftDelay) * backTrack, 0, 0), Space.World);
            this.transform.Translate(new Vector3(slidingRightDelay * backTrack, 0, 0), Space.World);
            this.transform.Translate(new Vector3(0, slidingUpDelay * backTrack, 0), Space.World);
            this.transform.Translate(new Vector3(0, (-slidingDownDelay) * backTrack, 0), Space.World);
            if (timer >= timeLimit && !MoveForever)
            {
                animationPlaying = false;
                if(triggerObjectMethodNameTRIGGERONCE != null)
                {
                    triggerObjectMethodNameTRIGGERONCE.TriggerOnce();
                }
                if(triggerObject2MethodNameTRIGGERONCE != null)
                {
                    triggerObject2MethodNameTRIGGERONCE.TriggerOnce();
                }
                if (inRangeToMove)
                {
                    if (floatingText != null)
                    {
                        floatingText.enabled = false;
                    }
                }
                if(OnlyTriggerOnce)
                {
                    this.enabled = false;
                }
            }
            if(timer > 2f && moveForever && IsControlMaster && isKeyDown)
            {
                animationPlaying = false;
                moveForever = !moveForever;
                isKeyDown = false;
                Debug.Log("Start");
            }
            if(!moveForever && MoveForever)
            {
                if(OnlyTriggerOnce)
                {
                    this.enabled = false;
                }
                break;
            }
            yield return new WaitForSeconds(MovementIncrement);
        }
    }
}