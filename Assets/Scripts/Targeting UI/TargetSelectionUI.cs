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

        // 기존 버튼 삭제
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // 플레이어별 버튼 생성
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
