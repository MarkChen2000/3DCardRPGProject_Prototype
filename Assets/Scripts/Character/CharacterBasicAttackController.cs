using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBasicAttackController : MonoBehaviour
{
    public Camera MainCamera;
    private Transform Trans_Camera;

    public float Player_AttackCD = 1f;
    private float AttackCDTimer = 0;

    public float TurningSmoothTime = 0.1f; // the time of character turning to target angle.
    float tunringsmooth_velocity;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void Start()
    {
        if (MainCamera == null) MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        Trans_Camera = MainCamera.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown(0)) // left mouse click
        {
            BasicAttack();
            
        }
    }

    private void BasicAttack()
    {
        if ( Time.time < AttackCDTimer )
        {
            Debug.Log("Attack is cooling!");
            return;
        }

        Ray detectray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(detectray, out hit))
        {
            Vector3 MouseClickPos = hit.point;

            // Rotate character to the attack point.
            Vector3 face_dir = (MouseClickPos - transform.position).normalized;
            float targetangle = Mathf.Atan2(face_dir.x, face_dir.z) * Mathf.Rad2Deg + Trans_Camera.eulerAngles.y;
            float smoothed_targetangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref tunringsmooth_velocity, TurningSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothed_targetangle, 0f);



            Debug.Log("Attack! Target:" + MouseClickPos);
        }
        else Debug.Log("Attack fail! didn't find a target!");

        AttackCDTimer = Time.time + Player_AttackCD;
    }

}
