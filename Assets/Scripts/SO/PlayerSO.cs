using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Game/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public List<PlayerData> players; // �÷��̾� ������ ����Ʈ
}

[System.Serializable]
public class PlayerData
{
    public int id; // ���� ID
    public string playerName;           // �÷��̾� �̸�
    public RoleCardSO roleCard;         // ���� ī��
    public CharacterSO characterData;  // ĳ���� ������
    public bool isRoleRevealed;         // ���� ���� ����

    public Item weaponCard;             // ���� ī�� (ItemSO�� ȣȯ)
    public Item potionCard;             // ���� ī�� (ItemSO�� ȣȯ)
    public Item debufferCard;           // ����� ī�� (ItemSO�� ȣȯ)

    [Range(1, 20)] public int health;   // �÷��̾� ü�� (Inspector���� ���� ����)
    [Range(0, 10)] public int attack;   // �÷��̾� ���ݷ�
    [Range(0, 10)] public int defense;  // �÷��̾� ����

    public bool isStealthed;            // ���� ����
    public bool isFirstAttack;          // ù ���� ����

    public void Initialize()
    {
        // CharacterSO���� �⺻ ü���� ������ �ʱ�ȭ
        health = characterData.baseHealth;
    }
}
