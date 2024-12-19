using UnityEngine;

[CreateAssetMenu(fileName = "RoleCardSO", menuName = "Game/RoleCardSO")]
public class RoleCardSO : ScriptableObject
{
    public string roleName;             // ���� �̸�
    public string description;          // ���� ���� (�¸� ���� ����)
    public Sprite roleFrontImage;       // ���� ī�� �ո� �̹���
    public Sprite roleBackImage;        // ���� ī�� �޸� �̹���
    public Role roleType;               // ���� Ÿ�� (������)
}
