using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectable : MonoBehaviour {

    public enum Type { Normal, Freeze };
    public Type type;
    public int amount;
}
