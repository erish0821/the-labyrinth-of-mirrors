using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("Player Management")]
    [SerializeField] PlayerSO playerSO;  // 전체 플레이어 리스트를 관리하는 PlayerSO
    [SerializeField] PlayerData currentPlayer;     // 현재 플레이어 데이터

    [Header("Card Management")]
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;
    [SerializeField] List<Card> otherCards1;
    [SerializeField] List<Card> otherCards2;
    [SerializeField] List<Card> otherCards3;

    [Header("Card Interaction")]
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform otherCardSpawnPoint;
    [SerializeField] Transform otherCardSpawnPoint1;
    [SerializeField] Transform otherCardSpawnPoint2;
    [SerializeField] Transform otherCardSpawnPoint3;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] ECardState eCardState;
    
    List<Item> itemBuffer;
    Card selectCard;  // 현재 선택된 카드
    bool isMyCardDrag;  // 카드 드래그 상태

    bool onMyCardArea;
    
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }
    int myPutCount; // 플레이어가 사용한 카드 수


    public Item PopItem()
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    void SetupItemBuffer()
    {
        itemBuffer = new List<Item>(77);
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            for (int j = 0; j < item.quantity; j++)
                itemBuffer.Add(item);
        }

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void Start()
    {
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
    }
    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void DistributeCards(bool isMine, int cardCount, int targetPlayer)
    {
        for (int i = 0; i < cardCount; i++)
        {
            AddCard(isMine, targetPlayer);
        }
    }

    void OnTurnStarted(bool myTurn) 
    {
        if (myTurn)
            myPutCount = 0;
    }

	void Update()
	{
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
        SetECardState();
    }

    void AddCard(bool isMine)
    {
        if (isMine)
        {
            // 내 카드 추가
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            var card = cardObject.GetComponent<Card>();
            card.Setup(PopItem(), true);
            myCards.Add(card);

            SetOriginOrder(myCards);
            CardAlignment(myCards);
        }
        else
        {
            // 기본적으로 상대 카드에 추가
            AddCard(false, 0);
        }
    }
    void AddCard(bool isMine, int targetPlayer)
    {
        if (isMine)
        {
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            var card = cardObject.GetComponent<Card>();
            card.Setup(PopItem(), true);
            myCards.Add(card);

            SetOriginOrder(myCards);
            CardAlignment(myCards);
        }
        else
        {
            List<Card> targetCardList = GetOtherCardList(targetPlayer);

            if (targetCardList != null)
            {
                var card = new GameObject($"OtherPlayer{targetPlayer}_Card");
                card.transform.position = GetOtherCardSpawnPoint(targetPlayer).position;

                targetCardList.Add(null); // 카드 리스트 관리
                UpdateOtherPlayerCardDisplay(targetPlayer, targetCardList.Count);
            }
            else
            {
                Debug.LogError($"Invalid targetPlayer {targetPlayer}");
            }
        }
    }

    void UpdateOtherPlayerCardDisplay(int targetPlayer, int cardCount)
    {
        // OtherPlayer의 카드 표시 위치
        Transform spawnPoint = GetOtherCardSpawnPoint(targetPlayer);

        // 카드 개수를 표시하는 텍스트 생성 또는 갱신
        var cardCountText = spawnPoint.GetComponentInChildren<TextMesh>();
        if (cardCountText == null)
        {
            var textObject = new GameObject($"OtherPlayer{targetPlayer}_CardCount");
            textObject.transform.SetParent(spawnPoint);
            textObject.transform.localPosition = new Vector3(0, -0.5f, 0); // 텍스트 위치 조정
            cardCountText = textObject.AddComponent<TextMesh>();
            cardCountText.fontSize = 20;
            cardCountText.color = Color.white;
        }

        cardCountText.text = $"x{cardCount}";
    }

    List<Card> GetOtherCardList(int targetPlayer)
    {
        switch (targetPlayer)
        {
            case 0: return otherCards;  // 기본 OtherPlayer
            case 1: return otherCards1; // OtherPlayer1
            case 2: return otherCards2; // OtherPlayer2
            case 3: return otherCards3; // OtherPlayer3
            default:
                Debug.LogError($"Invalid targetPlayer: {targetPlayer}");
                return null;
        }
    }

    Transform GetOtherCardSpawnPoint(int targetPlayer)
    {
        switch (targetPlayer)
        {
            case 0: return otherCardSpawnPoint;  // 기본 OtherPlayer
            case 1: return otherCardSpawnPoint1; // OtherPlayer1
            case 2: return otherCardSpawnPoint2; // OtherPlayer2
            case 3: return otherCardSpawnPoint3; // OtherPlayer3
            default:
                Debug.LogError($"Invalid targetPlayer: {targetPlayer}");
                return null;
        }
    }

    void SetOriginOrder(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i]?.GetComponent<Order>().SetOriginOrder(i);
        }
    }
    void CardAlignment(List<Card> cardList)
    {
        if (cardList == myCards) // MyPlayer만 정렬
        {
            List<PRS> originCardPRSs = RoundAlignment(myCardLeft, myCardRight, cardList.Count, -0.5f, Vector3.one * 1.9f);

            for (int i = 0; i < cardList.Count; i++)
            {
                var targetCard = cardList[i];
                targetCard.originPRS = originCardPRSs[i];
                targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
            }
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public bool TryPutCard(bool isMine, int targetPlayer)
    {
        List<Card> targetCards = isMine ? myCards : GetOtherCardList(targetPlayer);
        //if (isMine && myPutCount >= 1) //카드 1장만 쓰게 설정함. 이걸 수정할듯 
        //    return false;

        if (!isMine && (targetCards == null || targetCards.Count <= 0))
            return false;

        Card card = isMine ? selectCard : targetCards[Random.Range(0, targetCards.Count)];
        List<PlayerData> allPlayers = playerSO.players;

        if (card != null)
        {
            try
            {
                Debug.Log($"CardManager -> tryCard : 카드 시도 중");
                selectCard.UseCard(currentPlayer, allPlayers); // 카드 효과 발동
                Debug.Log($"Card {selectCard.item.name} effect executed!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error using card: {ex.Message}");
            }
            finally
            {
                myCards.Remove(selectCard);
                Destroy(selectCard.gameObject); // 카드 제거
                myPutCount++;
                CardAlignment(myCards);
            }
            return true;
        }

        return false;
    }

    #region MyCard
    public void CardMouseOver(Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;

        selectCard = card;
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown() 
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;

        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;

        if (eCardState != ECardState.CanMouseDrag)
            return;
   
        // 카드가 내 카드 영역 안에 있을 경우
        if (onMyCardArea)
        {
            Debug.Log("CardManager-CardMouseUP : 카드를 다시 손패로 되돌립니다.");
            // 손패로 카드 복귀 (아무 작업도 하지 않음)
        }
        else
        {
            // 카드를 사용하려는 시도
            bool cardUsed = TryPutCard(true, 0);
            if (cardUsed && selectCard != null)
                try
                {
                    Debug.Log($"CardManager -> CardMouseUP : Card {selectCard.item.name} effect executed!");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"카드 사용 중 오류 발생: {ex.Message}");
                }
                finally
                {
                    myCards.Remove(selectCard);
                    Destroy(selectCard.gameObject); // 카드 제거
                    selectCard = null;
                }
        }
    }

    void CardDrag()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;

        if (!onMyCardArea) //카드 사용 조건
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -4.8f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 3.5f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetECardState()
    {
        if (TurnManager.Inst.isLoading)
            eCardState = ECardState.Nothing;

        else if (!TurnManager.Inst.myTurn || myPutCount >= 1)
            eCardState = ECardState.CanMouseOver;

        else if (TurnManager.Inst.myTurn && myPutCount == 0)
            eCardState = ECardState.CanMouseDrag;
    }


    #endregion

}