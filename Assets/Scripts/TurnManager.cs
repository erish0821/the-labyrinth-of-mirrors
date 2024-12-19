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
	[SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
	[SerializeField] [Tooltip("카드 배분이 매우 빨라집니다")] bool fastMode;
	[SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

	[Header("Properties")]
	public bool isLoading; // 게임 끝나면 isLoading을 true로 하면 카드와 엔티티 클릭방지
	public bool myTurn;

	enum ETurnMode { Random, My, Other }
	WaitForSeconds delay05 = new WaitForSeconds(0.1f); //시간 조절
	WaitForSeconds delay07 = new WaitForSeconds(0.1f); //시간 조절

    public static Action<bool, int> OnAddCard; // isMine, targetPlayer
    public static event Action<bool> OnTurnStarted;
    private int currentPlayerIndex = 0; // 현재 플레이어 인덱스 (0: MyPlayer, 1~4: OtherPlayers)

    private int GetNextPlayerIndex()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex > 4) currentPlayerIndex = 1; // 순환 (0은 MyPlayer)
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

		myTurn = true; //이거 때문에 계속 내턴임
	}

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;

        // 플레이어 순서대로 5장씩 분배
        int totalPlayers = 5; // MyPlayer(1) + OtherPlayers(4)
        int cardsPerPlayer = 5;

        for (int cardCount = 0; cardCount < cardsPerPlayer; cardCount++)
        {
            for (int playerIndex = 0; playerIndex < totalPlayers; playerIndex++)
            {
                yield return delay05;

                // MyPlayer는 playerIndex 0, OtherPlayers는 1~4로 처리
                bool isMine = (playerIndex == 0);
                OnAddCard?.Invoke(isMine, playerIndex - 1); // OtherPlayerIndex는 -1로 변환
            }
        }

        StartCoroutine(StartTurnCo());
    }


    IEnumerator StartTurnCo()
	{
		isLoading = true;
		if (myTurn)
			GameManager.Inst.Notification("나의 턴");

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
