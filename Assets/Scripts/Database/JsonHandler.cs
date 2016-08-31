using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class JsonHandler {

    public enum JsonFileType {
        ITEM, SKY_MATERIAL
    }

    static Dictionary<JsonFileType, DataHolder> dataBase = new Dictionary<JsonFileType, DataHolder>();

    public static T getData<T>(JsonFileType type, string key) {
        return getDataHolder(type).getClass<T>(key);
    }
    public static DataHolder getDataHolder(JsonFileType type) {
        DataHolder dh;
        if (dataBase.TryGetValue(type, out dh) == false) {
            dh = loadClassFromJson<DataHolder>(type, null);
            dataBase.Add(type, dh);
        }
        return dh;
    }
    private static String getFilePath(JsonFileType type) {
        return Application.dataPath + "/StreamingAssets/" + type.ToString() + ".json";
    }
    static String getDirectory(JsonFileType type, string key) {
        return Application.dataPath + "/StreamingAssets/" + type.ToString();
    }
    public static T loadClassFromJson<T>(JsonFileType type, string key) {
        string path = getFilePath(type);
        //         string dir = getDirectory(type);
        //         if (Directory.Exists(dir) == false) {
        //             Directory.CreateDirectory(dir);
        //         }
        if (File.Exists(path) == false) {
            Debug.Log(path + " does not exist, creating new one..");
            File.Create(path).Dispose();
        }
        string jString = File.ReadAllText(path);
        if (jString != "") {
            LitJson.JsonData jd;
            jd = LitJson.JsonMapper.ToObject(jString);
            Debug.Log("successfully loaded " + path + "\n" + jString);
        }
        T o = fromJson<T>(jString);
        if (o == null) {
            if (key == null) {
                o = (T)Activator.CreateInstance(typeof(T));
            } else {
                System.Object[] a = new System.Object[1] { key };
                o = (T)Activator.CreateInstance(typeof(T), a);
            }
        }
        Debug.Log(o.GetType().ToString());
        return o;
    }
    public static String toJson(System.Object o) {
        return LitJson.JsonMapper.ToJson(o);
    }
    public static T fromJson<T>(String str) {
        T o = LitJson.JsonMapper.ToObject<T>(str);
        return o;
    }
    private static void saveClassToJson(JsonFileType type, System.Object o) {
        String str = toJson(o);
        String path = getFilePath(type);
        //         String dir = getDirectory(type, key);
        //         if (Directory.Exists(dir) == false) {
        //             Directory.CreateDirectory(dir);
        //         }
        if (File.Exists(path) == false) {
            Debug.Log(path + " does not exist, creating new one..");
            File.Create(path).Dispose();
        }
        File.WriteAllText(path, str);
        Debug.Log("saved " + path + " \n Data: " + str);
    }
    public static void saveData(JsonFileType type) {
        saveClassToJson(type, getDataHolder(type));
    }
    internal static void saveAllData() {
        foreach (JsonFileType type in Enum.GetValues(typeof(JsonFileType))) {
            DataHolder dh = getDataHolder(type);
            foreach (string key in dh.dataBase.Keys) {
                saveData(type);
            }
        }
    }
}
