using SmartStore.Core;
using SmartStore.Core.Domain.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Boards
{
	public partial interface IBoardService
	{
		void Insert(Board board);
		void Update(Board board);
		void Delete(Board board);
		Board GetBoardById(int Id);
		CustomerPosition GetPositionById(int Id);
		IList<Board> GetAllBoards();
		List<CustomerPosition> GetAllPositionForEmailNotification();
		void UpdateCustomerPosition(CustomerPosition customerPosition);
		IList<MyMatrix> GetMyMatrixByPositionId(int positionid);
		IList<MyBoardMember> GetMyBoardMemberByPostionId(int positionid);
		IPagedList<CustomerPosition> GetAllPosition(int boardid, int customerid, bool? cycled, int pageIndex = 0, int pageSize = int.MaxValue);
		IList<MyBoardMember> GetTreeView(int positionid);

		IList<TreeBalance> GetTreeBalance(int CustomerId);
		CustomerPosition CheckTodayPositionExisit(int Id);
	}
}
