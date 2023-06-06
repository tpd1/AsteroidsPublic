using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    private Slider progressBar;
    private TimelineController timelineController;

    private void Awake()
    {
        progressBar = GetComponent<Slider>();
    }

    private void Start()
    {
        timelineController = FindObjectOfType<TimelineController>();
    }

    private void Update()
    {
        progressBar.SetValueWithoutNotify(timelineController.GetCurrentPercent());
    }
}
