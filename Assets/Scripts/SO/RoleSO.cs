using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoleSO", menuName = "Game/RoleSO")]
public class RoleSO : ScriptableObject
{
    public List<RoleData> roles; // 역할 리스트
}

[System.Serializable]
public class RoleData
{
    public string roleName; // 역할 이름
    public Role roleType;   // 역할 Enum (공대장, 부대장 등)
}

public enum Role
{
    TeamLeader,      // 공대장
    Lieutenant,      // 부대장
    Doppelganger,    // 도플갱어
    DarkSorcerer     // 암흑교단
}
