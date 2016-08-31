using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static int maxChunkSize = 100;

    public static Chunk[,,] chunks = new Chunk[maxChunkSize * 2, maxChunkSize * 2, maxChunkSize * 2];

    static GameObject chunkPrefab;

    static GameObject playerPrefab;

    static GameObject blockSelection;

    static BlockSelectionHandler blockSelectionHandler;

    static List<Player> players = new List<Player>();

    public static Player localPlayer;

    public Vector3 spawnLoc;

    public void Start() {
        chunkPrefab = getPrefabGameObject("ChunkGameObject");

        playerPrefab = getPrefabGameObject("Player");

        blockSelection = getPrefabGameObject("BlockSelection");


        GameObject blockSelectionGameObject = Instantiate(blockSelection, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        blockSelectionHandler = blockSelectionGameObject.GetComponent<BlockSelectionHandler>();

        createChunk(0, 0, 0);

        spawnPlayer("player1", spawnLoc);
    }
    public void Update() {
        Timers.onUpdate();

        if (Input.GetKeyDown(KeyCode.E)) {
            localPlayer.onOpenInventoryKeyPress();
        }

    }
    public void FixedUpdate() {
        EventRegistry.callAllEvents();
    }
    void OnApplicationQuit() {
        Debug.Log("Saving all data");
        JsonHandler.saveAllData();
        Debug.Log("All data saved");
    }
    public static BlockSelectionHandler getBlockSelectionHandler() {
        return blockSelectionHandler;
    }
    public static GameObject getPrefabGameObject(String name) {
        return AssetDatabase.LoadAssetAtPath("Assets/Prefab/" + name + ".prefab", typeof(GameObject)) as GameObject;
    }
    public static void spawnPlayer(String playerName, Vector3 spawnLoc) {
        GameObject clone = Instantiate(playerPrefab, spawnLoc, Quaternion.identity) as GameObject;
        Player player = clone.GetComponent<Player>() as Player;
        players.Add(player);
        localPlayer = player;
    }
    public static Chunk getChunk(Vector3 vector) {
        return getChunk((int)vector.x, (int)vector.y, (int)vector.z);
    }
    public static Chunk getChunk(int x, int y, int z) {
        int xx = maxChunkSize + x;
        int yy = maxChunkSize + y;
        int zz = maxChunkSize + z;
        try {
            Chunk chunk = chunks[xx, yy, zz];
            if (chunk == null) {
                chunk = createChunk(x, y, z);
                setChunk(x, y, z, chunk);
            }
            return chunk;

        } catch (Exception e) {
            print(e.Message + ": " + xx + "  " + yy + "  " + zz);
            return null;
        }
    }
    private static void setChunk(int x, int y, int z, Chunk chunk) {
        x += maxChunkSize;
        y += maxChunkSize;
        z += maxChunkSize;
        chunks[x, y, z] = chunk;
    }
    public static Chunk createChunk(int x, int y, int z) {
        GameObject clone = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        Chunk chunk = clone.GetComponent<Chunk>();
        if (chunk != null) {
            setChunk(x, y, z, chunk);
        } else {
            Debug.Log("chunk component not found");
        }
        return chunk;
    }
}
