using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
	public interface ILobbies
	{
		public List<GameLobby> GetGameLobbies();
		public string CreateLobby();
		public GameLobby GetLobbyById(string id);
		public void AddStepToLobby(string Id, int index);
	}
}
