using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 360.0f;
 
    GameObject camera;  // 外部のオブジェクトを参照する
 
    Animator animator;
 
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = this.GetComponentInChildren<Animator>();
    }
 
    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.GetAxis("D-pad X"));
        // Debug.Log(Input.GetAxis("D-pad Y"));
        // ゲームパッドの左スティックのベクトルを取得する
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
         
        // 他のオブジェクトのスクリプトを読み込む（スクリプトはクラス扱い）
        CameraController cameraScript = camera.GetComponent<CameraController>();
 
        // カメラのY軸角度から回転行列を生成する
        Quaternion rot = Quaternion.Euler(0, cameraScript.cameraRotation.y * Mathf.Rad2Deg + 90, 0);
 
        // 逆行列を生成する
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        Matrix4x4 inv = m.inverse;
 
        // 回転行列をかけたベクトルに変換する
        direction = inv.MultiplyVector(direction);
 
        if (direction.magnitude > 0.001f)
        {
            // Slerpは球面線形補間、Vector3.Angleは二点間の角度（degree）を返す
            Vector3 forward = Vector3.Slerp(this.transform.forward, direction, rotationSpeed * Time.deltaTime / Vector3.Angle(this.transform.forward, direction));
 
            // ベクトル方向へ向かせる
            transform.LookAt(this.transform.position + forward);
        }
 
        // 座標移動させる
        transform.position += direction * moveSpeed * Time.deltaTime;
 
        // アニメーターコントローラへ値を渡す
        animator.SetFloat("Blend", direction.magnitude);

        // if (Input.GetKey("joystick button 0")) {
        //     animator.SetBool("is_jumping", true);
        // } else {
        //     animator.SetBool("is_jumping", false);
        // }

        if(Input.GetKey("space")){
            Debug.Log("Pushed Tab");

        }
    }
}