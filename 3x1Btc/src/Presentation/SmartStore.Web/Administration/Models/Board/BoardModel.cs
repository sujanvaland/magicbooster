using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.Board
{
	public class BoardModel
	{
		public BoardModel()
		{
			AvailableBoard = new List<SelectListItem>();
		}
		public int Id { get; set; }
		public int BoardId { get; set; }
		public int CustomerId { get; set; }
		public string CustomerName { get; set; }
		public int PlacedUnderPositionId { get; set; }
		public int PlacedUnderCustomerId { get; set; }
		public string PlacedUnderCustomerName { get; set; }
		public string BoardName { get; set; }
		public string BoardIds { get; set; }
		public decimal Price { get; set; }
		public bool IsCycled { get; set; }
		public string IsCycledString { get; set; }
		public DateTime CycledDate { get; set; }
		public DateTime PurchaseDate { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool ShowmyPosition { get; set; }
		public int GridPageSize { get; set; }
		public int NoOfPositionFilled { get; set; }
		public int NoOfPositionRemaining { get; set; }
		public List<SelectListItem> AvailableBoard { get; set; }
	}
}