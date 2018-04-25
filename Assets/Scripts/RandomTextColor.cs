using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomTextColor : MonoBehaviour
{
    public Color32 textColor32;    // The color that will be randomly set and
    private Text textObject;        // The Text object that we want to edit.
    public int R;
    public int G;
    public int B;
    public int A;
    public float frequency;
    private IEnumerator coroutine;

    void Awake()
    {
        coroutine = ChangeColor();
        StartCoroutine(coroutine);

        textObject = this.GetComponent<Text>();
    }

    void RandomizeTextColor()
    {
        // Call Random.Range and convert the random int value to byte.
        textColor32 = new Color32(
            (byte)Random.Range(0, R),     // R
            (byte)Random.Range(0, G),     // G
            (byte)Random.Range(0, B),     // B
            (byte)Random.Range(0, A));    // A

        // Set the color of [textObject] to [textColor32]
        textObject.color = textColor32;
    }
    private IEnumerator ChangeColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            RandomizeTextColor();
        }
    }

}