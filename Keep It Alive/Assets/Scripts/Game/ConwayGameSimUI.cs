using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConwayGameSimUI : MonoBehaviour
{
    public ConwayGame simulator;

    public TMP_Text berriesCounter;
    public TMP_Text seedsCounter;

    public Image clockTimerImage;


    private void Update()
    {
        if (simulator.TimeToNextStep > 0)
        {
            float realFill = (simulator.TimeToNextStep / simulator.secondsBetweenSteps);
            float fuzzyFill = realFill * realFill * realFill;

            clockTimerImage.fillAmount = fuzzyFill;
        }
        seedsCounter.text = "Seeds: " + simulator.seeds.ToString("00") + "/" + simulator.totalSeeds;
        berriesCounter.text = "Berries: " + simulator.berriesCollected.ToString("#");
    }
}
