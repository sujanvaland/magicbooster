using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartStore.Admin.Models.Board;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Services.Boards;
using SmartStore.Services.Customers;
using SmartStore.Services.Hyip;
using SmartStore.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    public class BoardController : AdminControllerBase
	{
		private readonly IBoardService _boardService;
		private readonly IWorkContext _workContext;
		private readonly AdminAreaSettings _adminAreaSettings;
		public BoardController(IBoardService boardService,IWorkContext workContext, AdminAreaSettings adminAreaSettings)
		{
			_boardService = boardService;
			_workContext = workContext;
			_adminAreaSettings = adminAreaSettings;
		}
        // GET: Board
        public ActionResult ViewBoard()
        {
			BoardModel model = new BoardModel();
			var boards = _boardService.GetAllBoards();
			foreach(var b in boards)
			{
				model.AvailableBoard.Add(new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
			}
			model.GridPageSize = _adminAreaSettings.GridPageSize;
			return View(model);
        }
		[HttpPost]
		public ActionResult GetTreeView(int PositionId)
		{
			var boards = _boardService.GetTreeView(PositionId);
			//return View();
			return new JsonResult
			{
				Data = boards
			};
		}

		public ActionResult TreeView(int PositionId)
		{
			ViewBag.PositionId = PositionId;
			return View();
			//return new JsonResult
			//{
			//	Data = boards
			//};
		}

		public ActionResult ViewBoardMember(int positionid)
		{
			ViewBag.PositionId = positionid;
			return View(positionid);
		}

		[GridAction(EnableCustomBinding = true)]
		public ActionResult ListCyclerBonus(GridCommand command, BoardModel model)
		{
			var gridModel = new GridModel<BoardModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;

			int[] BoardIds = model.BoardIds.ToIntArray();
			bool IsCycled = model.IsCycled;
			int customerid = 0;
			customerid = _workContext.CurrentCustomer.Id;
			if (_workContext.CurrentCustomer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
			}

			var custPositions = _boardService.GetAllPosition(model.BoardId, customerid, model.IsCycled, command.Page - 1, command.PageSize);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = custPositions.Select(x =>
			{
				var myMatrix = _boardService.GetMyMatrixByPositionId(x.Id).FirstOrDefault();
				var transModel = new BoardModel
				{
					Id = x.Id,
					BoardId = x.BoardId,
					BoardName = _boardService.GetBoardById(x.BoardId).Name,
					CustomerName = x.Customer.GetFullName(),
					PurchaseDate = x.PurchaseDate,
					NoOfPositionFilled = myMatrix.PositionFilled,
					NoOfPositionRemaining = myMatrix.PositionRemaining,
					IsCycledString = (x.IsCycled) ? "Cycled" : "Active",
					PlacedUnderPositionId = x.PlacedUnderPositionId,
				};
				return transModel;
			});

			gridModel.Total = custPositions.TotalCount;

			return new JsonResult
			{
				Data = gridModel
			};
		}

		[GridAction(EnableCustomBinding = true)]
		public ActionResult ListBoardMember(GridCommand command, int positionid)
		{
			var gridModel = new GridModel<MyBoardMember>();
			
			var custPositions = _boardService.GetMyBoardMemberByPostionId(positionid);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = custPositions.Select(x =>
			{
				var transModel = new MyBoardMember
				{
					LevelId = x.LevelId,
					Email = x.Email,
					PlacedUnderCustomerId = x.PlacedUnderCustomerId,
					PlacedUnderPositionId = x.PlacedUnderPositionId,
					CustomerId = x.CustomerId,
					PositionId = x.PositionId
				};
				return transModel;
			});

			gridModel.Total = custPositions.Count;

			return new JsonResult
			{
				Data = gridModel
			};
		}
	}
}