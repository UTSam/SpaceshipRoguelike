using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour
{
	public GameObject PlayerGO;
	public GameObject DungeonGO;


	private static Main instance = null;

	public static Main Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Main>();
			}
			return instance;
		}
	}

	private void Start()
	{

	}

	private void Update()
	{

	}


	public static bool HasInstance { get { return instance != null; } }
}