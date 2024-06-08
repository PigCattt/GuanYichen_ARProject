using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWall : MonoBehaviour
{
    public float rotationSpeed = 30f; // 旋转速度，度/秒
    public float targetAngle = 30f; // 目标旋转角度
    private bool isRotating = false;
    private bool rotateClockwise = true; // 用于切换旋转方向

    public void StartRotation()
    {
        if (!isRotating)
        {
            float angle = rotateClockwise ? targetAngle : -targetAngle;
            StartCoroutine(RotateByAngle(angle));
            rotateClockwise = !rotateClockwise; // 切换旋转方向
        }
    }

    private IEnumerator RotateByAngle(float angle)
    {
        isRotating = true;
        float rotatedAngle = 0f;

        while (Mathf.Abs(rotatedAngle) < Mathf.Abs(angle))
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            float actualRotation = Mathf.Min(rotationStep, Mathf.Abs(angle) - Mathf.Abs(rotatedAngle))* Mathf.Sign(angle);
            
            // 旋转物体
            transform.Rotate(0, actualRotation,0,Space.Self);
            rotatedAngle += actualRotation; // 保持方向
            yield return null;
        }

        // 确保物体到达精确的目标角度
        float remainingAngle = angle - rotatedAngle;
        if (remainingAngle != 0)
        {
            transform.Rotate(0, remainingAngle,0,Space.Self);
        }

        isRotating = false;
    }
}
