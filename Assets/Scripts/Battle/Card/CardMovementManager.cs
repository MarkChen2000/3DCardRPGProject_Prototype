using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovementManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    PlayerStatusController PlayerStatusCon;
    HandCardBattleController HandCardBattleCon;
    BattleUIManager _BattleUIManager;
    SoundEffectManager SoundEffectCon;

    CardData _card;

    Vector3 initPosition;
    public Camera MainCamera;
    RectTransform RectTrans_Card;
    CanvasGroup _CanvasGroup;

    bool Is_Dragging;
    PointerEventData EventData;

    //Vector2 lastMousePosition;
    //Transform Trans_Camera;
    //Vector3 mousePosition;
    //Ray detectray;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStatusCon = FindObjectOfType<PlayerStatusController>();
        HandCardBattleCon = FindObjectOfType<HandCardBattleController>();
        _BattleUIManager = FindObjectOfType<BattleUIManager>();
        SoundEffectCon = FindObjectOfType<SoundEffectManager>();

        _card = GetComponent<BattleCard_LoaderAndDisplay>()._CardData;
        RectTrans_Card = GetComponent<RectTransform>();
        _CanvasGroup = GetComponent<CanvasGroup>();

        if (MainCamera == null) MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        //Trans_Camera = MainCamera.GetComponent<Transform>();
    }

    // Update is called once per frame
    /*void Update()
    {
        //this.mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(this.mousePosition);
        //this.detectray = MainCamera.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(Input.mousePosition);
    }*/


    // because the onpointenter and ondrag will be called together when player drag the card,
    // the size of localScale will be conflicted, so I decide to cancel the feature.
    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTrans_Card.localPosition += new Vector3(0f, 50f, 0f);
        RectTrans_Card.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RectTrans_Card.localPosition -= new Vector3(0f, 50f, 0f);
        RectTrans_Card.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBegin");
        Is_Dragging = true;
        EventData = eventData;

        initPosition = RectTrans_Card.position;
        _CanvasGroup.alpha = 0.5f;

        RaycastHit hit;
        
        if (PlayerStatusCon.currentMana < _card.CardCost)
        {
            EventData.pointerDrag = null;
            OnDrag(EventData);
            _CanvasGroup.alpha = 1f;
            RectTrans_Card.position = initPosition;

            Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit);
            _BattleUIManager.SpawnHandCardPopupInfo("No enough mana!", hit.point);
            Is_Dragging = false;
            return; // fail to excute card
        }

        // lastMousePosition = eventData.position;
        // RectTrans_Card.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData.CardName.ToString();
    }

    public void OnDrag(PointerEventData eventData) // Called whenever this object is dragging.
                                                   // ( but when mouse stop moving, it will not be called. )
    {
        /*Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTrans_Card.position += new Vector3(diff.x, diff.y, transform.position.z);
        lastMousePosition = currentMousePosition;*/

        if (PlayerStatusCon.currentMana < _card.CardCost)
            return; // fail to excute card

        RectTrans_Card.anchoredPosition += eventData.delta;
    }

    void Update()
    {   
        if ( Is_Dragging ) 
        {
            if (Input.GetKey(KeyCode.Mouse1)) // cancel excute card.
            {
                EventData.pointerDrag = null;
                OnDrag(EventData);
                _CanvasGroup.alpha = 1f;
                RectTrans_Card.position = initPosition;
                Is_Dragging = false;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) // Called whenever finish the drag ( mouse button up ).
    {
        if (PlayerStatusCon.currentMana < _card.CardCost)
        {
            Debug.Log("Has no enough mana!!");
            return; // fail to excute card
        }

        //Vector2 currentMousePosition = eventData.position;
        RaycastHit hit;

        if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //Debug.Log(this.detectray);
            //Debug.Log(this.hit);
            //Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            //GameObject.Find("BattleManager").GetComponent<BattleManager>().executeCard(this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData, this.hit.point);
            
            if ( _card.Ability.Using_RangeDetect )
            {
                if ( !_card.Ability.CheckInActivateRange(hit.point)) // this function will send back whether can or can not activate the card.
                                                                     // If it is false, cancel the card ctivatation.
                {
                    _CanvasGroup.alpha = 1f;
                    RectTrans_Card.position = initPosition;
                    Is_Dragging = false;
                    return;
                }
            }

            HandCardBattleCon.updateStatus(_card);

            _card.ActivateCardAbility(hit.point);
            SoundEffectCon.SoundPlaySpellAttack();

            HandCardBattleCon.DrawCard();
            Destroy(this.gameObject);
        }
    }

    public void OnDrop(PointerEventData eventData) // Called whenever something enddrag (drop) on this item.
    {
    }

}
