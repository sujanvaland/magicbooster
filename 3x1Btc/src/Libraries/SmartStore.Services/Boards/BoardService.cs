using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Boards;

namespace SmartStore.Services.Boards
{
	public partial class BoardService : IBoardService
	{
		private readonly IRepository<Board> _boardRepository;
		private readonly IRepository<CustomerPosition> _customerPositionRepository;
		private readonly IDataProvider _dataProvider;
		private readonly IDbContext _dbContext;
		public BoardService(IRepository<Board> boardRepository,
			IRepository<CustomerPosition> customerPositionRepository,
			IDataProvider dataProvider,
			IDbContext dbContext)
		{
			_boardRepository = boardRepository;
			_dataProvider = dataProvider;
			_dbContext = dbContext;
			_customerPositionRepository = customerPositionRepository;
		}

		public void Insert(Board board)
		{
			_boardRepository.Insert(board);
		}

		public void Update(Board board)
		{
			_boardRepository.Update(board);
		}

		public void Delete(Board board)
		{
			_boardRepository.Delete(board);
		}
		
		public CustomerPosition GetPositionById(int Id)
		{
			return _customerPositionRepository.Table.Where(x => x.Id == Id).FirstOrDefault();
		}
		public Board GetBoardById(int Id)
		{
			return _boardRepository.Table.Where(x => x.Id == Id).FirstOrDefault();
		}

		public CustomerPosition CheckTodayPositionExisit(int Id)
		{
			return _customerPositionRepository.Table.Where(x => x.CustomerId == Id && x.BoardId == 1
			&& x.PurchaseDate.Year == DateTime.Now.Year
							&& x.PurchaseDate.Month == DateTime.Now.Month
							&& x.PurchaseDate.Day == DateTime.Now.Day
			).FirstOrDefault();
		}
		public IList<Board> GetAllBoards()
		{
			return _boardRepository.Table.OrderBy(x=>x.Price).ToList();
		}

		public IPagedList<CustomerPosition> GetAllPosition(int boardid, int customerid, bool? cycled, int pageIndex = 0, int pageSize = int.MaxValue)
		{
			var query = _customerPositionRepository.Table;
			if(boardid > 0)
			{
				query = query.Where(x => x.BoardId == boardid);
			}
			if(customerid > 0)
			{
				query = query.Where(x => x.CustomerId == customerid);
			}
			if (cycled == true)
				query = query.Where(x => x.IsCycled == cycled);

			query = query.OrderByDescending(x => x.Id);
			return new PagedList<CustomerPosition>(query, pageIndex, pageSize);
		}

		public List<CustomerPosition> GetAllPositionForEmailNotification()
		{
			var query = _customerPositionRepository.Table.Where(x=>x.EmailSentOnCycle == false && x.IsCycled == true);
			return query.ToList();
		}

		public void UpdateCustomerPosition(CustomerPosition customerPosition)
		{
			_customerPositionRepository.Update(customerPosition);
		}
		public IList<PurchasedPosition> SaveCustomerPosition(int customerid,int boardid,int NoOfPosition)
		{
			SqlParameter pCustomerId = new SqlParameter();
			pCustomerId.ParameterName = "memberid";
			pCustomerId.Value = customerid;
			pCustomerId.DbType = DbType.Int32;

			SqlParameter pBoardId = new SqlParameter();
			pCustomerId.ParameterName = "boardid";
			pCustomerId.Value = boardid;
			pCustomerId.DbType = DbType.Int32;

			SqlParameter pNoOfPosition = new SqlParameter();
			pCustomerId.ParameterName = "NoOfPosition";
			pCustomerId.Value = customerid;
			pCustomerId.DbType = DbType.Int32;

			var purchasedpostion = _dbContext.SqlQuery<PurchasedPosition>("Exec SpGetLevelMembers @customerid", pCustomerId, pBoardId, pNoOfPosition).ToList();
			return purchasedpostion;
		}

		public IList<MyMatrix> GetMyMatrixByPositionId(int positionid)
		{
			SqlParameter pPositionId = new SqlParameter();
			pPositionId.ParameterName = "positionid";
			pPositionId.Value = positionid;
			pPositionId.DbType = DbType.Int32;


			var myMatrix = _dbContext.SqlQuery<MyMatrix>("Exec SpGetPositionFilled @positionid", pPositionId).ToList();
			return myMatrix;
		}

		public IList<MyBoardMember> GetMyBoardMemberByPostionId(int positionid)
		{
			SqlParameter pPositionId = new SqlParameter();
			pPositionId.ParameterName = "positionid";
			pPositionId.Value = positionid;
			pPositionId.DbType = DbType.Int32;


			var myMatrix = _dbContext.SqlQuery<MyBoardMember>("Exec SpGetMemberBoardMember @positionid", pPositionId).ToList();
			return myMatrix;
		}

		public IList<MyBoardMember> GetTreeView(int positionid)
		{
			SqlParameter pPositionId = new SqlParameter();
			pPositionId.ParameterName = "customerid";
			pPositionId.Value = positionid;
			pPositionId.DbType = DbType.Int32;


			var myMatrix = _dbContext.SqlQuery<MyBoardMember>("Exec SpMyGetBoardView @customerid", pPositionId).ToList();
			return myMatrix;
		}

		public IList<TreeBalance> GetTreeBalance(int customerid)
		{
			SqlParameter pPositionId = new SqlParameter();
			pPositionId.ParameterName = "customerid";
			pPositionId.Value = customerid;
			pPositionId.DbType = DbType.Int32;


			var myMatrix = _dbContext.SqlQuery<TreeBalance>("Exec SpGetTreeBalance @customerid", pPositionId).ToList();
			return myMatrix;
		}
	}
	public class MyMatrix
	{
		public int PositionFilled { get; set; }
		public int PositionRemaining { get; set; }
	}

	public class TreeBalance
	{
		public int TodaysLeftPoint { get; set; }
		public int TodaysRightPoint { get; set; }
		public int TotalLeftPoint { get; set; }
		public int TotalRightPoint { get; set; }
		public int PointPaidTillDate { get; set; }
		public int Balance { get; set; }
	}
	public class MyBoardMember
	{
		public int LevelId { get; set; }
		public string PurchaseDate { get; set; }
		public string Email { get; set; }
		public int PositionId { get; set; }
		public int CustomerId { get; set; }
		public string BoardName { get; set; }
		public string Name { get; set; }
		public int PlacedUnderCustomerId { get; set; }
		public int PlacedUnderPositionId { get; set; }
		public int parentid { get; set; }
		public bool IsCycled { get; set; }
		public int HasLeft { get; set; }
		public int HasRight { get; set; }
		public string Position { get; set; }
	}
	public class PurchasedPosition
	{
		public int PositionId { get; set; }
		public int BoardId { get; set; }
		public int CustomerId { get; set; }
	}
}
