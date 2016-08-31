using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour {

	public void loadMap() {
        SceneManager.LoadScene("Map");
    }
}
