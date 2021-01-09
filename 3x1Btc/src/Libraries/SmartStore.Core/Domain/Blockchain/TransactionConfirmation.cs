using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Core.Domain.Blockchain
{
	public class TransactionConfirmation
	{
		public class Data
		{
			public List<Tx> txs { get; set; }
			public string txid { get; set; }
			public List<Output> outputs { get; set; }
			public string network { get; set; }
			public string address { get; set; }
			public bool is_valid { get; set; }
			public int confirmations { get; set; }
			public bool is_confirmed { get; set; }
			public object nodes { get; set; }
			public object is_double_spend { get; set; }
		}
		public class Tx
		{
			public string txid { get; set; }
			public int output_no { get; set; }
			public string script_asm { get; set; }
			public string script_hex { get; set; }
			public string value { get; set; }
			public int confirmations { get; set; }
			public int time { get; set; }
		}
		public class RootObject
		{
			public string status { get; set; }
			public Data data { get; set; }
		}
		public class Output
		{
			public int output_no { get; set; }
			public string value { get; set; }
			public string address { get; set; }
			public string type { get; set; }
			public string script { get; set; }
		}
	}
}