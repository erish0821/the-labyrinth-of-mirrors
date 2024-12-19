public class GameStart {
    // 게임 시작 (0: VS CPU, 1: Hot Seat, 2: Network)
    public void startGame(int playMode) {
        LoginGameManager.getInstance().setGameMode(playMode);
        SceneManager.loadScene("GameScene");
    }
}