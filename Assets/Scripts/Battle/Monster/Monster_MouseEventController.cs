using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_MouseEventController : MonoBehaviour
{
    //private bool onMouse;
    private bool onLock;
    private GameObject Lock;

    // Start is called before the first frame update
    void Start()
    {
        //onMouse = false;
        onLock = false;
        Lock = this.transform.GetChild(1).gameObject;
        Lock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (onLock == false)
            {
                onLock = true;
                Lock.SetActive(true);
            }
            else
            {
                onLock = false;
                Lock.SetActive(false);
            }
        }

        if (onLock == true)
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().currentTarget(this.gameObject);
            //Debug.Log(onLock);
        }
        else
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().currentTarget(null);
            //Debug.Log(onLock);
        }
    }

    private void OnMouseEnter()
    {
        //GameObject.Find("BattleManager").GetComponent<BattleManager>().currentTarget(this.gameObject);
    }

    private void OnMouseExit()
    {
        //onMouse = false;
        //GameObject.Find("BattleManager").GetComponent<BattleManager>().currentTarget(null);
    }

    private void OnMouseOver()
    {
        //onMouse = true;
    }

    public bool isOnMouse()
    {
        //return onMouse;
        return true;
    }
}
