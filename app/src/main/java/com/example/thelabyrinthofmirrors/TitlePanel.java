package com.example.thelabyrinthofmirrors;

import android.app.Activity;
import android.view.View;

public class TitlePanel {

    private final View titlePanelView;
    private final GameManager gameManager;

    public TitlePanel(Activity activity, GameManager gameManager) {
        this.gameManager = gameManager;
        titlePanelView = activity.findViewById(R.id.titlePanel);
    }

    public void startGameClick() {
        gameManager.startGame();
        setActive(false);
    }

    public void setActive(boolean isActive) {
        titlePanelView.setVisibility(isActive ? View.VISIBLE : View.GONE);
    }
}