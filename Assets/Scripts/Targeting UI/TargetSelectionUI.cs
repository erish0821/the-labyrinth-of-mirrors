using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUI : MonoBehaviour
{
    public static TargetSelectionUI Instance { get; private set; }

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(List<PlayerData> players, Action<PlayerData> onTargetSelected)
    {
        gameObject.SetActive(true);

        // ���� ��ư ����
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // �÷��̾ ��ư ����
        foreach (var player in players)
        {
            var button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<Text>().text = player.playerName;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                onTargetSelected?.Invoke(player);
                Hide();
            });
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
