using SmartStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Games
{
	public class GameService : IGameService
	{
		private readonly IDbContext _dbContext;
		public GameService(IDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<BetRound> GetNextRound()
		{
			var BetRound = _dbContext.SqlQuery<BetRound>("select * from BetRound").ToList();
			return BetRound;
		}
		public void PlaceBets(string numbers, int CustomerId, float BetAmount)
		{
			//foreach(var n in numbers)
			//{
				SqlParameter pBetNo = new SqlParameter();
				pBetNo.ParameterName = "@BetNo";
				pBetNo.Value = numbers;
				pBetNo.DbType = DbType.Int32;

				SqlParameter pCustomerId = new SqlParameter();
				pCustomerId.ParameterName = "@CustomerId";
				pCustomerId.Value = CustomerId;
				pCustomerId.DbType = DbType.Int32;

				SqlParameter pBetAmount = new SqlParameter();
				pBetAmount.ParameterName = "@BetAmount";
				pBetAmount.Value = BetAmount;
				pBetAmount.DbType = DbType.Int64;

				var custTraffic = _dbContext.SqlQuery<List<int>>("Exec SpPlaceBets @BetNo,@CustomerId,@BetAmount", pBetNo, pCustomerId, pBetAmount).ToList();
			//}
			
		}
	}

	public class BetRound
	{
		public	int Id { get; set; }
		public int? BetResult { get; set; }
		public DateTime? ResultDateTime { get; set; }
	}
}
