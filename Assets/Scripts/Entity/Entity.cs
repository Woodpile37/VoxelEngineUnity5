using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public string entityName;

    public float eyeHeigh = 0.8f;

    void Start() {
        init();
    }
    public virtual void init() {

    }
    public GameObject getGameObject() {
        return gameObject;
    }
    public Location getLocation() {
        Location l = new Location(getGameObject().transform.position);
        l.setDirection(getDirection());
        return l;
    }
    public virtual Location getEyeLocation() {
        return getLocation().add(0, eyeHeigh, 0);
    }
    public virtual Vector3 getDirection() {
        return new Vector3();
    }
    public void teleport(Location l) {
        getGameObject().transform.position = l.toVector();
    }
    void Update() {

    }
}
