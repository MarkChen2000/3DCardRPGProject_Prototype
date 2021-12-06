using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBasicAttackController : MonoBehaviour
{
    public Camera MainCamera;
    private Transform Trans_Camera;
    private CharacterMovementController Character_MoveCon;

    public float Character_AttackCD = 1f;
    private float AttackCDTimer = 0;

    [Tooltip("The time that make character turn in inidle state, which will make character stop turning toward the moveing direction.")]
    public float Character_AttackInidleTime = 2f;

    public float AttackTurningSmoothTime = 0.001f; // the time of character turning to target angle.
    float attacktunringsmooth_velocity;

    void Awake()
    {
        Character_MoveCon = GetComponent<CharacterMovementController>();
    }

    private void Start()
    {
        if (MainCamera == null) MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        Trans_Camera = MainCamera.GetComponent<Transform>();
    }

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
            float smoothed_targetangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref attacktunringsmooth_velocity, AttackTurningSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothed_targetangle, 0f);

            Character_MoveCon.RefreshStaringDurationTimer(Character_AttackInidleTime);
            Debug.Log("Attack! Target:" + MouseClickPos);
        }
        else Debug.Log("Attack fail! didn't find a target!");

        AttackCDTimer = Time.time + Character_AttackCD;
    }

}
