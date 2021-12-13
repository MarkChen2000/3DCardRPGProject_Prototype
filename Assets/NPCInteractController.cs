using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractController : MonoBehaviour
{
    private InteractController InteractCon;
    private InteractUIController InteractUICon;

    private GameObject Target_Player;
    public float PlayerCan_InteractRadius = 5f;

    public InteractType _InteracrType;
    public ConversationTemplate _ConversationTem;
    public MissionTemplate _MissionTem;
    public TransactionTemplate _TransactionTem;


    private void Awake()
    {
        InteractCon = GameObject.Find("InteractAndUIManager").GetComponent<InteractController>();
        InteractUICon = InteractCon.GetComponent<InteractUIController>();
        if (Target_Player == null) Target_Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_InteracrType == InteractType.Null) return; // if NPC doesn't have interact type, then don't do anything.

        if (Vector3.Distance(Target_Player.transform.position, transform.position) <= PlayerCan_InteractRadius)
        {
            InteractUICon.ShowInteractInfo(_InteracrType);
            if ( Input.GetKeyDown(KeyCode.E) && !InteractCon.Is_Interacting )
            {
                switch ( _InteracrType )
                {
                    case InteractType.Conversation:
                        InteractCon.StartConversationInteract(_ConversationTem);
                        break;
                    case InteractType.Transaction:

                        break;
                    case InteractType.Mission:

                        break;
                }
            }
        }
        else
        {
            InteractUICon.HideInteractInfo();
        }
    }

    private void OnDrawGizmos()
    {
        if (_InteracrType == InteractType.Null) return; 

        Gizmos.color = new Color(255f, 0f, 0f);
        Gizmos.DrawWireSphere(transform.position, PlayerCan_InteractRadius);
    }

}
