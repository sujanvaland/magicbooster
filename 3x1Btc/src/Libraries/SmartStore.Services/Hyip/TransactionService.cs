using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core;
using SmartStore.Core.Caching;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Security;
using SmartStore.Core.Domain.Stores;
using SmartStore.Data.Caching;

namespace SmartStore.Services.Hyip
{
	public partial class TransactionService : ITransactionService
	{
		private readonly IRepository<Transaction> _transactionRepository;
		private readonly IRequestCache _requestCache;
		private readonly IDbContext _dbContext;
		private const string TRANSACTION_PATTERN_KEY = "transaction.*";
		public TransactionService(IRepository<Transaction> transactionRepository,
			IRepository<StoreMapping> storeMappingRepository,
			IRepository<AclRecord> aclRepository,
			IWorkContext workContext,
			IDbContext dbContext,
			IRequestCache requestCache)
		{
			_transactionRepository = transactionRepository;
			_requestCache = requestCache;
			_dbContext = dbContext;
		}

		public void InsertTransaction(Transaction transaction)
		{
			Guard.NotNull(transaction, nameof(transaction));

			_transactionRepository.Insert(transaction);

			_requestCache.RemoveByPattern(TRANSACTION_PATTERN_KEY);
		}

		public void UpdateTransaction(Transaction transaction)
		{
			Guard.NotNull(transaction, nameof(transaction));

			_transactionRepository.Update(transaction);

			_requestCache.RemoveByPattern(TRANSACTION_PATTERN_KEY);
		}

		public void DeleteTransaction(Transaction transaction)
		{
			Guard.NotNull(transaction, nameof(transaction));

			transaction.Deleted = true;
			UpdateTransaction(transaction);
		}

		public Transaction GetTransactionById(int transactionid)
		{
			if (transactionid == 0)
				return null;

			return _transactionRepository.GetByIdCached(transactionid, "db.transaction.id-" + transactionid);
		}

		public Transaction GetTransactionByRefId(int RefId)
		{
			if (RefId == 0)
				return null;

			return _transactionRepository.Table.Where(x=>x.RefId == RefId && x.StatusId == 2 && x.TranscationTypeId == 7).FirstOrDefault();
		}

		public List<Transaction> GetTodaysWithdrawal(int customerid)
		{
			var transType = (int)TransactionType.Withdrawal;

			return _transactionRepository.Table.
				Where(x => x.TransactionDate.Year == DateTime.Now.Year
							&& x.TransactionDate.Month == DateTime.Now.Month
							&& x.TransactionDate.Day == DateTime.Now.Day
							&& x.TranscationTypeId == transType
							&& x.CustomerId == customerid && x.Deleted == false).ToList();
		}


		public IPagedList<Transaction> GetAllTransactions(int transactionid, int customerid, DateTime? startTime, DateTime? endTime, int[] ts, int[] tt, int pageIndex = 0, int pageSize = int.MaxValue)
		{
			var query = _transactionRepository.Table;

			if (ts != null && ts.Count() > 0)
				query = query.Where(x => ts.Contains(x.StatusId));

			if (tt != null && tt.Count() > 0)
			{
				if(tt[0] != 0)
				{
					query = query.Where(x => tt.Contains(x.TranscationTypeId));
				}				
			}				

			if (startTime.HasValue)
				query = query.Where(o => startTime.Value <= o.TransactionDate);
			if (startTime.HasValue)
				query = query.Where(o => startTime.Value >= o.TransactionDate);

			var transactions = (from t in query
								where (transactionid == 0 || t.Id == transactionid) &&
									 (customerid == 0 || t.CustomerId == customerid) && t.Deleted == false
								select t).OrderByDescending(x => x.CreatedOnUtc);
			// Paging
			return new PagedList<Transaction>(transactions, pageIndex, pageSize);
		}

		public IPagedList<Transaction> GetAllTransactions(int transactionid, int customerid, DateTime? startTime, DateTime? endTime, int[] processorids, int[] ts, int[] tt, int pageIndex = 0, int pageSize = int.MaxValue)
		{
			var query = _transactionRepository.Table;

			if (ts != null && ts.Count() > 0)
				query = query.Where(x => ts.Contains(x.StatusId));

			if (tt != null && tt.Count() > 0)
				query = query.Where(x => tt.Contains(x.TranscationTypeId));

			if (processorids != null && processorids.Count() > 0)
				query = query.Where(x => processorids.Contains(x.ProcessorId));

			if (startTime.HasValue)
				query = query.Where(o => startTime.Value <= o.TransactionDate);
			if (startTime.HasValue)
				query = query.Where(o => startTime.Value >= o.TransactionDate);


			var transactions = (from t in query
								where (transactionid == 0 || t.Id == transactionid) &&
									 (customerid == 0 || t.CustomerId == customerid) && t.Deleted == false
								select t).OrderByDescending(x => x.CreatedOnUtc);
			// Paging
			return new PagedList<Transaction>(transactions, pageIndex, pageSize);
		}

		public List<AllTransactionModel> GetAllTransactions(int customerid, int transactionid, DateTime? startDate, DateTime? endDate)
		{
			SqlParameter pCustomerId = new SqlParameter();
			pCustomerId.ParameterName = "customerid";
			pCustomerId.Value = customerid;
			pCustomerId.DbType = DbType.Int32;

			SqlParameter pTransactionid = new SqlParameter();
			pTransactionid.ParameterName = "transactionid";
			pTransactionid.Value = transactionid;
			pTransactionid.DbType = DbType.Int32;

			SqlParameter pStartDate = new SqlParameter();
			pStartDate.ParameterName = "startDate";
			if (startDate == null)
			{
				pStartDate.Value = DBNull.Value;
			}
			else
			{
				pStartDate.Value = startDate;
			}			
			pStartDate.DbType = DbType.DateTime;

			SqlParameter pEndDate = new SqlParameter();
			pEndDate.ParameterName = "endDate";
			if (endDate == null)
			{
				pEndDate.Value = DBNull.Value;
			}
			else
			{
				pEndDate.Value = endDate;
			}
			pEndDate.DbType = DbType.DateTime;

			var transactions = _dbContext.SqlQuery<AllTransactionModel>("Exec SpGetAllTransactions @customerid,@transactionid,@startDate,@endDate", pCustomerId, pTransactionid, pStartDate, pEndDate).ToList();

			// Paging
			return new List<AllTransactionModel>(transactions);
		}

		public PagedList<AllTransactionModel> GetAllTransactions(int customerid, int transactionid, DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = int.MaxValue)
		{
			SqlParameter pCustomerId = new SqlParameter();
			pCustomerId.ParameterName = "customerid";
			pCustomerId.Value = customerid;
			pCustomerId.DbType = DbType.Int32;

			SqlParameter pTransactionid = new SqlParameter();
			pTransactionid.ParameterName = "transactionid";
			pTransactionid.Value = transactionid;
			pTransactionid.DbType = DbType.Int32;

			SqlParameter pStartDate = new SqlParameter();
			pStartDate.ParameterName = "startDate";
			if (startDate == null)
			{
				pStartDate.Value = DBNull.Value;
			}
			else
			{
				pStartDate.Value = startDate;
			}
			pStartDate.DbType = DbType.DateTime;

			SqlParameter pEndDate = new SqlParameter();
			pEndDate.ParameterName = "endDate";
			if (endDate == null)
			{
				pEndDate.Value = DBNull.Value;
			}
			else
			{
				pEndDate.Value = endDate;
			}
			pEndDate.DbType = DbType.DateTime;

			var transactions = _dbContext.SqlQuery<AllTransactionModel>("Exec SpGetAllTransactions @customerid,@transactionid,@startDate,@endDate", pCustomerId, pTransactionid, pStartDate, pEndDate).ToList();

			// Paging
			return new PagedList<AllTransactionModel>(transactions, pageIndex, pageSize);
		}

		public List<Transaction> GetCoinRequest(int customerid)
		{
			var transType = (int)TransactionType.RequestCoin;

			return _transactionRepository.Table.
				Where(x => x.TranscationTypeId == transType
							&& x.CustomerId == customerid && x.Deleted == false).ToList();
		}

		public List<Transaction> GetTransactionByCustomerId(int CustomerId, int? TransactionTypeId)
		{
			if (CustomerId == 0)
				return null;

			var Transaction = _transactionRepository.Table.
				Where(x => x.CustomerId == CustomerId && x.StatusId == 2 && x.Deleted == false).ToList();

			if(TransactionTypeId != null)
			{
				Transaction = Transaction.Where(x => x.TranscationTypeId == TransactionTypeId).ToList();
			}
			return Transaction;
		}

		public partial class AllTransactionModel
		{
			public int CustomerId { get; set; }
			public float Amount { get; set; }
			public int TranscationTypeId { get; set; }
			public string StokistName { get; set; }
			public string FinalAmountRaw { get; set; }
			public DateTime TransactionDate { get; set; }
			public string TransactionDateString { get; set; }
			public string SenderUserName { get; set; }
			public string ReceiverUserName { get; set; }
		}
	}
}
