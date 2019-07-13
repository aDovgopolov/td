using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures;

public class GroupTest : MonoBehaviour
{
	private Figure figure;

	void Start()
    {
		figure = FindObjectOfType<Spawner>().PrepareFigure(this);
		
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);
		}
	}
	
	public void RemoveGroupTestScriptsFromObject()
	{
		enabled = false;
		gameObject.GetComponent<Group>().enabled = true;
	}

	public Figure GiveOwnFigure()
	{
		return figure;
	}
}
