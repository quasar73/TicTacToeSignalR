using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
	public class LobbiesService : ILobbies
	{
		private List<GameLobby> gameLobbies;
		private int gameCounting;
		public LobbiesService()
		{
			gameLobbies = new List<GameLobby>();
			gameCounting = 0;
		}

		public string CreateLobby()
		{
			GameLobby game = new GameLobby()
			{
				Id = (++gameCounting).ToString()
			};
			gameLobbies.Add(game);
			return game.Id;
		}		

		public List<GameLobby> GetGameLobbies()
		{
			return gameLobbies;
		}

		public GameLobby GetLobbyById(string id)
		{
			return gameLobbies.FirstOrDefault(g => g.Id == id);
		}

		public void AddStepToLobby(string Id, int index)
		{
			var lobby = GetLobbyById(Id);
			var matches = lobby.GameSteps.Where(s => s.Index == index);
			if(matches.Count() == 0)
			{
				lobby.GameSteps.Add(new GameStep() { Index = index, Player = lobby.Turn });
				lobby.Turn = !lobby.Turn;
			}
		}
	}
}
