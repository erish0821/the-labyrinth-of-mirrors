using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoleSO", menuName = "Game/RoleSO")]
public class RoleSO : ScriptableObject
{
    public List<RoleData> roles; // ���� ����Ʈ
}

[System.Serializable]
public class RoleData
{
    public string roleName; // ���� �̸�
    public Role roleType;   // ���� Enum (������, �δ��� ��)
}

public enum Role
{
    TeamLeader,      // ������
    Lieutenant,      // �δ���
    Doppelganger,    // ���ð���
    DarkSorcerer     // ���汳��
}
