namespace Protocol
{
	public class UserProtocol
	{
		public class USER
		{
			public int userCode;
			public string id;
			public string name;
			public string nickname;

			public USER()
			{
				userCode = 0;
				id = "";
				name = "";
				nickname = "";
			}

			public USER(
				int userCode = 0,
				string id = "",
				string name = "",
				string nickname = ""
				)
			{
				this.userCode = userCode;
				this.id = id;
				this.name = name;
				this.nickname = nickname;
			}
			public void Set(
				int userCode,
				string id,
				string name,
				string nickname
				)
			{
				this.userCode = userCode;
				this.id = id;
				this.name = name;
				this.nickname = nickname;
			}

			public void Get(
				out int userCode,
				out string id,
				out string name,
				out string nickname
				)
			{
				userCode = this.userCode;
				id = this.id;
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
			Generater.Generate(target.userCode, ref destination);
			Generater.Generate(target.id, ref destination);
			Generater.Generate(target.name, ref destination);
			Generater.Generate(target.nickname, ref destination);
			return;
		}

		static public RcdResult Convert(ByteList target)
		{
			RcdResult temp;
			USER result = new();

			// code 값 입력 받음
			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.userCode = (int)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.id = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.name = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.nickname = (string)temp.Value;

			return new(DataType.USER, result);
		}
	}
}