using UnityEngine;
using System.Collections;

public class RowController : MonoBehaviour
{
    [SerializeField] float moveDistance = 0.25f;
    [SerializeField] float topPosition = 4f;
    [SerializeField] float bottomPosition = -5.5f;

    [SerializeField] float fastSpeed = 0.015f;
    [SerializeField] float normalSpeed = 0.025f;
    [SerializeField] float slowSpeed = 0.06f;

    public bool rowStopped = true;
    public SymbolType stoppedSlot;

    // Starts the reel spin
    public void StartSpin(SymbolType targetSymbol)
    {
        if (!rowStopped)
            return;

        StartCoroutine(Spin(targetSymbol));
    }

    IEnumerator Spin(SymbolType targetSymbol)
    {
        rowStopped = false;

        // Start fast
        for (int i = 0; i < 40; i++)
        {
            MoveReel();
            yield return new WaitForSeconds(fastSpeed);
        }

        // Slow down a little
        for (int i = 0; i < 20; i++)
        {
            MoveReel();
            yield return new WaitForSeconds(normalSpeed);
        }

        // Slow down before stop
        for (int i = 0; i < 15; i++)
        {
            MoveReel();
            yield return new WaitForSeconds(slowSpeed);
        }

        float targetPosition = GetSymbolPosition(targetSymbol);

        // Move until we Get the result
        for (int i = 0; i < 40; i++)
        {
            if (Mathf.Abs(transform.position.y - targetPosition) <= moveDistance)
                break;

            MoveReel();

            yield return new WaitForSeconds(slowSpeed);
        }

        //To Ensure symbol is right position
        transform.position = new Vector3(
            transform.position.x,
            targetPosition,
            transform.position.z
        );

        stoppedSlot = targetSymbol;
        rowStopped = true;
    }

    private void MoveReel()
    {
        float newY = transform.position.y - moveDistance;

        // Create Illusion first to last
        if (newY <= bottomPosition)
            newY = topPosition;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    // Get position of each symbol
    private float GetSymbolPosition(SymbolType symbol)
    {
        switch (symbol)
        {
            case SymbolType.Chairy:
                return 4f;

            case SymbolType.Bail:
                return 1.75f;

            case SymbolType.Seven:
                return -0.75f;

            case SymbolType.Bar:
                return -3.25f;

            case SymbolType.Bomb:
                return -5.5f;
        }

        return 4f;
    }
}