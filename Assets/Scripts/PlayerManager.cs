using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.Progress;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Inst { get; private set; }
    [SerializeField] private PlayerSO playerSO; // PlayerSO�� Inspector���� ����

    private void Start()
    {
        InitializePlayers();
    }

    private void InitializePlayers()
    {
        for (int i = 0; i < playerSO.players.Count; i++)
        {
            playerSO.players[i].id = i; // ID �ο�
        }
    }

    private void Awake()
    {
        Inst = this;

        if (playerSO == null || playerSO.players == null || playerSO.players.Count == 0)
        {
            Debug.LogError("PlayerSO�� �������� �ʾҰų� �÷��̾� �����Ͱ� �����ϴ�!");
            return;
        }
    }

    // ��� �÷��̾� ��������
    public List<PlayerData> GetAllPlayers()
    {
        return playerSO.players;
    }

    // ������ ����
    public void ApplyDamageToTarget(int damage, TargetType targetType, PlayerData currentPlayer)
    {
        List<PlayerData> allPlayers = GetAllPlayers();

        Debug.Log($"��󿡰� {damage} �������� ���� (Ÿ�� Ÿ��: {targetType})");
        int repeatCount = 0;
        switch (targetType)
        {
            case TargetType.Self:
                ApplyHealthChange(currentPlayer, -damage);
                break;

            case TargetType.Single:
                PlayerData singleTarget = TargetingManager.Inst.SelectTarget(allPlayers, targetType, currentPlayer);
                Debug.Log($"[ApplyHealthChange] PlayerManager�����޵� PlayerData ����: " +
                $"ID: {singleTarget.id}, Name: {singleTarget.playerName}, " +
                $"Health: {singleTarget.health}, BaseHealth: {singleTarget.characterData.baseHealth}");
                if (singleTarget != null)
                {
                    ApplyHealthChange(singleTarget, -damage);
                }
                else
                {
                    Debug.LogWarning("Ÿ���� ���õ��� �ʾҽ��ϴ�!");
                }
                break;

            case TargetType.All:
                foreach (var player in allPlayers)
                {
                    ApplyHealthChange(player, -damage);
                }
                break;
           
            case TargetType.AllExceptSelf:
                foreach (var player in allPlayers)
                {
                    if (repeatCount >= 3) break;

                    PlayerData AllExceptSelfTarget = TargetingManager.Inst.SelectTarget(allPlayers, targetType, currentPlayer);
                    Debug.Log($"[ApplyHealthChange] PlayerManager�����޵� PlayerData ����: " +
                    $"ID: {AllExceptSelfTarget.id}, Name: {AllExceptSelfTarget.playerName}, " +
                    $"Health: {AllExceptSelfTarget.health}, BaseHealth: {AllExceptSelfTarget.characterData.baseHealth}");
                    if (AllExceptSelfTarget != null)
                    {
                        ApplyHealthChange(AllExceptSelfTarget, -damage);
                    }
                    else
                    {
                        Debug.LogWarning("Ÿ���� ���õ��� �ʾҽ��ϴ�!");
                    }

                    repeatCount++;
                }
                break;

            default:
                Debug.LogWarning("�ùٸ��� ���� Ÿ�� Ÿ���Դϴ�.");
                break;
        }
    }

    // ü�� ���� (������/ȸ��)
    private void ApplyHealthChange(PlayerData currentPlayer, int amount)
    {
        currentPlayer.health += amount;
        Debug.Log($"{currentPlayer.playerName}�� ü���� {amount}��ŭ ����Ǿ����ϴ�. ���� ü��: {currentPlayer.health}");
        UpdatePlayerHealth(currentPlayer);
    }

    // UI ������Ʈ
    public void UpdatePlayerHealth(PlayerData player)
    {
        Debug.Log($"[ApplyHealthChange] ���޵� PlayerData ����: " +
                  $"ID: {player.id}, Name: {player.playerName}, " +
                  $"Health: {player.health}, BaseHealth: {player.characterData.baseHealth}");

        // FindObjectsOfType�� PlayerUI�� ã�� �̸����� ����
        PlayerUI[] playerUIs = FindObjectsOfType<PlayerUI>();
        playerUIs = playerUIs.OrderBy(ui => ui.gameObject.name).ToArray();

        Debug.Log($"[PlayerUI �迭]: �� {playerUIs.Length}�� �߰�");
        for (int i = 0; i < playerUIs.Length; i++)
        {
            Debug.Log($"[{i}] PlayerUI - GameObject Name: {playerUIs[i].gameObject.name}");
        }

        // ID�� �������� PlayerUI�� ����
        int playerIndex = playerSO.players.IndexOf(player);
        if (playerIndex >= 0 && playerIndex < playerSO.players.Count)
        {
            PlayerUI playerUI = playerUIs[playerIndex];
            playerUI.UpdateHealth(player.health);
        }
        else
        {
            Debug.LogWarning($"UI�� ������Ʈ�� �� �����ϴ�. {player.playerName}�� UI�� ã�� �� �����ϴ�.");
        }
    }

    // �÷��̾� ü�� ȸ��
    public void HealPlayer(PlayerData currentPlayer, TargetType targetType, int heal)
    {
        List<PlayerData> allPlayers = GetAllPlayers();
        PlayerData selfTarget = TargetingManager.Inst.SelectTarget(allPlayers, targetType, currentPlayer);
        switch (targetType)
        {
            case TargetType.Self:
                ApplyHealthChange(selfTarget, heal);
                break;
            default:
                Debug.LogWarning("�ùٸ��� ���� Ÿ�� Ÿ���Դϴ�.");
                break;
        }
    }

    // ��� Ȱ��ȭ
    public void ActivateDefense(PlayerData player)
    {
        Debug.Log($"{player.playerName} ��� Ȱ��ȭ");
        player.isStealthed = true; // ��� �Ǵ� ���� ���� ����
    }

    // ������ ����
    public void EquipItem(Item item, PlayerData player)
    {
        Debug.Log($"�÷��̾� {player.playerName}��(��) ������ {item.name} ����");

        switch (item.name)
        {
            case "����":
                player.isStealthed = true; // ȸ�� ���� Ȱ��ȭ
                Debug.Log($"{player.playerName}��(��) ������ �����߽��ϴ�. ȸ�� ���� Ȱ��ȭ!");
                break;

            case "����":
                Debug.Log($"{player.playerName}��(��) ������ �������ϴ�. ���� �� ���� �ൿ �Ұ�.");
                // ���� ȿ�� ó��
                break;

            case "���̳ʸ���Ʈ":
                
                break;

            default:
                Debug.LogWarning($"�� �� ���� ���� ī��: {item.name}");
                break;
        }
    }


}
