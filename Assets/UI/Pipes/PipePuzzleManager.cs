using UnityEngine;
using System.Collections.Generic;

public class PipePuzzleManager : MonoBehaviour
{
    public static PipePuzzleManager Instance;

    public PipePiece[] pipes;

    private Dictionary<Vector2Int, PipePiece> grid =
        new Dictionary<Vector2Int, PipePiece>();

    private PipePiece startPipe;
    private PipePiece endPipe;

    private void Awake()
    {
        Instance = this;

        foreach (PipePiece pipe in pipes)
        {
            grid.Add(pipe.gridPosition, pipe);

            if (pipe.isStart)
                startPipe = pipe;

            if (pipe.isEnd)
                endPipe = pipe;
        }
    }

    public void CheckPuzzle()
    {
        HashSet<PipePiece> visited = new();

        bool solved = Traverse(startPipe, visited);

        if (solved)
        {
            Debug.Log("PUZZLE COMPLETADO");
        }
    }

    bool Traverse(PipePiece current, HashSet<PipePiece> visited)
    {
        if (current == endPipe)
            return true;

        visited.Add(current);



        foreach (PipeDirection direction in current.GetConnections())
        {
            Vector2Int nextPos =
                current.gridPosition + DirectionToVector(direction);

            if (!grid.ContainsKey(nextPos))
                continue;

            PipePiece neighbour = grid[nextPos];

            if (visited.Contains(neighbour))
                continue;

            PipeDirection opposite = GetOpposite(direction);

            if (!neighbour.HasConnection(opposite))
                continue;

            if (Traverse(neighbour, visited))
                return true;
        }

        return false;
    }

    Vector2Int DirectionToVector(PipeDirection direction)
    {
        return direction switch
        {
            PipeDirection.Up => Vector2Int.up,
            PipeDirection.Down => Vector2Int.down,
            PipeDirection.Left => Vector2Int.left,
            PipeDirection.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }

    PipeDirection GetOpposite(PipeDirection direction)
    {
        return direction switch
        {
            PipeDirection.Up => PipeDirection.Down,
            PipeDirection.Down => PipeDirection.Up,
            PipeDirection.Left => PipeDirection.Right,
            PipeDirection.Right => PipeDirection.Left,
            _ => direction
        };
    }
}