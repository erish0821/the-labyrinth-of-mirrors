using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Game/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [Header("ĳ���� �⺻ ����")]
    public string characterName;         // ĳ���� �̸�
    public int baseAttack = 0;           // �⺻ ���ݷ�
    public int baseHealth = 10;          // �⺻ ü��
    public int baseDefense = 0;          // �⺻ ����
    public Sprite characterSprite;       // ĳ���� �̹���

    [Header("ĳ���� ���� �ɷ�")]
    public string abilityName;           // �ɷ� �̸�
    [TextArea] public string description; // �ɷ� ����
    public AbilityEffect abilityEffect;  // ���� �ɷ� Enum
}
