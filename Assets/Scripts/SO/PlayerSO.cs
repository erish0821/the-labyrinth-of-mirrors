using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Game/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public List<PlayerData> players; // 플레이어 데이터 리스트
}

[System.Serializable]
public class PlayerData
{
    public int id; // 고유 ID
    public string playerName;           // 플레이어 이름
    public RoleCardSO roleCard;         // 역할 카드
    public CharacterSO characterData;  // 캐릭터 데이터
    public bool isRoleRevealed;         // 역할 공개 여부

    public Item weaponCard;             // 무기 카드 (ItemSO와 호환)
    public Item potionCard;             // 포션 카드 (ItemSO와 호환)
    public Item debufferCard;           // 디버프 카드 (ItemSO와 호환)

    [Range(1, 20)] public int health;   // 플레이어 체력 (Inspector에서 조정 가능)
    [Range(0, 10)] public int attack;   // 플레이어 공격력
    [Range(0, 10)] public int defense;  // 플레이어 방어력

    public bool isStealthed;            // 은신 여부
    public bool isFirstAttack;          // 첫 공격 여부

    public void Initialize()
    {
        // CharacterSO에서 기본 체력을 가져와 초기화
        health = characterData.baseHealth;
    }
}
