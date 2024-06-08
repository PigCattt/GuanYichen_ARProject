using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fold_Unfold : MonoBehaviour
{
    public List<GameObject> myObjs;
    public List<float> angles; // List of angles for each object
    public float speed; // Speed is now public
    public GameObject translateObject; // The object to transform after rotations
    public float translateDistance;
    public float translateSpeed;
    private List<float> intervals; // List of intervals for each object
    private int currentObjIndex = 0;
    private float nextRotationTime;
    private bool startRotation = false; // Flag to control rotation start
    private bool startTranslate = false; // Flag to check if the object has been transformed

    void Start()
    {
        if (myObjs.Count == 0 || angles.Count == 0 || myObjs.Count != angles.Count)
        {
            Debug.LogError("Ensure myObjs and angles lists are assigned and have the same length.");
            return;
        }

        intervals = new List<float>();
        for (int i = 0; i < angles.Count; i++)
        {
            intervals.Add(angles[i] / speed);
        }
    }

    void Update()
    {
        // Rotate objects if rotation has started
        if (startRotation)
        {
            RotateObjects();
        }

        // Translate object if translation has started
        if (startTranslate)
        {
            TranslateObject();
        }
    }

    private void RotateObjects()
    {
        if (Time.time < nextRotationTime && currentObjIndex < myObjs.Count)
        {
            RotateOneObj(myObjs[currentObjIndex]);
        }
        else if (currentObjIndex < myObjs.Count)
        {
            currentObjIndex++;
            if (currentObjIndex < myObjs.Count)
            {
                nextRotationTime = Time.time + intervals[currentObjIndex];
            }
            else
            {
                startRotation = false; // Stop rotation after all objects have been rotated
            }
        }
    }

    private void RotateOneObj(GameObject obj)
    {
        obj.transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self); // Only rotate around the y-axis
    }

    private void TranslateObject()
    {
        Vector3 newPosition = translateObject.transform.position - new Vector3(0f, translateDistance, 0f);

        translateObject.transform.position = Vector3.Lerp(translateObject.transform.position, newPosition, translateSpeed * Time.deltaTime);

        if (Vector3.Distance(translateObject.transform.position, newPosition) < 0.01f)
        {
            startTranslate = false;
        }
        else if (Vector3.Distance(translateObject.transform.position, newPosition) <= 0.1f)
        {
            translateObject.transform.position = newPosition;
            startTranslate = false;
        }
    }

    // This method should be called when the button is pressed
    public void StartRotation()
    {
        if (!startRotation)
        {
            currentObjIndex = 0; // Reset index
            startRotation = true;
            nextRotationTime = Time.time + intervals[currentObjIndex]; // Initialize nextRotationTime
        }
        if (!startTranslate)
        {
            startTranslate = true;
        }
    }
}