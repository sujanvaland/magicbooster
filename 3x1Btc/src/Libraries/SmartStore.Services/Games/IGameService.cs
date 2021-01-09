using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Games
{
	public partial interface IGameService
	{
		List<BetRound> GetNextRound();
		void PlaceBets(string numbers, int CustomerId, float BetAmount);
	}
}
