using UnityEngine;
using UnityEngine.EventSystems;

public class PipePiece : MonoBehaviour, IPointerClickHandler
{
    [Header("Posición en la rejilla")]
    public Vector2Int gridPosition;

    [Header("Conexiones base")]
    public PipeDirection[] baseConnections;

    [Header("Configuración")]
    public bool canRotate = true;
    public bool isStart;
    public bool isEnd;

    private int rotationSteps;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canRotate)
            return;

        Rotate();
    }

    void Rotate()
    {
        rotationSteps = (rotationSteps + 1) % 4;

        transform.Rotate(0, 0, -90);

        PipePuzzleManager.Instance.CheckPuzzle();
    }

    public bool HasConnection(PipeDirection direction)
    {
        foreach (var connection in GetConnections())
        {
            if (connection == direction)
                return true;
        }

        return false;
    }

    public PipeDirection[] GetConnections()
    {
        PipeDirection[] result = new PipeDirection[baseConnections.Length];

        for (int i = 0; i < baseConnections.Length; i++)
        {
            result[i] = RotateDirection(baseConnections[i]);
        }

        return result;
    }

    PipeDirection RotateDirection(PipeDirection dir)
    {
        return (PipeDirection)(((int)dir + rotationSteps) % 4);
    }
}