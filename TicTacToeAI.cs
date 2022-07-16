using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TicTacToeState{none, cross, circle}

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
}

public class TicTacToeAI : MonoBehaviour
{

	int _aiLevel;

	TicTacToeState[,] boardState;

	[SerializeField]
	private bool _isPlayerTurn;

	[SerializeField]
	private int _gridSize = 3;
	private int[,] pos = new int[3, 3]; // Create a 2d array to store players choices

	[SerializeField]
	private TicTacToeState playerState = TicTacToeState.circle;
	TicTacToeState aiState = TicTacToeState.cross;

	[SerializeField]
	private GameObject _xPrefab;

	[SerializeField]
	private GameObject _oPrefab;

	public UnityEvent onGameStarted;

	//Call This event with the player number to denote the winner
	public WinnerEvent onPlayerWin;

	ClickTrigger[,] _triggers;
	
	private void Awake()
	{
		if(onPlayerWin == null){
			onPlayerWin = new WinnerEvent();
		}
	}

	public void StartAI(int AILevel){
		Debug.Log("startAI AILevel: " + AILevel);
		_aiLevel = AILevel;
		StartGame();
	}

	public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
	{
		_triggers[myCoordX, myCoordY] = clickTrigger;
	}

	private void StartGame()
	{
		_triggers = new ClickTrigger[3,3];
		onGameStarted.Invoke();
	}

	public void PlayerSelects(int coordX, int coordY){
		Debug.Log("PlayerSelects: " + playerState); // circle
		SetVisual(coordX, coordY, playerState);
		// stores player's choice as 1
		pos[coordX, coordY] = 1;
		int result = CheckWin();
		if (result == 0)
		{
			Debug.Log("continue");
			int[] coor = getAiSelect();
			AiSelects(coor[0], coor[1]);
		} else
        {
			Debug.Log("game end: " + result);
			onPlayerWin.Invoke(result);
        }
	}


	public void AiSelects(int coordX, int coordY){
		Debug.Log("AiSelects: " + aiState); // cross
		SetVisual(coordX, coordY, aiState);
		// stores computer's choice as 2
		pos[coordX, coordY] = 2;
		int result = CheckWin();
		if (result == 2)
        {
			Debug.Log("game end: " + result);
			onPlayerWin.Invoke(result);
		}
	}

	private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
	{
		Instantiate(
			targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
			_triggers[coordX, coordY].transform.position,
			Quaternion.identity
		);
	}

	// Return:
	// 0: not game over, continue
	// 1: player wins
	// 2: ai wins
	// -1: tie
	private int CheckWin()
	{
		// Horzontal Winning Condtion
		//Winning Condition For First Row
		if (pos[0, 0] != 0 && pos[0, 0] == pos[0, 1] && pos[0, 1] == pos[0, 2])
		{
			return pos[0, 0];
		}
		//Winning Condition For Second Row
		else if (pos[1 ,0] != 0 && pos[1 ,0] == pos[1 ,1] && pos[1 ,1] == pos[1 ,2])
		{
			return pos[1 ,0];
		}
		//Winning Condition For Third Row
		else if (pos[2 ,0] != 0 && pos[2 ,0] == pos[2 ,1] && pos[2, 1] == pos[2, 2])
		{
			return pos[2, 0];
		}
		// Vertical Winning Condtion
		//Winning Condition For First Column
		else if (pos[0, 0] != 0 && pos[0, 0] == pos[1, 0] && pos[1, 0] == pos[2, 0])
		{
			return pos[0, 0];
		}
		//Winning Condition For Second Column
		else if (pos[0, 1] != 0 && pos[0, 1] == pos[1, 1] && pos[1, 1] == pos[2, 1])
		{
			return pos[0, 1];
		}
		//Winning Condition For Third Column
		else if (pos[0, 2] != 0 && pos[0, 2] == pos[1, 2] && pos[1, 2] == pos[2, 2])
		{
			return pos[0, 2];
		}
		// Diagonal Winning Condition
		else if (pos[0, 0] != 0 && pos[0, 0] == pos[1, 1] && pos[1, 1] == pos[2, 2])
		{
			return pos[0, 0];
		}
		else if (pos[0, 2] != 0 && pos[0, 2] == pos[1, 1] && pos[1, 1] == pos[2, 0])
		{
			return pos[0, 2];
		}
		// Checking For Draw
		// If all the cells or values filled with X or O then any player has won the match
		else if (
			pos[0, 0] != 0 &&
			pos[0, 1] != 0 &&
			pos[0, 2] != 0 &&
			pos[1, 0] != 0 &&
			pos[1, 1] != 0 &&
			pos[1, 2] != 0 &&
			pos[2, 0] != 0 &&
			pos[2, 1] != 0 &&
			pos[2, 2] != 0 )
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}

	private int[] getAiSelect()
	{
		int[] result;
		// Horzontal Winning Condtion
		//Winning Condition For First Row
		if (pos[0, 0] == pos[0, 1] && pos[0, 0] == 1 && pos[0, 2] == 0)
		{
			result = new int[2] { 0, 2 };
			return result;
		} else if (pos[0, 0] == pos[0, 2] && pos[0, 0] == 1 && pos[0, 1] == 0)
		{
			result = new int[2] { 0, 1 };
			return result;
		} else if (pos[0, 1] == pos[0, 2] && pos[0, 1] == 1 && pos[0, 0] == 0)
		{
			result = new int[2] { 0, 0 };
			return result;
		}
        ////Winning Condition For Second Row
        else if (pos[1, 0] == pos[1, 1] && pos[1, 0] == 1 && pos[1, 2] == 0)
        {
			result = new int[2] { 1, 2 };
			return result;
		}
		else if (pos[1, 0] == pos[1, 2] && pos[1, 0] == 1 && pos[1, 1] == 0)
		{
			result = new int[2] { 1, 1 };
			return result;
		}
		else if (pos[1, 1] == pos[1, 2] && pos[1, 1] == 1 && pos[1, 0] == 0)
		{
			result = new int[2] { 1, 0 };
			return result;
		}
		////Winning Condition For Third Row
		else if (pos[2, 0] == pos[2, 1] && pos[2, 0] == 1 && pos[2, 2] == 0)
		{
			result = new int[2] { 2, 2 };
			return result;
		}
		else if (pos[2, 0] == pos[2, 2] && pos[2, 0] == 1 && pos[2, 1] == 0)
		{
			result = new int[2] { 2, 1 };
			return result;
		}
		else if (pos[2, 1] == pos[2, 2] && pos[2, 1] == 1 && pos[2, 0] == 0)
		{
			result = new int[2] { 2, 0 };
			return result;
		}
		//// Vertical Winning Condtion
		////Winning Condition For First Column
		else if (pos[0, 0] == pos[1, 0] && pos[0, 0] == 1 && pos[2, 0] == 0)
		{
			result = new int[2] { 2, 0 };
			return result;
		}
		else if (pos[0, 0] == pos[2, 0] && pos[0, 0] == 1 && pos[1, 0] == 0)
		{
			result = new int[2] { 1, 0 };
			return result;
		}
		else if (pos[1, 0] == pos[2, 0] && pos[1, 0] == 1 && pos[0, 0] == 0)
		{
			result = new int[2] { 0, 0 };
			return result;
		}
		////Winning Condition For Second Column
		else if (pos[0, 1] == pos[1, 1] && pos[0, 1] == 1 && pos[2, 1] == 0)
		{
			result = new int[2] { 2, 1 };
			return result;
		}
		else if (pos[0, 1] == pos[2, 1] && pos[0, 1] == 1 && pos[1, 1] == 0)
		{
			result = new int[2] { 1, 1 };
			return result;
		}
		else if (pos[1, 1] == pos[2, 1] && pos[1, 1] == 1 && pos[0, 1] == 0)
		{
			result = new int[2] { 0, 1 };
			return result;
		}
		////Winning Condition For Third Column
		else if (pos[0, 2] == pos[1, 2] && pos[0, 2] == 1 && pos[2, 2] == 0)
		{
			result = new int[2] { 2, 2 };
			return result;
		}
		else if (pos[0, 2] == pos[2, 2] && pos[0, 2] == 1 && pos[1, 2] == 0)
		{
			result = new int[2] { 1, 2 };
			return result;
		}
		else if (pos[1, 2] == pos[2, 2] && pos[1, 2] == 1 && pos[0, 2] == 0)
		{
			result = new int[2] { 0, 2 };
			return result;
		}
		//// Diagonal Winning Condition
		else if (pos[0, 0] == pos[1, 1] && pos[0, 0] == 1 && pos[2, 2] == 0)
		{
			result = new int[2] { 2, 2 };
			return result;
		}
		else if (pos[0, 0] == pos[2, 2] && pos[0, 0] == 1 && pos[1, 1] == 0)
		{
			result = new int[2] { 1, 1 };
			return result;
		}
		else if (pos[1, 1] == pos[2, 2] && pos[1, 1] == 1 && pos[0, 0] == 0)
		{
			result = new int[2] { 0, 0 };
			return result;
		}
		else if (pos[2, 0] == pos[1, 1] && pos[2, 0] == 1 && pos[0, 2] == 0)
		{
			result = new int[2] { 0, 2 };
			return result;
		}
		else if (pos[2, 0] == pos[0, 2] && pos[2, 0] == 1 && pos[1, 1] == 0)
		{
			result = new int[2] { 1, 1 };
			return result;
		}
		else if (pos[1, 1] == pos[0, 2] && pos[1, 1] == 1 && pos[2, 0] == 0)
		{
			result = new int[2] { 2, 0 };
			return result;
		}
		// Nothing to block. Choose a random empty space.
		else {
			for (int i = 0; i <= 2; i++)
            {
				for (int j = 0; j <=2; j++)
                {
					if (pos[i, j] == 0)
                    {
						result = new int[2] { i, j };
						return result;
                    }
                }
            }
			return new int[2] { -1, -1 }; // shouldn't exist.
		}
	}
}
