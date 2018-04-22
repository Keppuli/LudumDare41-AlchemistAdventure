using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomTextColor : MonoBehaviour
{
    public Color32 textColor32;    // The color that will be randomly set and
    public Text textObject;        // The Text object that we want to edit.

    void Awake()
    {
        // Initialize [textObject] by getting the
        // Text component of object that this script is attached to.
        textObject = this.GetComponent<Text>();
    }

    void RandomizeTextColor()
    {
        // Randomly set each values of textColor32 by using Random.Range.
        // Call Random.Range and convert the random int value to byte.
        textColor32 = new Color32(
            (byte)Random.Range(0, 255),     // R
            (byte)Random.Range(0, 255),     // G
            (byte)Random.Range(0, 255),     // B
                                  255);               // A

        // Set the color of [textObject] to [textColor32]
        textObject.color = textColor32;
    }

    void Update()
    {
        RandomizeTextColor();
    }
}