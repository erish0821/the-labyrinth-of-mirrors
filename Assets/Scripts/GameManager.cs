using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ġƮ, UI, ��ŷ, ���ӿ���
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Multiline(10)]
    [SerializeField] string cheatInfo;
    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] ResultPanel resultPanel;
    [SerializeField] TitlePanel titlePanel;
    [SerializeField] CameraEffect cameraEffect;
    [SerializeField] GameObject endTurnBtn;

    WaitForSeconds delay2 = new WaitForSeconds(2);


    void Start()
    {
        UISetup();
    }

    void UISetup()
    {
        notificationPanel.ScaleZero();
        resultPanel.ScaleZero();
        titlePanel.Active(true);
        cameraEffect.SetGrayScale(false);
    }

    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey() //Ű�� ���� ���� ����
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
            TurnManager.Inst.EndTurn();
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }

    public IEnumerator GameOver(bool isMyWin)
    {
        TurnManager.Inst.isLoading = true;
        endTurnBtn.SetActive(false);
        yield return delay2;

        TurnManager.Inst.isLoading = true;
        resultPanel.Show(isMyWin ? "�¸�" : "�й�");
        cameraEffect.SetGrayScale(true);
    }
}
