using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
	public class GameStep
	{
		public bool? Player { get; set; }
		public int Index { get; set; }
	}
}
