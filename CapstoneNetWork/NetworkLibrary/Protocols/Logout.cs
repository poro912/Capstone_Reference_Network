using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
	public class LogoutProtocol
	{
		public class LOGOUT
		{
			// Data Declear
			public int seqNo;
			public int studentID;
			public LOGOUT()
			{
				seqNo = 0;
				studentID = 0;
			}
			public LOGOUT(
				int seqNo = 0,
				int studentID = 0
				)
			{
				this.seqNo = seqNo;
				this.studentID = studentID;
			}

			public void Get(
				out int seqNo,
				out int studentID
			)
			{
				seqNo = this.seqNo;
				studentID = this.studentID;
			}

			public void Set(
				int seqNo,
				int studentID
				)
			{
				this.seqNo = seqNo;
				this.studentID = studentID;
			}
		}

		static public void Generate(
			LOGOUT target,
			ref ByteList destination
			)
		{
			destination.Add(DataType.LOGOUT);
			Generater.Generate(target.seqNo, ref destination);
			Generater.Generate(target.studentID, ref destination);
		}
		// List<byte>를 클래스로 변환
		static public RcdResult Convert(ByteList target)
		{
			RcdResult temp;
			LOGOUT result = new();

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.seqNo = (int)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.studentID = (int)temp.Value;

			return new(DataType.LOGOUT, result);
		}
	}
}
