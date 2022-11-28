namespace Protocol
{
	public class MessageProtocol
	{

		public class MESSAGE
		{
			public int studentID;
			public int targetID;
			public string content;
			public int seq;

			public MESSAGE()
			{
				studentID = 0;
				targetID = 0;
				content = "";
				seq = 0;
			}
			public MESSAGE(
				int studentID = 0,
				int targetID = 0,
				string content = "",
				int seq = 0
				)
			{
				this.studentID = studentID;
				this.targetID = targetID;
				this.content = content;
				this.seq = seq;
			}

			public void Set(
				int studentID,
				int targetID,
				string content,
				int seq = 0
				)
			{
				this.studentID = studentID;
				this.targetID = targetID;
				this.content = content;
				this.seq = seq;
			}

			// 데이터를 각 변수에 저장
			public void Get(
				out int studentID,
				out int targetID,
				out string content
				)
			{
				studentID = this.studentID;
				targetID = this.targetID;
				content = this.content;
			}
			public bool check(int studentID,int seq)
			{
				return seq == this.seq && studentID == this.studentID;
			}
		}

		// 클래스를 List<byte>로 변환
		static public void Generate(
			MESSAGE target,
			ref ByteList destination
			)
		{
			destination.Add(DataType.MESSAGE);
			Generater.Generate(target.studentID, ref destination);
			Generater.Generate(target.targetID, ref destination);
			Generater.Generate(target.content, ref destination);
			Generater.Generate(target.seq, ref destination);
		}

		// List<byte>를 클래스로 변환
		static public RcdResult Convert(ByteList target)
		{
			RcdResult temp;
			MESSAGE result = new();

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.studentID = (int)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.targetID = (int)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.content = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.seq = (int)temp.Value;

			return new(DataType.MESSAGE, result);
		}
	}
}
