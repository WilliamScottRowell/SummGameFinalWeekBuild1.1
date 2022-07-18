using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
public class TrapTrigger : MonoBehaviour
{
    public TrapRotate wreckingBallScript;
    public FallingTrap fallScript;
    public ArrowTrap arrowScript;
    

    
    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "WreckingBallPressurePlate")
        {
            wreckingBallScript.begin = true;

        }
        if (other.gameObject.CompareTag("FallingTrapPressurePlate"))
        {
            fallScript.fall = true;
        }
        if (other.gameObject.CompareTag("ArrowTrapPressurePlate"))
        {
            arrowScript.shoot = true;
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            Destroy(other.gameObject);
        }

    }
}
