import javafx.scene.control.TextField;
import javafx.scene.layout.Pane;
import javafx.scene.text.Text;
import java.util.List;

public class Lobby {
    private Text userCount;
    private Text roomsCount;
    private TextField roomName;
    private GameStart gameStart;
    private Pane content;
    private Pane room;
    private Pane lobby;
    private boolean inLobby;
    private float currentTime = 0f;
    private float updateDelay = 2f;

    public void update(float deltaTime) {
        if (inLobby) {
            currentTime += deltaTime;
            if (currentTime >= updateDelay) {
                updateLobby();
                currentTime = 0.0f;
            }
        }
        userCount.setText(String.valueOf(NetworkManager.getInstance().getUserCount()));
        roomsCount.setText(String.valueOf(NetworkManager.getInstance().getRoomsCount()));
    }

    public void clickNetworkPlay() {
        joinLobby();
    }

    private void joinLobby() {
        NetworkManager.getInstance().joinLobby().thenAccept(result -> {
            inLobby = NetworkManager.getInstance().checkInLobby();
            if (inLobby) {
                lobby.setVisible(true);
            } else {
                LoginGameManager.getInstance().failedToConnect();
            }
        });
    }

    public void clickCreateRoom() {
        NetworkManager.getInstance().createRoom(roomName.getText());
        gameStart.startGame(2);
    }

    private void updateLobby() {
        content.getChildren().clear();
        List<LobbyRoom> roomList = NetworkManager.getInstance().getRoomList();
        roomsCount.setText(String.valueOf(roomList.size()));
        
        for (LobbyRoom lobbyRoom : roomList) {
            Pane temp = new Pane(room);
            // 룸 정보 설정
            temp.setUserData(lobbyRoom);
            // 클릭 이벤트 설정
            temp.setOnMouseClicked(event -> clickRoom());
            content.getChildren().add(temp);
        }
    }
}