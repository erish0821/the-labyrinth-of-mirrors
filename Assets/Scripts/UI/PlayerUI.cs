using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text playerNameText;        // �÷��̾� �̸�
    public Image roleCardImage;            // ���� ī�� �̹���
    public TMP_Text roleCardName;          // ���� �̸�
    public TMP_Text HPText;                // ü�� ǥ�� �ؽ�Ʈ

    public Image characterImage;           // ĳ���� �̹���
    public TMP_Text characterNameText;     // ĳ���� �̸�

    public Image weaponImage;              // ���� �̹��� (���� ��Ȱ��ȭ)
    public Image potionImage;              // ���� �̹��� (���� ��Ȱ��ȭ)
    public Image debuffImage;              // ����� �̹��� (���� ��Ȱ��ȭ)

    private PlayerData currentPlayerData;  // ���� �÷��̾� ������ ĳ��

    // �÷��̾� UI �ʱ� ����
    public void Setup(PlayerData player, bool isMyUI)
    {
        currentPlayerData = player; // ���� �÷��̾� �����͸� ĳ��

        // 1. **�÷��̾� �̸� ����**
        playerNameText.text = player.playerName;

        // 2. **ü�� ǥ��**
        UpdateHealth(player.health);

        // 3. **���� ī�� ����**
        if (isMyUI || player.isRoleRevealed) // ���� ���� UI�̰ų� ������ ������ ���
        {
            roleCardImage.sprite = player.roleCard.roleFrontImage;
            roleCardName.text = player.roleCard.roleName;
        }
        else // �ٸ� �÷��̾�� ������ ������� ���
        {
            roleCardImage.sprite = player.roleCard.roleBackImage;
            roleCardName.text = "";
        }

        // 4. **ĳ���� ī�� ���� (�׻� ����)**
        characterImage.sprite = player.characterData.characterSprite;
        characterNameText.text = player.characterData.characterName;

        // 5. **������/���� ī�� ���� (���� ��Ȱ��ȭ)**
        weaponImage.gameObject.SetActive(false); // ���� ī�� ����
        potionImage.gameObject.SetActive(false); // ���� ī�� ����
        debuffImage.gameObject.SetActive(false); // ����� ī�� ����
    }

    // ü�� ������Ʈ �޼���
    public void UpdateHealth(int health)
    {
        if (currentPlayerData != null)
        {
            HPText.text = $"HP: {currentPlayerData.health}"; // ü���� UI�� ǥ��
        }
        else
        {
            Debug.LogWarning("PlayerData�� �������� �ʾҽ��ϴ�!");
        }
    }

    // ������/���� �̹��� ������Ʈ
    public void UpdateItemImages()
    {
        if (currentPlayerData != null)
        {
            // ���� �̹��� ������Ʈ
            if (currentPlayerData.weaponCard != null)
            {
                weaponImage.sprite = currentPlayerData.weaponCard.sprite;
                weaponImage.gameObject.SetActive(true);
            }
            else
            {
                weaponImage.gameObject.SetActive(false);
            }

            // ���� �̹��� ������Ʈ
            if (currentPlayerData.potionCard != null)
            {
                potionImage.sprite = currentPlayerData.potionCard.sprite;
                potionImage.gameObject.SetActive(true);
            }
            else
            {
                potionImage.gameObject.SetActive(false);
            }

            // ����� �̹��� ������Ʈ
            /*if (currentPlayerData.debuffCard != null)
            {
                debuffImage.sprite = currentPlayerData.debuffCard.sprite;
                debuffImage.gameObject.SetActive(true);
            }
            else
            {
                debuffImage.gameObject.SetActive(false);
            }*/
        }
        else
        {
            Debug.LogWarning("PlayerData�� �������� �ʾҽ��ϴ�!");
        }
    }
}
