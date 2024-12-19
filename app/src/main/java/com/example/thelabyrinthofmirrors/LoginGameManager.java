package com.example.thelabyrinthofmirrors;

import android.app.Activity;
import android.os.Handler;
import android.os.Looper;
import android.widget.FrameLayout;
import android.widget.TextView;

public class LoginGameManager {

    private static LoginGameManager instance;
    private final Activity activity;
    private final Handler handler;
    public int gameMode = 0;

    private LoginGameManager(Activity activity) {
        this.activity = activity;
        this.handler = new Handler(Looper.getMainLooper());
    }

    public static synchronized LoginGameManager getInstance(Activity activity) {
        if (instance == null) {
            instance = new LoginGameManager(activity);
        }
        return instance;
    }

    public void failedToConnect() {
        printMessage("서버 접속 실패");
    }

    public void roomIsFull() {
        printMessage("방이 가득 찼습니다");
    }

    public void joinFailed() {
        printMessage("방에 참가할 수 없습니다");
    }

    private void printMessage(String message) {
        handler.post(() -> {
            TextView textView = new TextView(activity);
            textView.setText(message);
            textView.setTextSize(24f);
            textView.setTextColor(0xFFFFFFFF); // White color

            FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(
                    FrameLayout.LayoutParams.WRAP_CONTENT,
                    FrameLayout.LayoutParams.WRAP_CONTENT
            );
            textView.setLayoutParams(layoutParams);

            FrameLayout rootLayout = activity.findViewById(android.R.id.content);
            rootLayout.addView(textView);

            handler.postDelayed(() -> rootLayout.removeView(textView), 1000L); // Message duration
        });
    }
}

