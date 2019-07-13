using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures;
using System;

public class SplashManager
{
	private static SplashManager _instance;
	private Group group;
	delegate void Splashing();
	Splashing splashing;

	public SplashManager(Group _group)
	{
		this.group = _group;

		//Sum2((x, y) => { PlaceOfConnection2(x, y); });
	}

	//delegate void IsEqual2(Transform x, Vector2 y);

	

	//private static int Sum2(IsEqual2 func)
	//{
	//	int result = 0;
	//	func(null, new Vector2(0, 0));
	//	Debug.Log("private static int Sum2(IsEqual2 func)");
	//	return result;
	//}

	//public void PlaceOfConnection2(Transform child, Vector2 v)
	//{
	//	Debug.Log("PlaceOfConnection2");
	//}




	public void CheckCollisions()
	{
		foreach (Transform child in group.gameObject.transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);

			if (v.y == 0) continue;

			bool isSomethingDown = Grid.grid[(int)v.x, (int)v.y - 1] != null;
			if (isSomethingDown && Grid.grid[(int)v.x, (int)v.y - 1].parent != group.gameObject.transform)
			{
				PlaceOfConnection(child, v);
			}
		}

		if (group.Figure.HasSecondFloor && splashing != null)
			splashing();

		Grid.FillEmptyGrid(group);
	}

	public void PlaceOfConnection(Transform child, Vector2 v)
	{
		if (group.Figure.HasSecondFloor)
		{
			if (child.GetSiblingIndex() % 2 == (group.Figure.RotatedBy180GameObject ? 1 : 0))
			{
				CheckColorDownElement(child.GetSiblingIndex(), v);
			}
		}
		else
			CheckColorDownElement(child.GetSiblingIndex(), v);
	}

	public void CheckColorDownElement(int ChildIndex, Vector2 v)
	{
		SingleBrick ob2 = Grid.grid[(int)v.x, (int)v.y - 1].gameObject.GetComponent<SingleBrick>();

		if (group.Figure.AllBricks[ChildIndex].Universal || group.Figure.AllBricks[ChildIndex].MyColor == ob2.MyColor || ob2.Universal)
		{
			Grid.DeleteElements((int)v.x, (int)v.y);
			UIManager.Instance.UpdatePlayerCount(group.Figure.BrickCost * 2);
			if (splashing == null)
				splashing = SplashingPair;
		}
	}

	public void SplashingPair()
	{
		foreach (Transform child in group.gameObject.transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);

			if ((int)v.y == 0) continue;

			bool isSomethingDown = Grid.grid[(int)v.x, (int)v.y - 1] != null;
			if (isSomethingDown && Grid.grid[(int)v.x, (int)v.y - 1].parent != group.gameObject.transform)
			{
				CheckColorDownElement(child.GetSiblingIndex(), v);
			}
		}
		//Grid.FillEmptyGrid(group);
	}
}
