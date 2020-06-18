using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
	public class GameLobby
	{
		public string Id { get; set; }
		public List<GameStep> GameSteps { get; set; }
		public bool Turn { get; set; }
		public short Readiness { get; private set; }
		public GameLobby()
		{
			GameSteps = new List<GameStep>();
			Turn = false;
			Readiness = 0;
		}

		public void SetReady()
		{
			Readiness += (short)(Readiness < 2 ? 1 : 0);
		}

		public bool IsReady
		{
			get => Readiness == 2; 
		}
	}
}
