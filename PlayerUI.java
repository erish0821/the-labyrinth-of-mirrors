import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.text.Text;

public class PlayerUI {
    private Text playerNameText;
    private ImageView roleCardImage;
    private Text roleCardName;
    private Text hpText;
    private ImageView characterImage;
    private Text characterNameText;
    private ImageView weaponImage;
    private Text weaponNameText;
    private ImageView potionImage;
    private ImageView debuffImage;
    private PlayerData currentPlayerData;
    private Text cardCount;
    private Item item;

    public void setup(PlayerData player, boolean isMyUI) {
        currentPlayerData = player;

        if (player.getPlayerName() != null && !player.getPlayerName().isEmpty()) {
            playerNameText.setText(player.getPlayerName());
        } else {
            playerNameText.setText("Unknown Player");
            System.out.println("Warning: PlayerName is empty!");
        }

        updateHealth(player.getHealth());

        if (isMyUI || player.isRoleRevealed()) {
            roleCardImage.setImage(player.getRoleCard().getRoleFrontImage());
            roleCardName.setText(player.getRoleCard().getRoleName());
        } else {
            roleCardImage.setImage(player.getRoleCard().getRoleBackImage());
            roleCardName.setText("");
        }

        characterImage.setImage(player.getCharacterData().getCharacterSprite());
        characterNameText.setText(player.getCharacterData().getCharacterName());

        weaponImage.setVisible(false);
        potionImage.setVisible(false);
        debuffImage.setVisible(false);
    }

    public void updateHealth(int health) {
        if (currentPlayerData != null) {
            hpText.setText("HP: " + currentPlayerData.getHealth());
        } else {
            System.out.println("Warning: PlayerData is not set!");
        }
    }

    public void updateItemImages(Item item, PlayerData currentPlayerData) {
        if (currentPlayerData != null) {
            if (currentPlayerData.getWeaponCard() != null) {
                weaponImage.setImage(item.getSprite());
                weaponImage.setVisible(true);
                weaponNameText.setText(item.getName());
                weaponNameText.setVisible(true);
            } else {
                weaponImage.setVisible(false);
                weaponNameText.setVisible(false);
            }
        } else {
            System.out.println("Warning: PlayerData is not set!");
        }
    }
}