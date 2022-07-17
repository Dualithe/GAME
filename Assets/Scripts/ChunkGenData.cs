using UnityEngine;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ChunkGenData", menuName = "GAME/ChunkGenData")]
public class ChunkGenData : ScriptableObject
{
	[SerializeField] private string id;
	public List<ChunkGenData_Structure> structures;
	public List<ChunkGenData_Neighbour> neighbours;
	public List<Item> reqItems;

	public string Id => id;


#if UNITY_EDITOR
	void OnValidate()
	{
		var path = AssetDatabase.GetAssetPath(this);
		id = AssetDatabase.AssetPathToGUID(path);
	}
#endif
}
