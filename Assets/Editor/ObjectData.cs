using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObject", menuName = "Custon Script/Object Data")]
public class ObjectData : ScriptableObject
{
    [Header("Object graphics")]
    public Sprite image;

    //Exemple code
    [Header("Object Config")]
    public Vector3 position;
    public Vector3 size;

    public string code;

    public bool active;

    public enum Type
    {
        player,
        mob,
        obj
    }

    public Type type;
}
