using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{		
	// Все это костыль галимый... тачем передвигать нужно. TODO
	private Group group;
	// Start is called before the first frame update

	private static MoveManager _instance;

	public static MoveManager Instance
	{
		get
		{
			if (_instance == null)
			{

				Debug.LogError("Error");
			}
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	public void SetGroup(Group _group)
	{
		this.group = _group;
	}

	public void MoveLeft()
	{
		Debug.Log("MoveLeft()");
		group.MoveLeft();
	}

	public void MoveRight()
	{
		Debug.Log("MoveRight()");

		group.MoveRight();
	}

	public void Revert()
	{
		Debug.Log("Revert()");

		group.ChangeRotation();
	}

	public void MoveDown()
	{
		Debug.Log("MoveDown()");

		group.MoveToFloor(15);
	}
}
