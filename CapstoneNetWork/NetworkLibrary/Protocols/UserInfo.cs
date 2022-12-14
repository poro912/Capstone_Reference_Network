namespace Protocol
{
	public class UserProtocol
	{
		public class USER
		{
			public int studentID;
			public string name;
			public string nickname;
			public int seqNo;

			public USER()
			{
				studentID = 0;
				name = "";
				nickname = "";
				seqNo = 0;
			}

			public USER(
				int studentID = 0,
				string name = "",
				string nickname = "",
				int seqNo = 0
				)
			{
				this.seqNo = seqNo;
				this.studentID = studentID;
				this.name = name;
				this.nickname = nickname;
			}
			public void Set(
				
				int studentID,
				string name,
				string nickname,
				int seqNo = 0
				)
			{
				this.seqNo = seqNo;
				this.studentID = studentID;
				this.name = name;
				this.nickname = nickname;
			}

			public void Get(
				out int seqNo,
				out int studentID,
				out string name,
				out string nickname
				)
			{
				seqNo = this.seqNo;
				studentID = this.studentID;
				name = this.name;
				nickname = this.nickname;
			}
		}

		// byte 데이터를 생성
		static public void Generate(
			USER target,
			ref ByteList destination
			)
		{
			destination.Add(DataType.USER);
			Generater.Generate(target.studentID, ref destination);
			Generater.Generate(target.name, ref destination);
			Generater.Generate(target.nickname, ref destination);
			Generater.Generate(target.seqNo, ref destination);
			return;
		}

		static public RcdResult Convert(ByteList target)
		{
			RcdResult temp;
			USER result = new();

			// code 값 입력 받음
			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.studentID = (int)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.name = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.nickname = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.seqNo = (int)temp.Value;

			return new(DataType.USER, result);
		}
	}
}