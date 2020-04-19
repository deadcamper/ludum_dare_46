using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailStateUI : MonoBehaviour
{
    public ConwayGame conwayGame; // Listens to this

    public GameObject failMessage;

    bool hasFailed = false;

    private void Start()
    {
        failMessage.SetActive(false);
    }
    private void Update()
    {
        if (!hasFailed)
        {
            int numSeeds = conwayGame.seeds;
            int numLivePlants = conwayGame.gameOfLife.aliveCells.Count;

            int resources = numSeeds + numLivePlants;

            if (resources < 3) // it is impossible to sustain Conway with less than three seeds
            {
                hasFailed = true;
                StartCoroutine(HandleFailure());
            }
        }
    }

    private IEnumerator HandleFailure()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            failMessage.SetActive(!failMessage.activeSelf);
            yield return new WaitForSeconds(1);
        }
    }
}
