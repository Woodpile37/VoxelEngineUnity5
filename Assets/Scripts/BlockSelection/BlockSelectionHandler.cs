using UnityEngine;
using System.Collections;

public class BlockSelectionHandler : MonoBehaviour {

    MeshRenderer meshRenderer;

    GameObject cube;

    void Start() {

        cube = transform.FindChild("Cube").gameObject;

        meshRenderer = cube.GetComponent<MeshRenderer>();
    }
    public void show(Vector3 vector) {
        meshRenderer.enabled = true;
        gameObject.transform.position = vector;
    }
    public void hide() {
        meshRenderer.enabled = false;
    }
}
