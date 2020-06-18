using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
	public class HomeController : Controller
	{
		private ILobbies lobbiesService;
		private IHubContext<MainHub> hubContext;

		public HomeController(ILobbies lobbiesService, IHubContext<MainHub> hubContext)
		{
			this.lobbiesService = lobbiesService;
			this.hubContext = hubContext;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View(lobbiesService.GetGameLobbies());
		}

		[HttpPost]
		public async Task Create(string connectionId)
		{
			string id = lobbiesService.CreateLobby();
			await hubContext.Clients.AllExcept(connectionId).SendAsync("AddLobby", lobbiesService.GetGameLobbies());
			await hubContext.Clients.Client(connectionId).SendAsync("RedirectToLobby", id);
		}


		public IActionResult Game(string Id)
		{
			return View(lobbiesService.GetLobbyById(Id));
		}

		
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
