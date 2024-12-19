import javafx.animation.ScaleTransition;
import javafx.scene.text.Text;
import javafx.util.Duration;

public class NotificationPanel {
    private Text notificationText;

    public void show(String message) {
        notificationText.setText(message);
        
        ScaleTransition showScale = new ScaleTransition(Duration.seconds(0.3));
        showScale.setToX(1);
        showScale.setToY(1);
        
        ScaleTransition hideScale = new ScaleTransition(Duration.seconds(0.3));
        hideScale.setToX(0);
        hideScale.setToY(0);
        
        showScale.setOnFinished(e -> {
            Thread.sleep(900);
            hideScale.play();
        });
        
        showScale.play();
    }

    public void scaleZero() {
        setScale(0);
    }

    private void setScale(double scale) {
        getTransforms().setScale(scale, scale);
    }
}