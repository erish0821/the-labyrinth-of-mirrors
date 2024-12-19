using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// TargetType ������ ����
public enum TargetType
{
    Self,           // �� �ڽ�
    Single,         // Ư�� ��� 1�� (�ڽ� ����)
    All,            // ��ü ��� (�ڽ� ����)
    AllExceptSelf   // ��ü ��� (�ڽ� ����)
}

public class TargetingManager : MonoBehaviour
{
    public static TargetingManager Inst { get; private set; } // �̱��� �ν��Ͻ�
    private PlayerData selectedTarget;

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        Debug.Log("TargetingManager�� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    //Targeting ���� �ż���
    public void StartTargetSelection(List<PlayerData> players, System.Action<PlayerData> onTargetSelected)
    {
        TargetSelectionUI.Instance.Show(players, (target) =>
        {
            selectedTarget = target;
            onTargetSelected?.Invoke(selectedTarget);
        });
    }


    // Ư�� ���(Ÿ��)�� �����ϴ� �޼���
    public PlayerData SelectTarget(List<PlayerData> players, TargetType targetType, PlayerData currentPlayer)
    {
        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("TargetType -> SelectTarget �÷��̾� ����� ����ְų� null�Դϴ�.");
            return null;
        }

        switch (targetType)
        {
            case TargetType.Self:
                if (players != null && players.Count > 0)
                {
                    // �� �ڽ��� ������ Ÿ�� ��� ����
                    List<PlayerData> potentialTargets = new List<PlayerData>();

                    // �ڽ� ����
                    foreach (var player in players)
                    {
                        if (player.id == currentPlayer.id) // ���� ID�� ��
                        {
                            potentialTargets.Add(player);
                        }
                    }
                    Debug.Log($"Ÿ�� �ĺ� ��: {potentialTargets.Count}");
                    if (potentialTargets.Count > 0)
                    {
                        return potentialTargets[0];
                    }
                }
                break;

            case TargetType.Single:
                if (players != null && players.Count > 0)
                {
                    // �� �ڽ��� ������ Ÿ�� ��� ����
                    List<PlayerData> potentialTargets = new List<PlayerData>();

                    // �ڽ� ����
                    foreach (var player in players)
                    {
                        if (player.id != currentPlayer.id) // ���� ID�� ��
                        {
                            potentialTargets.Add(player);
                        }
                    }
                    Debug.Log($"Ÿ�� �ĺ� ��: {potentialTargets.Count}");
                    if (potentialTargets.Count > 0)
                    {
                        // ���� ���: �������� ����
                        PlayerData target = potentialTargets[Random.Range(0, potentialTargets.Count)];
                        return target;
                    }
                    else
                    {
                        Debug.LogWarning("Ÿ�� �ĺ��� �����ϴ�!");
                    }
                }
                break;

            case TargetType.All:
                Debug.Log("��ü ��� (�ڽ� ����)");
                if (players != null && players.Count > 0)
                {
                    // �ڽ� ���� ��ü �÷��̾� ��� ��ȯ
                    return players[0]; // ������ ù ��° �÷��̾ ����������, ��ü ����� ��ȯ�Ϸ��� ���� �ʿ�
                }
                break;

            case TargetType.AllExceptSelf:
                // �ڽ��� ������ ��� ����Ʈ���� �������� �ϳ��� ����
                if (players != null && players.Count > 0)
                {
                    List<PlayerData> potentialTargets = new List<PlayerData>();
                    // �ڽ� ����
                    foreach (var player in players)
                    {
                        if (player.id != currentPlayer.id) // ���� ID�� ��
                        {
                            potentialTargets.Add(player);
                        }
                    }
                    Debug.Log($"Ÿ�� �ĺ� ��: {potentialTargets.Count}");
                    if (potentialTargets.Count > 0)
                    {
                        // ���� ���: �������� ����
                        PlayerData target = potentialTargets[Random.Range(0, potentialTargets.Count)];
                        return target;
                    }
                    else
                    {
                        Debug.LogWarning("Ÿ�� �ĺ��� �����ϴ�!");
                    }
                }
                break;

            default:
                Debug.LogWarning("�� �� ���� TargetType�Դϴ�.");
                break;
        }

        Debug.LogWarning("Ÿ�� ���� ����!");
        return null;
    }
}
