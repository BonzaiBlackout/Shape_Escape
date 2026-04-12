using UnityEngine;
using TMPro;

public class DeathCounterUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = "Deaths: " + PlayerController2D.deathCount;
    }
}
