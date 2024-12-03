using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCameraController : MonoBehaviour
{
    public Camera resultCamera;  // ResultCameraをインスペクタで設定
    public float rotationSpeed = 20f;  // 回転速度（調整可能）
    public float targetYPosition = 4f; // 最終的に高さ4にする
    public float targetRotationY = 90f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        // まず移動させ、次に回転
        MoveCameraToTargetPosition();
        RotateCameraToTarget();
    }

    private void MoveCameraToTargetPosition()
    {
        // 現在のカメラ位置を取得
        Vector3 currentPosition = resultCamera.transform.position;

        // 指定の位置までスムーズに移動（補間）
        float step = 0.05f;  // 移動スピード（調整可能）
        resultCamera.transform.position = Vector3.MoveTowards(currentPosition, new Vector3(currentPosition.x, targetYPosition, currentPosition.z), step);
    }

    private void RotateCameraToTarget()
    {
        // 現在の回転を取得
        Quaternion currentRotation = resultCamera.transform.rotation;

        // 目標回転（目標角度をQuaternionに変換）
        Quaternion targetRotation = Quaternion.Euler(90, targetRotationY, 0); // x軸90度、y軸目標回転、z軸0度

        // 回転を目標角度に向けて補間
        resultCamera.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
