using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid :MonoBehaviour
{
	public static int w = 10;
	public static int h = 20;
	public static Transform[,] grid = new Transform[w, h];

	public static Vector2 RoundVec2(Vector2 v)
	{
		return new Vector2(Mathf.Round(v.x),
						   Mathf.Round(v.y));
	}

	public static bool InsideBorder(Vector2 pos)
	{
		return ((int)pos.x >= 0 &&
				(int)pos.x < w &&
				(int)pos.y >= 0);
	}

	public static void DeleteElements(int x, int y)
	{
		Destroy(grid[x, y].gameObject);
		Destroy(grid[x, y - 1].gameObject);
		grid[x, y] = null;
		grid[x, y - 1] = null;

		if (grid[x, y + 1] != null)
		{
			grid[x, y - 1] = grid[x, y + 1];
			grid[x, y + 1] = null;
			
			grid[x, y - 1].position += new Vector3(0, -2, 0);
		}
	}

	public static void UpdateGrid(Group group)
	{
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.w; ++x)
				if (Grid.grid[x, y] != null)
					if (Grid.grid[x, y].parent == group.gameObject.transform)
						Grid.grid[x, y] = null;

		foreach (Transform child in group.gameObject.transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);
			Grid.grid[(int)v.x, (int)v.y] = child;
		}
	}

	public static void FillEmptyGrid(Group group)
	{
		foreach (Transform child in group.gameObject.transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);
			if ((int)v.y == -1) return;

			while (v.y - 1 >= 0 && Grid.grid[(int)v.x, (int)v.y - 1] == null)
			{
				Grid.grid[(int)v.x, (int)v.y - 1] = child;
				Grid.grid[(int)v.x, (int)v.y - 1].position += new Vector3(0, -1, 0);
				Grid.grid[(int)v.x, (int)v.y] = null;
				v.y = v.y - 1;
			}
		}
	}


	public static bool IsValidGridPos(Group group)
	{
		foreach (Transform child in group.gameObject.transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);

			if (!Grid.InsideBorder(v))
				return false;

			if (Grid.grid[(int)v.x, (int)v.y] != null &&
				Grid.grid[(int)v.x, (int)v.y].parent != group.gameObject.transform)
				return false;
		}
		return true;
	}

}
