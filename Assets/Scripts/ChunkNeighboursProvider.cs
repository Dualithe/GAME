using UnityEngine;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ChunkGenData_Neighbour
{
    public ChunkGenData chunkData;
    public int ratio = 0;
    [Header("Optional")]
    public int amount = 0;
}

[System.Serializable]
public class ChunkGenData_Structure
{
    public GameObject structurePrefab;
    public int minAmount = 0;
    public int maxAmount = 0;
}

public class ChunkNeighboursProvider
{

    private Dictionary<string, ChunkGenData> initializedChunks = new Dictionary<string, ChunkGenData>();

    public List<ChunkGenData> PushChunkAndGetPossibleNeighbours(ChunkGenData chunk, int neighboursAmount)
    {
        chunk = InitChunkIfNotInitialized(chunk);

        var neighbours = new List<ChunkGenData>(neighboursAmount);

        int neighboursLeft = neighboursAmount;
        for (int i = 0; i < neighboursAmount; ++i)
        {
            var availableForcedChunks = chunk.neighbours.Where(row => row.amount > 0).ToList();
            if (availableForcedChunks.Count > 0)
            {
                var randomForcedChunk = availableForcedChunks[Random.Range(0, availableForcedChunks.Count)];
                randomForcedChunk.amount = System.Math.Max(randomForcedChunk.amount - 1, 0);
                neighbours.Add(randomForcedChunk.chunkData);
                neighboursLeft--;
            }
            else
            {
                break;
            }
        }
        for (int i = 0; i < neighboursLeft; ++i)
        {
            var ratio = chunk.neighbours.Select(n => n.ratio).ToList();
            var randIdx = GetIndexFromRatio(ratio);
            neighbours.Add(chunk.neighbours[randIdx].chunkData);
        }

        return neighbours;
    }

    private ChunkGenData InitChunkIfNotInitialized(ChunkGenData chunk)
    {
        if (initializedChunks.TryGetValue(chunk.Id, out var ch))
        {
            return ch;
        }
        chunk = ScriptableObject.Instantiate(chunk);
        initializedChunks.Add(chunk.Id, chunk);
        return chunk;
    }

    private int GetIndexFromRatio(List<int> ratio)
    {
        if (ratio.Count == 0) {
            return -1;
        }
        var idxes = ratio.Select((r, i) => Enumerable.Repeat(i, r)).SelectMany(l => l).ToList();
        return idxes[Random.Range(0, idxes.Count)];
    }

}
