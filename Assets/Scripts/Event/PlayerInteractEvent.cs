using UnityEngine;
using System.Collections;

public class PlayerInteractEvent {

    public enum Action {
        RIGHT_CLICK_AIR, RIGHT_CLICK_BLOCK, LEFT_CLICK_BLOCK, LEFT_CLICK_AIR
    }

    Block clickedBlock;

    Player player;

    Direction clickedSide;

    Action action;

    public PlayerInteractEvent(Player player, Action action, Block clickedBlock, Direction clickedSide) {
        this.player = player;
        this.clickedBlock = clickedBlock;
        this.clickedSide = clickedSide;
        this.action = action;
    }

    public Player getPlayer() {
        return player;
    }
    public Direction getClickedSide() {
        return clickedSide;
    }
    public Action getAction() {
        return action;
    }
    public Block getBlock() {
        return clickedBlock;
    }
}
