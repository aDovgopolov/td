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
		MoveManager.Instance.SetGroup(this);
		width = (float)Screen.width / 2.0f;
		height = (float)Screen.height / 2.0f;

		// Position used for the cube.
		position = transform.position;//new Vector3(0.0f, 0.0f, 0.0f);

		Debug.Log($"{this} =   {Grid.IsValidGridPos(this)}" );
		//if (!Grid.IsValidGridPos(this))
		//{
		//	Destroy(gameObject);
		//}

		if (GetComponent<GroupTest>() != null)
			Figure =  GetComponent<GroupTest>().GiveOwnFigure();
		else
			Figure = FindObjectOfType<Spawner>().PrepareFigure(this);

		SplashManager = new SplashManager(this);

		StartCoroutine(DestroyItselfWhenEmpty());

		//startTime = Time.time;
		//journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		lastPos = transform.localPosition.x;
	}

	private Vector3 position;
	private float width;
	private float height;
	private Vector3 pos;

	// follow finger part
	//public Transform startMarker;
	//public Transform endMarker;
	//public float speed = 1.0F;
	//private float startTime;
	//private float journeyLength;
	private float lastPos;

	void Update()
	{	

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Moved)
			{
				//Vector3 touchPosition1 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				//Debug.Log($"{transform.position.x}  =  {touchPosition1.x}");
				////Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
				////transform.position = touchPosition;
				//if(lastPos < touchPosition1.x)
				//{
				//	MoveLeft();
				//}
				//else
				//{
				//	MoveRight();
				//}
			}

			//if (Input.touchCount == 1)
			//{
			//	Debug.Log("Input.touchCount ==1 ");
			//	ChangeRotation();
			//}
			//if (Input.touchCount == 2)
			//{
			//	Debug.Log("Input.touchCount ==2 ");
			//	MoveToFloor(15);
			//}
		}




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

		lastPos = transform.position.x;
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

	public void ChangeRotation()
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