using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// TargetType 열거형 정의
public enum TargetType
{
    Self,           // 나 자신
    Single,         // 특정 대상 1명 (자신 포함)
    All,            // 전체 대상 (자신 포함)
    AllExceptSelf   // 전체 대상 (자신 제외)
}

public class TargetingManager : MonoBehaviour
{
    public static TargetingManager Inst { get; private set; } // 싱글턴 인스턴스
    private PlayerData selectedTarget;

    private void Awake()
    {
        // 싱글턴 초기화
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        Debug.Log("TargetingManager가 초기화되었습니다.");
    }

    //Targeting 과정 매서드
    public void StartTargetSelection(List<PlayerData> players, System.Action<PlayerData> onTargetSelected)
    {
        TargetSelectionUI.Instance.Show(players, (target) =>
        {
            selectedTarget = target;
            onTargetSelected?.Invoke(selectedTarget);
        });
    }


    // 특정 대상(타겟)을 선택하는 메서드
    public PlayerData SelectTarget(List<PlayerData> players, TargetType targetType, PlayerData currentPlayer)
    {
        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("TargetType -> SelectTarget 플레이어 목록이 비어있거나 null입니다.");
            return null;
        }

        switch (targetType)
        {
            case TargetType.Self:
                if (players != null && players.Count > 0)
                {
                    // 나 자신을 제외한 타겟 목록 생성
                    List<PlayerData> potentialTargets = new List<PlayerData>();

                    // 자신 제외
                    foreach (var player in players)
                    {
                        if (player.id == currentPlayer.id) // 고유 ID로 비교
                        {
                            potentialTargets.Add(player);
                        }
                    }
                    Debug.Log($"타겟 후보 수: {potentialTargets.Count}");
                    if (potentialTargets.Count > 0)
                    {
                        return potentialTargets[0];
                    }
                }
                break;

            case TargetType.Single:
                if (players != null && players.Count > 0)
                {
                    // 나 자신을 제외한 타겟 목록 생성
                    List<PlayerData> potentialTargets = new List<PlayerData>();

                    // 자신 제외
                    foreach (var player in players)
                    {
                        if (player.id != currentPlayer.id) // 고유 ID로 비교
                        {
                            potentialTargets.Add(player);
                        }
                    }
                    Debug.Log($"타겟 후보 수: {potentialTargets.Count}");
                    if (potentialTargets.Count > 0)
                    {
                        // 단일 대상: 랜덤으로 선택
                        PlayerData target = potentialTargets[Random.Range(0, potentialTargets.Count)];
                        return target;
                    }
                    else
                    {
                        Debug.LogWarning("타겟 후보가 없습니다!");
                    }
                }
                break;

            case TargetType.All:
                Debug.Log("전체 대상 (자신 포함)");
                if (players != null && players.Count > 0)
                {
                    // 자신 포함 전체 플레이어 목록 반환
                    return players[0]; // 여전히 첫 번째 플레이어를 선택하지만, 전체 목록을 반환하려면 수정 필요
                }
                break;

            case TargetType.AllExceptSelf:
                // 자신을 제외한 대상 리스트에서 랜덤으로 하나를 선택
                if (players != null && players.Count > 0)
                {
                    List<PlayerData> potentialTargets = new List<PlayerData>();
                    // 자신 제외
                    foreach (var player in players)
                    {
                        if (player.id != currentPlayer.id) // 고유 ID로 비교
                        {
                            potentialTargets.Add(player);
                        }
                    }
                    Debug.Log($"타겟 후보 수: {potentialTargets.Count}");
                    if (potentialTargets.Count > 0)
                    {
                        // 단일 대상: 랜덤으로 선택
                        PlayerData target = potentialTargets[Random.Range(0, potentialTargets.Count)];
                        return target;
                    }
                    else
                    {
                        Debug.LogWarning("타겟 후보가 없습니다!");
                    }
                }
                break;

            default:
                Debug.LogWarning("알 수 없는 TargetType입니다.");
                break;
        }

        Debug.LogWarning("타겟 선택 실패!");
        return null;
    }
}
