using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private InteractionUIController InteractionUICon;

    private Transform Trans_InteractDetectPos;
    public float PlayerCan_InteractionRadius = 4f;

    // Start is called before the first frame update
    void Awake()
    {
        InteractionUICon = GameObject.Find("InteractAndUIManager").GetComponent<InteractionUIController>();
        Trans_InteractDetectPos = transform.GetChild(2);
    }

    private void Update()
    {
        CheckCanInteract();
    }

    private void CheckCanInteract() // this part may cost a lot of performance, but didn't thank some method is more better.
    {
        Collider[] colliders = Physics.OverlapSphere(Trans_InteractDetectPos.position, PlayerCan_InteractionRadius);
        bool is_npc_near = false;
        foreach (Collider item in colliders)
        {
            //Debug.Log("Collide" + item.name);
            NPCInteractionController npc_interactioncon = item.gameObject.GetComponent<NPCInteractionController>();
            if ( npc_interactioncon!=null )
            {
                InteractionUICon.ShowInteractionInfo(npc_interactioncon._InteractionType);
                npc_interactioncon.OnInteractZone();
                is_npc_near = true;
            }
        }
        if (is_npc_near == false) InteractionUICon.HideInteractionInfo();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0f, 0f);
        Gizmos.DrawWireSphere(transform.position, PlayerCan_InteractionRadius);
    }
}
