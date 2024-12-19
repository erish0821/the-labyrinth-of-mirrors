import javafx.scene.control.Button;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.paint.Color;
import javafx.scene.text.Text;

public class EndTurnBtn {
    private Image active;
    private Image inactive;
    private Text btnText;
    private Button button;
    private ImageView imageView;

    public void start() {
        // 초기에는 버튼을 비활성화로 설정
        setup(false);
        // TurnManager의 턴 시작 이벤트에 구독
        TurnManager.addOnTurnStartedListener(this::onTurnStarted);
    }

    public void destroy() {
        // 이벤트 구독 해제
        TurnManager.removeOnTurnStartedListener(this::onTurnStarted);
    }

    private void onTurnStarted(int currentPlayerIndex) {
        // currentPlayerIndex가 0이면 내 턴이므로 버튼 활성화
        boolean isActive = (currentPlayerIndex == 0);
        setup(isActive);
    }

    public void setup(boolean isActive) {
        imageView.setImage(isActive ? active : inactive);
        button.setDisable(!isActive);
        btnText.setFill(isActive ? 
            Color.rgb(255, 195, 90) : 
            Color.rgb(55, 55, 55));
    }
}