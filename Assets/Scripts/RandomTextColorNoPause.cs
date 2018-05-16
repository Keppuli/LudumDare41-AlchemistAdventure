using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Customized version for game over text that ignores Unity's time.scale 0
public class RandomTextColorNoPause : MonoBehaviour {

    public Color32 textColor32;    // The color that will be randomly set and
    private Text textObject;        // The Text object that we want to edit.

    void Awake()
    {
        textObject = GetComponent<Text>(); // Default text object to control is this object
    }

    void Update()
    {
        RandomizeTextColor();
    }

    void RandomizeTextColor()
    {
        // Call Random.Range and convert the random int value to byte.
        textColor32 = new Color32(
            (byte)Random.Range(0, 255),     // R
            (byte)Random.Range(0, 255),     // G
            (byte)Random.Range(0, 255),     // B
            (byte)Random.Range(255, 255));    // A

        // Set the color of [textObject] to [textColor32]
        textObject.color = textColor32;
    }


}
