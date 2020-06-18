using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Services;

namespace TicTacToe.Models
{
	public class MainHub : Hub
	{
		ILobbies lobbiesService;
		public MainHub(ILobbies lobbiesService)
		{
			this.lobbiesService = lobbiesService;
		}

		public async Task AddToGameGroup(string id, string connectionId)
		{
			await Groups.AddToGroupAsync(connectionId, id);
			var lobby = lobbiesService.GetLobbyById(id);
			lobby.SetReady();
			await Clients.Client(connectionId).SendAsync("SetPlayerTurn", lobby.Turn);
			lobby.Turn = !lobby.Turn;
			if(lobby.IsReady)
			{
				await Clients.Group(id).SendAsync("ReadyNotify");
			}
		}

		public async Task DoStep(string index, string id, string connectionId, bool turn)
		{
			if (lobbiesService.GetLobbyById(id).Turn == turn)
			{
				bool? winner;
				await Groups.AddToGroupAsync(connectionId, id);
				lobbiesService.AddStepToLobby(id, Int32.Parse(index));
				await Clients.Group(id).SendAsync("UpdateField", lobbiesService.GetLobbyById(id));
				if (IsFinished(lobbiesService.GetLobbyById(id).GameSteps, out winner))
				{
					if(winner != null)
						await Clients.Group(id).SendAsync("WinningPanel", winner);
					else
						await Clients.Group(id).SendAsync("DrawPanel");
				}
			}
		}

		private bool IsFinished(List<GameStep> gameSteps, out bool? winner)
		{
			bool isFinished = false;
			winner = null;


			List<int> field = new List<int>();
			for (int i = 0; i < 9; i++)
				field.Add(-1);

			foreach (var step in gameSteps)
				field[step.Index] = step.Player == false ? 0 : 1;

			for(int i = 0; i < 3; i++)
			{
				if((field[i*3] == field[i*3 + 1] && field[i*3] == field[i*3 + 2]) && field[i*3] != -1)
				{
					winner = field[i * 3] == 0 ? false : true;
					isFinished = true;
					break;
				}
				else if(field[i] == field[i + 3] && field[i] == field[i + 6] && field[i] != -1)
				{
					winner = field[i] == 0 ? false : true;
					isFinished = true;
					break;
				}
			}

			if(((field[0] == field[4] && field[4] == field[8]) || (field[2] == field[4] && field[4] == field[6])) && field[4] != -1)
			{
				isFinished = true;
				winner = field[4] == 0 ? false : true;
			}
			if (gameSteps.Count == 9)
				return true;

			return isFinished;
		}
	}
}
