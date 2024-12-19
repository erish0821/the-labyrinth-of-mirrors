package com.example.thelabyrinthofmirrors;

public class LobbyRoom {

    private String roomName;
    private int maxPlayers;
    private int playerCount;

    public LobbyRoom(String roomName, int maxPlayers, int playerCount) {
        this.roomName = roomName;
        this.maxPlayers = maxPlayers;
        this.playerCount = playerCount;
    }

    public String getRoomName() {
        return roomName;
    }

    public void setRoomName(String roomName) {
        this.roomName = roomName;
    }

    public int getMaxPlayers() {
        return maxPlayers;
    }

    public void setMaxPlayers(int maxPlayers) {
        this.maxPlayers = maxPlayers;
    }

    public int getPlayerCount() {
        return playerCount;
    }

    public void setPlayerCount(int playerCount) {
        this.playerCount = playerCount;
    }
}
