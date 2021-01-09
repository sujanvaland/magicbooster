using SmartStore.Core;
using SmartStore.Services.Customers;
using SmartStore.Services.Games;
using SmartStore.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Web.Controllers
{
    public class LotteryGamesController : PublicControllerBase
	{
		private readonly IWorkContext _workContext;
		private readonly IGameService _gameService;
		private readonly ICustomerService _customerService;
		public LotteryGamesController(IWorkContext workContext, 
			IGameService gameService,
			ICustomerService customerService)
		{
			_workContext = workContext;
			_gameService = gameService;
			_customerService = customerService;
		}
		// GET: LotteryGames
		public ActionResult Lucky10()
        {
			var id = _workContext.CurrentCustomer.Id;
			var betRound = _gameService.GetNextRound();
			if(betRound.Count > 0)
			{
				var NextRound = betRound.Where(x => x.BetResult == null).FirstOrDefault();
				ViewBag.NextRound = NextRound.Id;
				DateTime now = DateTime.Now;
				TimeSpan difference = Convert.ToDateTime(NextRound.ResultDateTime).Subtract(now);
				ViewBag.SecondsLeft =  difference.TotalSeconds;
				ViewBag.ResultDateTime = NextRound.ResultDateTime;
			}
			ViewBag.AvailableBalance = _customerService.GetAvailableBalance(id);
			return View(betRound);
        }

		[HttpPost]
		public ActionResult PlaceBets(string selectedNos,float betAmount)
		{
			//NotifyError("This function is disabled currently");
			//return Json(new { success = false, error = "" });

			var id = _workContext.CurrentCustomer.Id;
			if(selectedNos == "")
			{
				NotifyError(T("SelectNumber"));
				return Json(new { success = false, error = "" });
			}
			if (betAmount <= 0)
			{
				NotifyError(T("EnterValidBetAmount"));
				return Json(new { success = false, error = "" });
			}
			int SelectedNoCount = selectedNos.ToIntArray().Length;
			if (_customerService.GetAvailableBalance(id)> SelectedNoCount * betAmount)
			{
				try
				{
					var array = selectedNos.ToIntArray();
					foreach(var n in array)
					{
						_gameService.PlaceBets(n.ToString(), _workContext.CurrentCustomer.Id, betAmount);
					}
					return Json(new { success = true, error = "" });
				}
				catch (Exception ex)
				{
					return Json(new { success = false, error = ex.ToString() });
				}
			}
			else
			{
				NotifyError(T("InsufficentBalance"));
				return Json(new { success = false, error = "" });
			}
		}
    }
}