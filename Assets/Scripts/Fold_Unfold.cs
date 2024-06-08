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
    private Vector3 initialPosition; // The initial position of the translateObject
    private bool isFolding = false; // Flag to track whether folding or unfolding

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

        if (translateObject != null)
        {
            initialPosition = translateObject.transform.position;
        }
    }

    void Update()
    {
        if (startRotation)
        {
            // Rotate the current object if within the interval
            if (Time.time < nextRotationTime)
            {
                RotateOneObj(myObjs[currentObjIndex]);
            }
            // Move to the next object when the interval elapses
            else if (currentObjIndex < myObjs.Count)
            {
                currentObjIndex++;
                if (currentObjIndex < myObjs.Count)
                {
                    nextRotationTime = Time.time + intervals[currentObjIndex]; // Set the time for the next rotation
                }
                else
                {
                    startRotation = false; // Stop rotation after all objects have been rotated
                }
            }
        }

        if (startTranslate && translateObject != null)
        {
            TranslateObject();
        }
    }

    private void RotateOneObj(GameObject obj)
    {
        float rotationSpeed = isFolding ? speed : -speed;
        obj.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self); // Rotate around the y-axis only
    }

    private void TranslateObject()
    {
        // Calculate the target position
        float direction = isFolding ? -1f : 1f;
        Vector3 targetPosition = initialPosition + new Vector3(0f, direction * translateDistance, 0f);

        // Move the object towards the target position using MoveTowards
        translateObject.transform.position = Vector3.MoveTowards(translateObject.transform.position, targetPosition, translateSpeed * Time.deltaTime);

        // Check if the object has reached the target position
        if (translateObject.transform.position == targetPosition)
        {
            startTranslate = false; // Stop translating
        }
    }

    // This method should be called when the button is pressed
    public void StartRotation()
    {
        if (!startRotation)
        {
            currentObjIndex = 0; // Reset index
            startRotation = true;
            if (isFolding)
            {
                nextRotationTime = Time.time + intervals[currentObjIndex]; // Initialize nextRotationTime
            }
            else
            {
                nextRotationTime = Time.time + intervals[intervals.Count - 1]; // Initialize nextRotationTime for unfold
            }
        }

        if (translateObject != null)
        {
            startTranslate = true;
            initialPosition = translateObject.transform.position; // Reset initial position
        }

        isFolding = !isFolding; // Toggle between folding and unfolding
    }
}
