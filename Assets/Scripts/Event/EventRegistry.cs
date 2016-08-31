using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventRegistry {

    static List<PlayerInteractEvent> playerInteractEvents = new List<PlayerInteractEvent>();

    public static void registerPlayerInteractEvent(PlayerInteractEvent e) {
        playerInteractEvents.Add(e);
    }
    public static void callAllEvents() {
        foreach (PlayerInteractEvent e in playerInteractEvents) {
            EventHandler.onPlayerInteractEvent(e);
        }
        playerInteractEvents.Clear();
    }
}
