using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
	public static TurnManager Inst { get; private set; }
	void Awake() => Inst = this;

	[Header("Develop")]
	[SerializeField] [Tooltip("���� �� ��带 ���մϴ�")] ETurnMode eTurnMode;
	[SerializeField] [Tooltip("ī�� ����� �ſ� �������ϴ�")] bool fastMode;
	[SerializeField] [Tooltip("���� ī�� ������ ���մϴ�")] int startCardCount;

	[Header("Properties")]
	public bool isLoading; // ���� ������ isLoading�� true�� �ϸ� ī��� ��ƼƼ Ŭ������
	public bool myTurn;

	enum ETurnMode { Random, My, Other }
	WaitForSeconds delay05 = new WaitForSeconds(0.1f); //�ð� ����
	WaitForSeconds delay07 = new WaitForSeconds(0.1f); //�ð� ����

    public static Action<bool, int> OnAddCard; // isMine, targetPlayer
    public static event Action<bool> OnTurnStarted;
    private int currentPlayerIndex = 0; // ���� �÷��̾� �ε��� (0: MyPlayer, 1~4: OtherPlayers)

    private int GetNextPlayerIndex()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex > 4) currentPlayerIndex = 1; // ��ȯ (0�� MyPlayer)
        return currentPlayerIndex;
    }

    void GameSetup()
	{
		if (fastMode)
			delay05 = new WaitForSeconds(0.05f);

		switch (eTurnMode)
		{
			case ETurnMode.Random:
				myTurn = Random.Range(0, 2) == 0;
				break;
			case ETurnMode.My:
				myTurn = true;
				break;
			case ETurnMode.Other:
				myTurn = false;
				break;
		}

		myTurn = true; //�̰� ������ ��� ������
	}

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;

        // �÷��̾� ������� 5�徿 �й�
        int totalPlayers = 5; // MyPlayer(1) + OtherPlayers(4)
        int cardsPerPlayer = 5;

        for (int cardCount = 0; cardCount < cardsPerPlayer; cardCount++)
        {
            for (int playerIndex = 0; playerIndex < totalPlayers; playerIndex++)
            {
                yield return delay05;

                // MyPlayer�� playerIndex 0, OtherPlayers�� 1~4�� ó��
                bool isMine = (playerIndex == 0);
                OnAddCard?.Invoke(isMine, playerIndex - 1); // OtherPlayerIndex�� -1�� ��ȯ
            }
        }

        StartCoroutine(StartTurnCo());
    }


    IEnumerator StartTurnCo()
	{
		isLoading = true;
		if (myTurn)
			GameManager.Inst.Notification("���� ��");

		yield return delay07;
        OnAddCard?.Invoke(myTurn, myTurn ? 0 : GetNextPlayerIndex());
        yield return delay07;
		isLoading = false;
		OnTurnStarted?.Invoke(myTurn);
	}

	public void EndTurn()
	{
		myTurn = !myTurn;
		StartCoroutine(StartTurnCo());
	}
}
