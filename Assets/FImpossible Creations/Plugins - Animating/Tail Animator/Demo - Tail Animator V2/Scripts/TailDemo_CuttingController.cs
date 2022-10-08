using UnityEngine;
using UnityEngine.UI;

public class TailDemo_CuttingController : MonoBehaviour
{
    public Text CountText;
    public Slider slider;
    public TailDemo_SegmentedTailGenerator generator;


    private void Start()
    {
        if (slider)
        {
            slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
    }

    void Update()
    {
        if (CountText)
        {
            if (generator)
            {
                CountText.text = "Segments Count: " + generator.SegmentsCount;
            }
        }

        if (generator)
        {
            if (slider)
            {
                if (slider.value != generator.SegmentsCount)
                    slider.value = generator.SegmentsCount;
            }
        }
    }

    public void ValueChangeCheck()
    {
        generator.SegmentsCount = (int)slider.value;
        generator.OnValidate();
    }
}
