using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ObjectRotater : MonoBehaviour
{
    Quaternion startingRotation;

    public float rotationSpeed;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    private void Start()
    {
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        if(rotateX)
        {
            x = rotationSpeed;
        }
        if(rotateY)
        {
            y = rotationSpeed;
        }
        if(rotateZ)
        {
            z = rotationSpeed;
        }

        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);


    }

    public void ResetRotation()
    {
        gameObject.transform.rotation = startingRotation;
    }
}

[CustomEditor(typeof(ObjectRotater))]
public class RotaterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset Rotation Data"))
        {
            ObjectRotater obj = (ObjectRotater)target;
            obj.ResetRotation();
        }
    }
}

