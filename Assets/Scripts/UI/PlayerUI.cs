using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text playerNameText;        // 플레이어 이름
    public Image roleCardImage;            // 역할 카드 이미지
    public TMP_Text roleCardName;          // 역할 이름
    public TMP_Text HPText;                // 체력 표시 텍스트

    public Image characterImage;           // 캐릭터 이미지
    public TMP_Text characterNameText;     // 캐릭터 이름

    public Image weaponImage;              // 무기 이미지 (현재 비활성화)
    public Image potionImage;              // 포션 이미지 (현재 비활성화)
    public Image debuffImage;              // 디버프 이미지 (현재 비활성화)

    private PlayerData currentPlayerData;  // 현재 플레이어 데이터 캐싱

    // 플레이어 UI 초기 설정
    public void Setup(PlayerData player, bool isMyUI)
    {
        currentPlayerData = player; // 현재 플레이어 데이터를 캐싱

        // 1. **플레이어 이름 설정**
        playerNameText.text = player.playerName;

        // 2. **체력 표시**
        UpdateHealth(player.health);

        // 3. **역할 카드 설정**
        if (isMyUI || player.isRoleRevealed) // 내가 보는 UI이거나 역할이 공개된 경우
        {
            roleCardImage.sprite = player.roleCard.roleFrontImage;
            roleCardName.text = player.roleCard.roleName;
        }
        else // 다른 플레이어에게 역할이 비공개인 경우
        {
            roleCardImage.sprite = player.roleCard.roleBackImage;
            roleCardName.text = "";
        }

        // 4. **캐릭터 카드 설정 (항상 공개)**
        characterImage.sprite = player.characterData.characterSprite;
        characterNameText.text = player.characterData.characterName;

        // 5. **아이템/버프 카드 설정 (현재 비활성화)**
        weaponImage.gameObject.SetActive(false); // 무기 카드 숨김
        potionImage.gameObject.SetActive(false); // 포션 카드 숨김
        debuffImage.gameObject.SetActive(false); // 디버프 카드 숨김
    }

    // 체력 업데이트 메서드
    public void UpdateHealth(int health)
    {
        if (currentPlayerData != null)
        {
            HPText.text = $"HP: {currentPlayerData.health}"; // 체력을 UI에 표시
        }
        else
        {
            Debug.LogWarning("PlayerData가 설정되지 않았습니다!");
        }
    }

    // 아이템/버프 이미지 업데이트
    public void UpdateItemImages()
    {
        if (currentPlayerData != null)
        {
            // 무기 이미지 업데이트
            if (currentPlayerData.weaponCard != null)
            {
                weaponImage.sprite = currentPlayerData.weaponCard.sprite;
                weaponImage.gameObject.SetActive(true);
            }
            else
            {
                weaponImage.gameObject.SetActive(false);
            }

            // 포션 이미지 업데이트
            if (currentPlayerData.potionCard != null)
            {
                potionImage.sprite = currentPlayerData.potionCard.sprite;
                potionImage.gameObject.SetActive(true);
            }
            else
            {
                potionImage.gameObject.SetActive(false);
            }

            // 디버프 이미지 업데이트
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
            Debug.LogWarning("PlayerData가 설정되지 않았습니다!");
        }
    }
}
