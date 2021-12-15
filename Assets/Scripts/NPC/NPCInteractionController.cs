using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionController : MonoBehaviour
{
    private InteractionController InteractionCon;
    private InteractionUIController InteractionUICon;

    private GameObject Target_Player;
    public float PlayerCan_InteractionRadius = 5f;

    public InteractionType _InteractionType;
    public ConversationTemplate _ConversationTem;
    public TransactionTemplate _TransactionTem;
    public MissionTemplate _MissionTem;

    private bool Can_Interact = false;

    private void Awake()
    {
        InteractionCon = GameObject.Find("InteractAndUIManager").GetComponent<InteractionController>();
        InteractionUICon = InteractionCon.GetComponent<InteractionUIController>();
        if (Target_Player == null) Target_Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_InteractionType == InteractionType.Null) return; // if NPC doesn't have interact type, then don't do anything.

        if ( Can_Interact )
        {
            if ( Input.GetKeyDown(KeyCode.E) && !InteractionCon.Is_Interacting ) 
                //When there is not interacting, then the interact botton (E) work as active botton.
            {
                switch (_InteractionType)
                {
                    case InteractionType.Conversation:
                        InteractionCon.StartConversationInteraction(_ConversationTem);
                        break;
                    case InteractionType.Transaction:
                        InteractionCon.StartTransactionInteraction(_TransactionTem);
                        break;
                    case InteractionType.Mission:

                        break;
                }
            }
        }

        Can_Interact = false; // till next frame, if the OnInteractZone didn't get called, then Can_Interaction will return to false;
    }

    public void OnInteractZone()
    {
        if (_InteractionType == InteractionType.Null) return;
        Can_Interact = true;
    }

}
