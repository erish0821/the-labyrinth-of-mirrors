using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class Card : MonoBehaviour
{
    [Header("Visual Elements")]
    [SerializeField] SpriteRenderer cardSpriteRenderer; // ī�� ���
    [SerializeField] SpriteRenderer characterSpriteRenderer; // ī�� ���
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text effectTMP;

    [Header("Card Sprites")]
    [SerializeField] Sprite cardFrontSprite;
    [SerializeField] Sprite cardBackSprite;

    [Header("Card Data")]
    public PRS originPRS; // ī���� ���� ��ġ�� ȸ��, ũ�� ����
    public Item item;     // ī���� ������
    private bool isFront; // ī�尡 �ո����� ����

    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            cardSpriteRenderer.sprite = cardFrontSprite;
            characterSpriteRenderer.sprite = this.item.sprite; // ĳ���� ��������Ʈ ����
            nameTMP.text = this.item.name;
            effectTMP.text = this.item.effect;
        }
        else
        {
            // ī�� �޸� ������ ��ǥ��
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
            case "���� ī��":
                ApplyDamage(currentPlayer);
                break;

            case "ȸ�� ī��":
                HealPlayer(currentPlayer);
                break;

            case "��� ī��":
                ActivateDefense(currentPlayer);
                break;

            case "Ư�� ī��":
                ApplySpecialEffect(currentPlayer);
                break;

            case "���� ī��":
                EquipItem(currentPlayer, allPlayers);
                break;

            default:
                Debug.LogWarning($"�� �� ���� ī�� ����: {item.type}");
                break;
        }

        // ī�� ��� �� ����
        Destroy(gameObject);
    }

    private void ApplyDamage(PlayerData currentPlayer)
    {
        Debug.Log($"ī�� {item?.name} ���: {item?.effect}");

        // currentPlayer null üũ
        if (currentPlayer == null)
        {
            Debug.LogError("currentPlayer�� null�Դϴ�! ApplyDamage ȣ�� ���� �ùٸ� ���� �����ؾ� �մϴ�.");
            return;
        }

        // PlayerManager �ν��Ͻ� null üũ
        if (PlayerManager.Inst == null)
        {
            Debug.LogError("PlayerManager �ν��Ͻ��� null�Դϴ�. PlayerManager�� ���� �߰��Ǿ����� Ȯ���ϼ���.");
            return;
        }

        // PlayerSO���� ������ ��������
        List<PlayerData> allPlayers = PlayerManager.Inst.GetAllPlayers();
        if (allPlayers == null || allPlayers.Count == 0)
        {
            Debug.LogError("PlayerSO���� �÷��̾� �����͸� �������� ���߽��ϴ�. PlayerSO�� Ȯ���ϼ���.");
            return;
        }

        // item null üũ
        if (item == null)
        {
            Debug.LogError("item ��ü�� null�Դϴ�! Card�� �ùٸ� ������ �����͸� �����ؾ� �մϴ�.");
            return;
        }

        // TargetType ����
        if (!System.Enum.IsDefined(typeof(TargetType), item.targetType))
        {
            Debug.LogError("TargetType�� �ùٸ��� �������� �ʾҽ��ϴ�.");
            return;
        }
        // TargetType�� ���� ó��
        switch (item.targetType)
        {
            case TargetType.Self:
                Debug.Log("�������� �ڽſ��� ����˴ϴ�.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.Self, currentPlayer);
                break;

            case TargetType.Single:
                Debug.Log("�������� Ư�� ��󿡰� ����˴ϴ�.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.Single, currentPlayer);
                break;

            case TargetType.All:
                Debug.Log("�������� ��� �÷��̾�� ����˴ϴ�.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.All, currentPlayer);
                break;

            case TargetType.AllExceptSelf:
                Debug.Log("�������� �ڽ��� ������ ��� �÷��̾�� ����˴ϴ�.");
                PlayerManager.Inst.ApplyDamageToTarget(item.damage, TargetType.AllExceptSelf, currentPlayer);
                break;

            default:
                Debug.LogWarning($"�� �� ���� TargetType: {item.targetType}");
                break;
        }
    }

    private void HealPlayer(PlayerData currentPlayer)
    {
        Debug.Log($"HealPlayerī�� {item.name} ���: {item.effect}");
        PlayerManager.Inst.HealPlayer(currentPlayer, TargetType .Self, item.heal);
    }

    private void ActivateDefense(PlayerData currentPlayer)
    {
        Debug.Log($"ī�� {item.name} ���: {item.effect}");
        PlayerManager.Inst.ActivateDefense(currentPlayer);
    }

    private void ApplySpecialEffect(PlayerData currentPlayer)
    {
        Debug.Log($"Card -> ApplySpecialEffect : {item.name}");
        switch (item.name)
        {
            case "������ ����":
                ApplyDamage(currentPlayer);
                break;

            case "����":
                HealPlayer(currentPlayer);
                break;

            case "Shield":

                break;

            case "DoubleAttack":

                break;

            default:
                Debug.LogWarning("�� �� ���� ī�� �Դϴ�");
                break;
        }
    }

    private void EquipItem(PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        Debug.Log($"���� ī�� {item.name} ���: {item.effect}");

        switch (item.name)
        {
            case "����":
                PlayerData targetPlayer = TargetingManager.Inst.SelectTarget(allPlayers, TargetType.Single, currentPlayer); // ������ �κ�
                if (targetPlayer != null)
                {
                    Debug.Log($"{currentPlayer.playerName}�� {targetPlayer.playerName}���� ������ �����մϴ�.");
                    // ApplyJailEffect(targetPlayer);
                }
                else
                {
                    Debug.LogWarning("Ÿ�� �÷��̾ ã�� �� �����ϴ�.");
                }
                break;

            case "���̳ʸ���Ʈ":
                Debug.Log("���̳ʸ���Ʈ ����: ��� �÷��̾�� ������ �ݴϴ�.");
                // PlayerManager.Inst.ApplyEquipment(item);
                break;

            case "����":
                Debug.Log("���� ����: ȸ�� Ȯ�� ����.");
                currentPlayer.isStealthed = true;
                break;

            default:
                Debug.LogWarning($"�� �� ���� ���� ī��: {item.name}");
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
