using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EventHandler {

    public static void onPlayerInteractEvent(PlayerInteractEvent e) {
        Player player = e.getPlayer();
        player.onPlayerInteractEvent(e);


    }
}
