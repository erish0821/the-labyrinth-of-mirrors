package com.example.thelabyrinthofmirrors;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;

import java.util.List;

public class TargetSelectionUI {

    private final Activity activity;
    private final LinearLayout buttonParent;
    private View rootView;

    public TargetSelectionUI(Activity activity) {
        this.activity = activity;

        LayoutInflater inflater = activity.getLayoutInflater();
        rootView = inflater.inflate(R.layout.target_selection_ui, null);
        buttonParent = rootView.findViewById(R.id.buttonParent);
        rootView.setVisibility(View.GONE);

        activity.addContentView(rootView, new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.MATCH_PARENT
        ));
    }

    public void show(List<PlayerData> players, OnTargetSelectedListener listener) {
        rootView.setVisibility(View.VISIBLE);
        buttonParent.removeAllViews();

        for (PlayerData player : players) {
            Button button = new Button(activity);
            button.setText(player.getPlayerName());
            button.setOnClickListener(v -> {
                listener.onTargetSelected(player);
                hide();
            });
            buttonParent.addView(button);
        }
    }

    public void hide() {
        rootView.setVisibility(View.GONE);
    }

    public interface OnTargetSelectedListener {
        void onTargetSelected(PlayerData player);
    }
}