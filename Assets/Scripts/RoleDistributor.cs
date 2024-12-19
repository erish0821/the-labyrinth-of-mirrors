using System.Collections.Generic;
using UnityEngine;

public class RoleDistributor : MonoBehaviour
{
    public List<RoleCardSO> roleCards;       // ���� ī�� ����Ʈ (ScriptableObject�� ����)
    public List<CharacterSO> characters;    // ĳ���� ����Ʈ (ScriptableObject�� ����)
    public PlayerSO playerSO;               // �÷��̾� ������

    [Header("Player UI Parent")]
    public Transform playerUIParent; // Player UI�� ���Ե� �θ� Canvas ������Ʈ
    private List<PlayerUI> playerUIPanels = new List<PlayerUI>();   // UI �г� ����Ʈ (Inspector���� ����)

    void Start()
    {
        InitializePlayerUIPanels(); // �ڵ����� UI �г� �ʱ�ȭ
        AssignRolesAndCharacters(); // ���Ұ� ĳ���͸� ����
        DisplayAssignedData();      // ������ �����͸� ���
        UpdatePlayerUIs();          // UI ������Ʈ
    }

    private void InitializePlayerUIPanels()
    {
        playerUIPanels.Clear();

        // Player UI �θ� ������Ʈ���� �ڽĵ��� �˻��Ͽ� PlayerUI ������Ʈ �߰�
        foreach (Transform child in playerUIParent)
        {
            var playerUI = child.GetComponent<PlayerUI>();
            if (playerUI != null)
            {
                playerUIPanels.Add(playerUI);
            }
        }

        Debug.Log($"Initialized {playerUIPanels.Count} PlayerUI panels.");
    }

    public void AssignRolesAndCharacters()
    {
        List<RoleCardSO> availableRoles = new List<RoleCardSO>(roleCards);
        List<CharacterSO> availableCharacters = new List<CharacterSO>(characters);

        ShuffleList(availableRoles);
        ShuffleList(availableCharacters);

        for (int i = 0; i < playerSO.players.Count; i++)
        {
            var player = playerSO.players[i];

            player.roleCard = availableRoles[i];
            player.characterData = availableCharacters[i];

            player.health = player.characterData.baseHealth;
            player.attack = player.characterData.baseAttack;
            player.defense = player.characterData.baseDefense;

            if (player.roleCard.roleType == Role.TeamLeader)
            {
                player.isRoleRevealed = true; // ������ ���Ҹ� ����
            }
            else
            {
                player.isRoleRevealed = false; // ������ ������ �����
            }

            player.isStealthed = false;
            player.isFirstAttack = true;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void UpdatePlayerUIs()
    {
        for (int i = 0; i < playerSO.players.Count; i++)
        {
            bool isMyUI = (i == 0); // ù ��° �÷��̾ �� �ڽ�
            playerUIPanels[i].Setup(playerSO.players[i], isMyUI);
        }
    }

    public void DisplayAssignedData()
    {
        foreach (var player in playerSO.players)
        {
            Debug.Log($"Player: {player.playerName}, Role: {player.roleCard.roleName}, Revealed: {player.isRoleRevealed}");
        }
    }
}
