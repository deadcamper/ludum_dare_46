using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConwayGameSimUI : MonoBehaviour
{
    public ConwayGame simulator;

    public TMP_Text berriesCounter;
    public TMP_Text maxDayCounter;
    public TMP_Text cropsAliveCounter;

    public TMP_Text seedsCounter;

    public TMP_Text dayCounter;
    public Image clockTimerImage;

    public TMP_Text cropsWastedCounter;

    public bool isFlip;

    private int lastDayCount;


    private void Update()
    {
        UpdateClock();
        seedsCounter.text = "Seeds\n" + simulator.seeds.ToString("00") + "/" + simulator.totalSeeds;
        berriesCounter.text = "Berries Collected\n" + simulator.berriesCollected.ToString("0");

        dayCounter.text = "Days\n" + simulator.gameOfLife.NumSteps;

        maxDayCounter.text = "Days Alive\n" + simulator.DaysAlive;

        cropsAliveCounter.text = "Crops Alive\n" + simulator.CellsAlive;

        cropsWastedCounter.text = simulator.DeadCellsRemoved + "\nCrops wasted";

        lastDayCount = simulator.gameOfLife.NumSteps;
    }
    
    private void UpdateClock()
    {
        float offFill = (simulator.TimeToNextStep / simulator.secondsBetweenSteps) * 1.25f;
        float fuzzyFill = offFill * offFill;

        if (simulator.gameOfLife.NumSteps != lastDayCount)
        {
            isFlip ^= true;
        }

        clockTimerImage.fillClockwise = isFlip;

        if (isFlip)
        {
            fuzzyFill = 1 - fuzzyFill;
        }

        clockTimerImage.fillAmount = fuzzyFill;
    }
}
