using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager _instance;
	private int totalCount = 0;
	private int topPlayerCount;
	public Text playerCount;
	public Text topPlayerCountText;
	public Image nextFigureImage;

	public static UIManager Instance
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

	public void UpdatePlayerCount(int count)
	{
		totalCount += count;
		playerCount.text = "" + totalCount;
	}

	public int GetTotalCount()
	{
		return totalCount;
	}

	public int GetTopCount()
	{
		return topPlayerCount;
	}

	public void SetTopPlayerCount(int count)
	{
		topPlayerCount = count;
		topPlayerCountText.text = "" + count;
	}

	public void SetNewParent(GameObject newChild)
	{
		newChild.GetComponent<Group>().RemoveGroupScriptsFromObject();
		newChild.transform.SetParent(GameObject.FindGameObjectWithTag("NextFigure").transform);
		newChild.transform.localPosition = new Vector3(-1518f, -610f);//(-433.5f, -192f, 0); (-269.5f, -362f);
	}
}
