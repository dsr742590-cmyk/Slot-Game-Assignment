using TMPro;
using UnityEngine;
using System.Collections;

public enum SymbolType
{
    Seven,
    Chairy,
    Bar,
    Bail,
    Bomb
}

public class MachineController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI prizeText;
    [SerializeField] RowController[] rows;

    [SerializeField] GameObject handleNormal;
    [SerializeField] GameObject handlePulled;

    // Game settings
    [SerializeField] int spinCost = 10;
    [SerializeField] int playerCoins = 1000;

    // Prize for each symbol
    [SerializeField] int sevenPrize = 1000;
    [SerializeField] int chairyPrize = 100;
    [SerializeField] int barPrize = 250;
    [SerializeField] int bailPrize = 500;
    [SerializeField] int bombPrize = 1;

    bool isSpinning;

    private void Start()
    {
        // Show starting coins
        prizeText.text = "Coins: " + playerCoins;

        handleNormal.SetActive(true);
        handlePulled.SetActive(false);
    }

    // Called from the Spin button
    public void PullTheHandle()
    {
        if (isSpinning || playerCoins < spinCost)
            return;

        playerCoins -= spinCost;
        isSpinning = true;

        StartCoroutine(SpinMachine());
    }

    IEnumerator SpinMachine()
    {
        // Show Hendle Pulled
        handleNormal.SetActive(false);
        handlePulled.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        // Start each reel with a random result
        for (int i = 0; i < rows.Length; i++)
        {
            SymbolType result = (SymbolType)Random.Range(0, 5);

            rows[i].StartSpin(result);

            // Small delay between reels
            yield return new WaitForSeconds(0.05f);
        }

        // Wait For All Reel To Stop
        while (!rows[0].rowStopped ||
               !rows[1].rowStopped ||
               !rows[2].rowStopped)
        {
            yield return null;
        }

        // Check all symbols match
        if (rows[0].stoppedSlot == rows[1].stoppedSlot &&
            rows[1].stoppedSlot == rows[2].stoppedSlot)
        {
            int prize = GetPrize(rows[0].stoppedSlot);

            playerCoins += prize;

            prizeText.text = "Prize: " + prize +
                             "\nCoins: " + playerCoins;
        }
        else
        {
            prizeText.text = "Coins: " + playerCoins;
        }

        isSpinning = false;

        // Change Handle to normal
        handleNormal.SetActive(true);
        handlePulled.SetActive(false);
    }

    // Get the prizeAccording to the symbols
    int GetPrize(SymbolType symbol)
    {
        if (symbol == SymbolType.Seven)
            return sevenPrize;

        if (symbol == SymbolType.Chairy)
            return chairyPrize;

        if (symbol == SymbolType.Bar)
            return barPrize;

        if (symbol == SymbolType.Bail)
            return bailPrize;

        return bombPrize;
    }
}