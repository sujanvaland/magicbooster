using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Hyip
{
	public enum TransactionType
	{
		All,
		Funding,
		Purchase,
		ROI,
		Commission,
		Withdrawal,
		DirectBonus,
		CyclerBonus,
		UnilevelBonus,
		PoolBonus,
		SharePurchase,
		Repurchase,
		RepurchaseROI,
		BetEarning,
		Transfer,
		TransferCoin,
		EarnedCoin,
		PurchaseByCoin,
		RequestCoin
	}
}
