using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
	/// <summary>
	///网关信息
	/// </summary>
	[Serializable]
	internal class ApiGateResult
	{
		/// <summary>
		/// 返回码
		/// </summary>
		public int RetCode
		{
			get;
			set;
		}
		/// <summary>
		/// 返回消息
		/// </summary>
		public string RetMsg
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public string LastLoadApisTime
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public string IntervalTime
		{
			get;
			set;
		}
		/// <summary>
		/// 文档类型
		/// </summary>
		public string ContentType
		{
			get;
			set;
		}
		/// <summary>
		/// 消息
		/// </summary>
		public string Message
		{
			get;
			set;
		}
	}

	/// <summary>
	/// 数据结果
	/// </summary>
	/// <typeparam name="T">数据类型</typeparam>
	[Serializable]
	public class ApiResultInfo<T>
	{
		/// <summary>
		/// 成功状态
		/// </summary>
		public bool HasSuccess
		{
			get;
			set;
		}
		/// <summary>
		/// 返回消息
		/// </summary>
		public string RetMsg
		{
			get;
			set;
		}
		/// <summary>
		/// 返回状态吗
		/// </summary>
		public int RetCode
		{
			get;
			set;
		}
		/// <summary>
		/// 结果数据
		/// </summary>
		public T InfoObj
		{
			get;
			set;
		}
	}
}
