using UnityEngine;
using System.Collections;
using System;

[SerializeField]
public class SkyMaterialSetting : SerializableClass {

    public bool opaque = true;

    public double breakingSpeed = 1;

    public string topTextureName;

    public string botTextureName;

    public string eastTextureName;

    public string westTextureName;

    public string northTextureName;

    public string southTextureName;

    public string defaultTexture;

    public SkyMaterial skyMaterial = SkyMaterial.AIR;

    public SkyMaterialSetting() {
    }
    public SkyMaterialSetting(string key) {
        SkyMaterial sm = (SkyMaterial)Enum.Parse(typeof(SkyMaterial), key);
        defaultTexture = sm.ToString();
        setDefaultValue(sm);
    }
    public void setDefaultValue(SkyMaterial skyMaterial) {
        topTextureName = skyMaterial.ToString();

        botTextureName = skyMaterial.ToString();

        westTextureName = skyMaterial.ToString();

        eastTextureName = skyMaterial.ToString();

        northTextureName = skyMaterial.ToString();

        southTextureName = skyMaterial.ToString();

        this.skyMaterial = skyMaterial;
    }
    public bool isOpaque() {
        switch (skyMaterial) {
            case SkyMaterial.AIR:
                {
                    return true;
                }
        }
        return false;
    }
    public double getBreakingSpeed() {
        return 1d;
    }
    private string getTextureName(Direction d) {
        switch (d) {
            case Direction.UP:
                {
                    return topTextureName;
                }
            case Direction.DOWN:
                {
                    return botTextureName;
                }
            case Direction.EAST:
                {
                    return eastTextureName;
                }
            case Direction.WEST:
                {
                    return westTextureName;
                }
            case Direction.NORTH:
                {
                    return northTextureName;
                }
            case Direction.SOUTH:
                {
                    return southTextureName;
                }
        }
        return "DEFAULT";
    }
    public Material getMaterial() {
        return Resources.Load<Material>("Material/" + defaultTexture);
    }
    public Material getMaterial(Direction d) {
        return Resources.Load<Material>("Material/" + getTextureName(d));
    }
}
