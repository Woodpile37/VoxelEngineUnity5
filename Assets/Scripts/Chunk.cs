using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Chunk : MonoBehaviour {

    Block[,,] blocks;

    public static int xSize = 16;

    public static int ySize = 16;

    public static int zSize = 16;

    Vector3 chunkLocation;

    public MeshRenderer meshRenderer;

    public Mesh mesh;

    MeshFilter meshFilter;

    public MeshCollider meshCollider;

    public Vector2 triUV1_1 = new Vector2(1, 1);
    public Vector2 triUV1_2 = new Vector2(1, 0);
    public Vector2 triUV1_3 = new Vector2(0, 1);

    public Dictionary<SkyMaterial, List<int>> materialTriangles = new Dictionary<SkyMaterial, List<int>>();

    public bool isRendered = false;

    public void Start() {

        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter = GetComponent<MeshFilter>();

        meshCollider = GetComponent<MeshCollider>();

        chunkLocation = transform.position;

        blocks = new Block[Chunk.xSize, Chunk.ySize, Chunk.zSize];

        mesh = new Mesh();

        generate();

        putToRenderQueue();
    }
    public void generate() {
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                for (int z = 0; z < zSize; z++) {
                    var count = Enum.GetNames(typeof(SkyMaterial)).Length;
                    int r = UnityEngine.Random.Range(0, count);
                    SkyMaterial rsm = (SkyMaterial)r;
                    SkyMaterial sm = rsm;
                    //if (chunkLocation.Equals(new Vector3(0, 0, 0))) {
                    //    if (y == 0) {
                    //        sm = SkyMaterial.GRASS;
                    //    }
                    // }
                    blocks[x, y, z] = new Block(this, new Vector3(x, y, z), sm);
                }
            }
        }

    }
    public void render() {
        materialTriangles.Clear();

        meshRenderer.materials = new List<Material>().ToArray();

        meshFilter.mesh = null;

        this.mesh = RenderHandler.createMesh2(this);
        meshFilter.mesh = mesh;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.Optimize();

        meshCollider.sharedMesh = mesh;

        isRendered = true;
    }
    public Vector3 getChunkLocation() {
        return this.chunkLocation;
    }
    public Block getBlock(Vector3 vector) {
        return getBlock((int)vector.x, (int)vector.y, (int)vector.z);
    }
    public Block getBlock(int x, int y, int z) {
        try {
            return blocks[x, y, z];
        } catch (Exception e) {
            //Debug.Log(e.Message + "   id: " + x + "   " + y + "   " + z);
        }
        return null;
    }

    internal void putToRenderQueue() {
        RenderHandler.putChunkToRenderQueue(this);
    }
}
