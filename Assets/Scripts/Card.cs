using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class Card : MonoBehaviour
{
    [Header("Visual Elements")]
    [SerializeField] SpriteRenderer cardSpriteRenderer; // 카드 배경
    [SerializeField] SpriteRenderer characterSpriteRenderer; // 카드 배경
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text effectTMP;

    [Header("Card Sprites")]
    [SerializeField] Sprite cardFrontSprite;
    [SerializeField] Sprite cardBackSprite;

    [Header("Card Data")]
    public PRS originPRS; // 카드의 원래 위치와 회전, 크기 정보
    public Item item;     // 카드의 데이터
    private bool isFront; // 카드가 앞면인지 여부

    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            cardSpriteRenderer.sprite = cardFrontSprite;
            characterSpriteRenderer.sprite = this.item.sprite; // 캐릭터 스프라이트 설정
            nameTMP.text = this.item.name;
            effectTMP.text = this.item.effect;
        }
        else
        {
            // 카드 뒷면 데이터 비표시
            cardSpriteRenderer.sprite = cardBackSprite;
            nameTMP.text = "";
            effectTMP.text = "";
        }
    }

    public void UseCard(PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        if (currentPlayer == null)
        {
            Debug.LogError("currentPlayer is null!");
            return;
        }

        Debug.Log($"Current Player Info: ID = {currentPlayer.id}");
        switch (item.type)
        {
            case "공격 카드":
                ApplyDamage(currentPlayer);
                break;

            case "회복 카드":
                HealPlayer(currentPlayer);
                break;

            case "방어 카드":
                ActivateDefense(currentPlayer);
                break;

            case "특수 카드":
                ApplySpecialEffect(currentPlayer);
                break;

            case "장착 카드":
                EquipItem(currentPlayer, allPlayers);
                break;

            default:
                Debug.LogWarning($"알 수 없는 카드 유형: {item.type}");
                break;
        }

        // 카드 사용 후 삭제
        Destroy(gameObject);
    }

    private void ApplyDamage(PlayerData currentPlayer)
    {
        Debug.Log($"카드 {item?.name} 사용: {item?.effect}");

        // currentPlayer null 체크
        if (currentPlayer == null)
        {
            Debug.LogError("currentPlayer가 null입니다! ApplyDamage 호출 전에 올바른 값을 전달해야 합니다.");
            return;
        }

        // PlayerManager 인스턴스 null 체크
        if (PlayerManager.Inst == null)
        {
            Debug.LogError("PlayerManager 인스턴스가 null입니다. PlayerManager가 씬에 추가되었는지 확인하세요.");
            return;
        }

        // PlayerSO에서 데이터 가져오기
        List<PlayerData> allPlayers = PlayerManager.Inst.GetAllPlayers();
        if (allPlayers == null || allPlayers.Count == 0)
        {
            Debug.LogError("PlayerSO에서 플레이어 데이터를 가져오지 못했습니다. PlayerSO를 확인하세요.");
            return;
        }

        // item null 체크
        if (item == null)
        {
            Debug.LogError("item 객체가 null입니다! Card에 올바른 아이템 데이터를 설정해야 합니다.");
            return;
        }

        // TargetType 검증
        if (!System.Enum.IsDefined(typeof(TargetType), item.targetType))
        {
            Debug.LogError("TargetType이 올바르게 설정되지 않았습니다.");
            return;
        }
        // TargetType에 따른 처리
        switch (item.targetType)
        {
            case TargetType.Self:
                Debug.Log("데미지가 자신에게 적용됩니다.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.Self, currentPlayer);
                break;

            case TargetType.Single:
                Debug.Log("데미지가 특정 대상에게 적용됩니다.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.Single, currentPlayer);
                break;

            case TargetType.All:
                Debug.Log("데미지가 모든 플레이어에게 적용됩니다.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.All, currentPlayer);
                break;

            case TargetType.AllExceptSelf:
                Debug.Log("데미지가 자신을 제외한 모든 플레이어에게 적용됩니다.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.AllExceptSelf, currentPlayer);
                break;

            default:
                Debug.LogWarning($"알 수 없는 TargetType: {item.targetType}");
                break;
        }
    }

    private void HealPlayer(PlayerData currentPlayer)
    {
        Debug.Log($"HealPlayer카드 {item.name} 사용: {item.effect}");
        PlayerManager.Inst.HealPlayer(currentPlayer, TargetType .Self, item.heal);
    }

    private void ActivateDefense(PlayerData currentPlayer)
    {
        Debug.Log($"카드 {item.name} 사용: {item.effect}");
        PlayerManager.Inst.ActivateDefense(currentPlayer);
    }

    private void ApplySpecialEffect(PlayerData currentPlayer)
    {
        Debug.Log($"Card -> ApplySpecialEffect : {item.name}");
        switch (item.name)
        {
            case "무차별 난사":
                ApplyDamage(currentPlayer);
                break;

            case "주점":
                HealPlayer(currentPlayer);
                break;

            case "Shield":

                break;

            case "DoubleAttack":

                break;

            default:
                Debug.LogWarning("알 수 없는 카드 입니다");
                break;
        }
    }

    private void EquipItem(PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        Debug.Log($"장착 카드 {item.name} 사용: {item.effect}");

        switch (item.name)
        {
            case "감옥":
                PlayerData targetPlayer = TargetingManager.Inst.SelectTarget(allPlayers, TargetType.Single, currentPlayer); // 수정된 부분
                if (targetPlayer != null)
                {
                    Debug.Log($"{currentPlayer.playerName}가 {targetPlayer.playerName}에게 감옥을 장착합니다.");
                    // ApplyJailEffect(targetPlayer);
                }
                else
                {
                    Debug.LogWarning("타겟 플레이어를 찾을 수 없습니다.");
                }
                break;

            case "다이너마이트":
                Debug.Log("다이너마이트 장착: 모든 플레이어에게 영향을 줍니다.");
                // PlayerManager.Inst.ApplyEquipment(item);
                break;

            case "술통":
                Debug.Log("술통 장착: 회피 확률 증가.");
                currentPlayer.isStealthed = true;
                break;

            default:
                Debug.LogWarning($"알 수 없는 장착 카드: {item.name}");
                break;
        }
    }

    void OnMouseOver()
    {
        if (isFront)
            CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        if (isFront)
            CardManager.Inst.CardMouseExit(this);
    }

    void OnMouseDown()
    {
        if (isFront)
            CardManager.Inst.CardMouseDown();
    }

    void OnMouseUp()
    {
        if (isFront)
            CardManager.Inst.CardMouseUp();
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

}
