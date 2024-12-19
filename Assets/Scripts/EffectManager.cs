using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
    }

    public void ExecuteSpecialEffect(Item item, PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        switch (item.name)
        {
            case "������ ����":
                ApplyAreaDamage(item.damage, item.targetType, currentPlayer, allPlayers);
                break;
            case "����":
                StartDuel(currentPlayer, allPlayers);
                break;
            case "����":
                HealAllPlayers(item.heal, allPlayers);
                break;
            case "��Ż":
                StealCard(currentPlayer, allPlayers);
                break;
            default:
                Debug.LogWarning($"�� �� ���� Ư�� ȿ��: {item.name}");
                break;
        }
    }

    private void ApplyAreaDamage(int damage, TargetType targetType, PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        Debug.Log($"���� ������: {damage} (Ÿ�� Ÿ��: {targetType})");

        // Ÿ�� Ÿ�Կ� ���� ó��
        switch (targetType)
        {
            case TargetType.All:
                foreach (var player in allPlayers)
                {
                    player.health -= damage;
                    Debug.Log($"{player.playerName}���� {damage}�� �������� �������ϴ�. ���� ü��: {player.health}");
                }
                break;

            case TargetType.AllExceptSelf:
                foreach (var player in allPlayers)
                {
                    if (player != currentPlayer)
                    {
                        player.health -= damage;
                        Debug.Log($"{player.playerName}���� {damage}�� �������� �������ϴ�. ���� ü��: {player.health}");
                    }
                }
                break;

            default:
                Debug.LogWarning("Ÿ�� Ÿ���� �ùٸ��� �ʽ��ϴ�.");
                break;
        }
    }

    private void StartDuel(PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        Debug.Log("���� ����!");
        // �� �÷��̾� �� ���� ����
        PlayerData target = SelectTarget(allPlayers);
        Debug.Log($"{currentPlayer.playerName}��(��) {target.playerName}��(��) ������ �����մϴ�!");
        // ���� ���� ���� �ʿ�
    }

    private void HealAllPlayers(int healAmount, List<PlayerData> allPlayers)
    {
        Debug.Log($"��� �÷��̾ ü���� {healAmount}��ŭ ȸ���մϴ�.");
        foreach (var player in allPlayers)
        {
            player.health += healAmount;
            Debug.Log($"{player.playerName}�� ü���� {healAmount}��ŭ ȸ���Ǿ����ϴ�. ���� ü��: {player.health}");
        }
    }

    private void StealCard(PlayerData currentPlayer, List<PlayerData> allPlayers)
    {
        PlayerData target = SelectTarget(allPlayers);
        Debug.Log($"{currentPlayer.playerName}��(��) {target.playerName}�� ī�带 ��Ż�մϴ�.");
        // Ÿ�� �÷��̾��� ī�� ��Ż ����
        if (target.weaponCard != null)
        {
            currentPlayer.weaponCard = target.weaponCard;
            target.weaponCard = null;
            Debug.Log($"{currentPlayer.playerName}�� {target.playerName}�� ���� ī�带 ��Ż�߽��ϴ�.");
        }
    }

    private PlayerData SelectTarget(List<PlayerData> allPlayers)
    {
        // ������ Ÿ�� ���� ���� (ù ��° �÷��̾� ����)
        foreach (var player in allPlayers)
        {
            if (player != null)
                return player;
        }
        return null;
    }
}
