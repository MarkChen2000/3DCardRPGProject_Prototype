using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovementManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    BattleManager _BattleManager;
    PlayerStatusController PlayerStatusCon;
    CardData _card;

    Vector3 initPosition;
    Vector2 lastMousePosition;
    public Camera MainCamera;
    Transform Trans_Camera;
    Vector3 mousePosition;
    Ray detectray;
    RaycastHit hit;
    RectTransform RectTrans_Card;

    // Start is called before the first frame update
    void Start()
    {
        _BattleManager = FindObjectOfType<BattleManager>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        _card = GetComponent<BattleCard_LoaderAndDisplay>()._CardData;
        RectTrans_Card = GetComponent<RectTransform>();
        initPosition = RectTrans_Card.position;
        RectTrans_Card.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        if (MainCamera == null)
        {
            MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        }
        Trans_Camera = MainCamera.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //this.mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(this.mousePosition);
        this.detectray = MainCamera.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(Input.mousePosition);
    }

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
        if (PlayerStatusCon.currentMana < _card.CardCost)
        {
            Debug.Log("Has no enough mana!!");
            return; // fail to excute card
        }

        lastMousePosition = eventData.position;
        RectTrans_Card.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _BattleManager.currentCard(_card);
        this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData.CardName.ToString();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTrans_Card.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayerStatusCon.currentMana < _card.CardCost)
        { return; }

        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;

        Vector3 newPosition = RectTrans_Card.position + new Vector3(diff.x, diff.y, transform.position.z);
        RectTrans_Card.position = newPosition;
        lastMousePosition = currentMousePosition;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            RectTrans_Card.position = new Vector3(initPosition.x, initPosition.y, initPosition.z);
            _BattleManager.currentCard(null);
            this.OnEndDrag(eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //this.detectray = MainCamera.ScreenPointToRay(Input.mousePosition);
        Vector2 currentMousePosition = eventData.position;

        if (Physics.Raycast(this.detectray, out this.hit))
        {
            this.mousePosition = this.hit.point;
            //Debug.Log(this.detectray);
            //Debug.Log(this.hit);
            //Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _BattleManager.executeCard(_card, mousePosition);
            //GameObject.Find("BattleManager").GetComponent<BattleManager>().executeCard(this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData, this.hit.point);
            _BattleManager.currentCard(null);
            Destroy(this.gameObject);
            _BattleManager.DrawCard();
        }
    }
}
