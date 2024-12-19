package com.example.thelabyrinthofmirrors;

import android.app.Activity;
import android.view.View;
import android.widget.TextView;
import android.widget.FrameLayout;
import android.animation.ObjectAnimator;

public class ResultPanel {

    private final Activity activity;
    private final TextView resultTextView;

    public ResultPanel(Activity activity) {
        this.activity = activity;

        FrameLayout rootLayout = activity.findViewById(android.R.id.content);
        resultTextView = new TextView(activity);
        resultTextView.setTextSize(24f);
        resultTextView.setVisibility(View.GONE);

        FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.WRAP_CONTENT,
                FrameLayout.LayoutParams.WRAP_CONTENT
        );
        resultTextView.setLayoutParams(layoutParams);
        rootLayout.addView(resultTextView);
    }

    public void show(String message) {
        resultTextView.setText(message);
        resultTextView.setVisibility(View.VISIBLE);

        ObjectAnimator scaleX = ObjectAnimator.ofFloat(resultTextView, "scaleX", 0f, 1f);
        ObjectAnimator scaleY = ObjectAnimator.ofFloat(resultTextView, "scaleY", 0f, 1f);
        scaleX.setDuration(500);
        scaleY.setDuration(500);
        scaleX.start();
        scaleY.start();
    }

    public void restart() {
        activity.recreate();
    }

    public void hide() {
        resultTextView.setVisibility(View.GONE);
    }
}