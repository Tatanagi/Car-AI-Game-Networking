using UnityEngine;
using TMPro;

public class StopLight : MonoBehaviour
{
    [Header("Light Timing")]
    public float greenTime = 5f;
    public float redTime = 5f;

    [Header("Current State")]
    public bool isGreen = true;

    [Header("UI Display")]
    public TMP_Text timerText;

    private float timer;

    private void Start()
    {
        timer = greenTime;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isGreen = !isGreen;

            if (isGreen)
            {
                timer = greenTime;
                Debug.Log("GREEN LIGHT");
            }
            else
            {
                timer = redTime;
                Debug.Log("RED LIGHT");
            }
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Show countdown (whole number)
            int displayTime = Mathf.CeilToInt(timer);
            timerText.text = displayTime.ToString();

            // Change text color based on light state
            timerText.color = isGreen ? Color.green : Color.red;
        }
    }
}