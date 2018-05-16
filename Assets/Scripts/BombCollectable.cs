using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectable : MonoBehaviour {

    public enum Type { Normal, Freeze };
    public Type type;
    public int amount;  // Used in tutorial to provide player with 999 bombs, otherwise this is just 1
}
