using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {
	public List<int[]> tiles;
	public List<int> pillars;
	int xDim, zDim;
	
	// Use this for initialization
	void Start () {
		tiles = new List<int[]>();
		tiles.Add (new int[] {0,0,2,2,2,2,2,2,2,0});
		tiles.Add (new int[] {0,0,2,2,2,2,2,2,2,0});
		tiles.Add (new int[] {0,0,2,2,2,2,2,2,2,0});
		tiles.Add (new int[] {0,0,2,2,2,2,2,2,2,0});
		tiles.Add (new int[] {0,0,2,2,0,0,0,0,0,0});
		tiles.Add (new int[] {0,0,2,2,0,0,0,0,0,0});
		tiles.Add (new int[] {1,1,1,1,0,0,0,0,0,0});
		tiles.Add (new int[] {1,1,1,1,0,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,3,2,3,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,3,2,2,0,0,0,0,0});
		tiles.Add (new int[] {0,0,3,3,3,3,3,2,3,3,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,2,2,2,2,2,2,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,1,1,1,2,3,3,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,1,1,1,2,3,0,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,1,1,1,2,3,0,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,1,1,1,2,3,0,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,4,4,4,2,3,0,0,0,0,0});
		tiles.Add (new int[] {0,0,3,2,4,4,4,2,3,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
		tiles.Add (new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
		zDim = tiles.Count - 1;
		xDim = tiles[0].Length - 1;
	
		for (int z = 1; z < zDim; ++z) {
			for (int x = 1; x < xDim; ++x) {
				if (tiles[z][x] != 0) {
					PlaceCeiling(x,z);
				}
				if (tiles[z][x-1] < tiles[z][x]) {
					PlaceZWall(x-1, z);
				}
				if (tiles[z][x+1] < tiles[z][x]) {
					PlaceZWall(x, z);
				}
				if (tiles[z-1][x] < tiles[z][x]) {
					PlaceXWall(x, z-1);
				}
				if (tiles[z+1][x] < tiles[z][x]) {
					PlaceXWall(x, z);
				}
			}
		}
	}
	
	void PlaceZWall(int x, int z) {
		GameObject newXWall = Instantiate(ObjectPrefabDefinitions.main.ZWall) as GameObject;
		newXWall.transform.parent = transform;
		newXWall.transform.position = new Vector3(2 + 4 * x, 1.75f, 4 * z);
		HandlePillars(x, z);
		HandlePillars(x, z + 1);
	}
	
	void PlaceXWall(int x, int z) {
		GameObject newXWall = Instantiate(ObjectPrefabDefinitions.main.XWall) as GameObject;
		newXWall.transform.parent = transform;
		newXWall.transform.position = new Vector3(4 * x, 1.75f, 2 + 4 * z);
		HandlePillars(x - 1, z + 1);
		HandlePillars(x, z + 1);
	}
	
	void HandlePillars(int x, int z) {
		if (!pillars.Contains(x * zDim + z)) {
			pillars.Add (x * zDim + z);
		} else {
			GameObject newPillar = Instantiate(ObjectPrefabDefinitions.main.Pillar) as GameObject;
			newPillar.transform.parent = transform;
			newPillar.transform.position = new Vector3(2 + 4 * x, 1.75f, -2 + 4 * z);
		}
	}
	
	void PlaceCeiling(int x, int z) {
		GameObject newCeiling = Instantiate(ObjectPrefabDefinitions.main.Ceiling) as GameObject;
		newCeiling.transform.parent = transform;
		newCeiling.transform.position = new Vector3(4 * x, 3.625f, 4 * z);
	}
	
	//z: -4 1.5 2
	//z:  0 1.5 2
	
	//x: -2 1.5 4
	//x: -2 1.5 0
}
