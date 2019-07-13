using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures; 

public class Group : MonoBehaviour
{
	private float lastFall = 1;
	private float clicked = 0;
	private float clicktime = 0;
	private readonly float clickdelay = 0.3f;
	private SplashManager SplashManager;
	public Figure Figure { get; private set; }

	void Start()
    {
		if (!Grid.IsValidGridPos(this))
		{
			Destroy(gameObject);
		}

		if (GetComponent<GroupTest>() != null)
			Figure =  GetComponent<GroupTest>().GiveOwnFigure();
		else
			Figure = FindObjectOfType<Spawner>().PrepareFigure(this);

		SplashManager = new SplashManager(this);

		StartCoroutine(DestroyItselfWhenEmpty());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			ChangeRotation();
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveLeft();
		}

		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveRight();
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			clicked++;
			if (clicked == 1) clicktime = Time.time;

			if (clicked > 1 && Time.time - clicktime < clickdelay)
			{
				clicked = 0;
				clicktime = 0;
				MoveToFloor(15);
			}
			else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
		{	
			MoveDownAndFall();
		}
	}

	public void MoveToFloor(int floorLevel)
	{
		transform.position += new Vector3(0, -floorLevel, 0);

		if (!Grid.IsValidGridPos(this))
		{
			transform.position += new Vector3(0, floorLevel, 0);
			MoveToFloor(floorLevel - 1);
		}
		else
		{
			Grid.UpdateGrid(this);
		}
	}

	public void MoveLeft()
	{
		transform.position += new Vector3(-1, 0, 0);
			
		if (Grid.IsValidGridPos(this))
			Grid.UpdateGrid(this);
		else
			transform.position += new Vector3(1, 0, 0);
	}

	public void MoveRight()
	{
		transform.position += new Vector3(1, 0, 0);
		
		if (Grid.IsValidGridPos(this))
			Grid.UpdateGrid(this);
		else
			transform.position += new Vector3(-1, 0, 0);
	}

	public void MoveDownAndFall()
	{
		transform.position += new Vector3(0, -1, 0);
		
		if (Grid.IsValidGridPos(this))
		{
			Grid.UpdateGrid(this);
		}
		else
		{
			transform.position += new Vector3(0, 1, 0);
			
			Grid.FillEmptyGrid(this);
			SplashManager.CheckCollisions();
			FindObjectOfType<Spawner>().SpawnNext();

			enabled = false;
		}

		lastFall = Time.time;
	}

	private void ChangeRotation()
	{
		if (!Figure.HasSecondFloor) return;

		if (!Figure.RotatedBy180GameObject)
			Figure.RotatedBy180GameObject = true;
		else
			Figure.RotatedBy180GameObject = false;
		RotateElement();
	}

	private void RotateElement()
	{
		int val = Figure.upperColorNumber;
		Figure.upperColorNumber = Figure.lowerColorNumber;
		Figure.lowerColorNumber = val;

		for (int i = 0; i < transform.childCount; i += 2)
		{
			this.gameObject.transform.GetChild(i).transform.position     += new Vector3(0, Figure.RotatedBy180GameObject ? 1 : -1, 0);
			this.gameObject.transform.GetChild(i + 1).transform.position += new Vector3(0, Figure.RotatedBy180GameObject ? -1 : 1, 0);
		}
	}

	public void RemoveGroupScriptsFromObject()
	{
		enabled = false;
		gameObject.AddComponent<GroupTest>();
	}

	IEnumerator DestroyItselfWhenEmpty()
	{
		while (true)
		{
			if (gameObject.transform.childCount != 0)
			{
				yield return new WaitForSeconds(10f);
			}
			else
			{
				Destroy(gameObject);
				yield break;
			}
		}
	}
}