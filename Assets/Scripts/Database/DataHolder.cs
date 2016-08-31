using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[SerializeField]
public class DataHolder {


    public Dictionary<String, Dictionary<string, System.Object>> dataBase = new Dictionary<String, Dictionary<string, System.Object>>();

    public DataHolder() {

    }
    public T getClass<T>(string key) {
        T o;
        Dictionary<string, object> t;
        if (dataBase.TryGetValue(key, out t) == false) {
            System.Object[] a = new System.Object[1] { key };
            o = (T)Activator.CreateInstance(typeof(T), a);
            t = LitJson.JsonMapper.ToObject<Dictionary<string, object>>(LitJson.JsonMapper.ToJson(o));
            setClass(key, t);
            Debug.Log("added to database " + dataBase.Count);
        } else {
            o = LitJson.JsonMapper.ToObject<T>(LitJson.JsonMapper.ToJson(t));
        }

        return (T)o;
    }
    public void setClass(string key, System.Object c) {
        Dictionary<string, object> d = LitJson.JsonMapper.ToObject<Dictionary<string, object>>(LitJson.JsonMapper.ToJson(c));
        dataBase.Add(key, d);
    }


    /*    public Dictionary<String, string> dataBase = new Dictionary<String, string>();

    public DataHolder() {

    }
    public T getClass<T>(string key) {
        String str;
        T t;
        if (dataBase.TryGetValue(key, out str) == false) {
            System.Object[] a = new System.Object[1] { key };
            t = (T)Activator.CreateInstance(typeof(T), a);
        } else {
            t = JsonHandler.fromJson<T>(str);
        }
        return (T)t;
    }
    public void setClass(string key, System.Object o) {
        dataBase.Add(key, JsonHandler.toJson(o));
    }
    */
}
