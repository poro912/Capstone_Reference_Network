namespace Protocol
{
    public class MessageProtocol
    {

        public class MESSAGE
        {
            public int creater;
            public string content;
            public DateTime time;
            public bool isPrivate;
			public bool isDelete;
			public MESSAGE()
            {
                creater = 0;
                content = "";
                time = new();
                isPrivate = false;
				isDelete = false;
			}
            public MESSAGE(
                int creater = 0,
                string content = "",
                DateTime startTime = new(),
                bool isPrivate = false,
				bool isDelete = false
				)
            {
                this.creater = creater;
                this.content = content;
                this.time = startTime;
                this.isPrivate = isPrivate;
				this.isDelete = isDelete;
			}

            public void Set(
                int creater,
                string content,
                DateTime startTime,
                bool isPrivate,
				bool isDelete = false
				)
            {
                this.creater = creater;
                this.content = content;
                this.time = startTime;
                this.isPrivate = isPrivate;
				this.isDelete = isDelete;
			}

            // 데이터를 각 변수에 저장
            public void Get(
                out int creater,
                out string content,
                out DateTime startTime,
                out bool isPrivate,
				out bool isDelete
				)
            {
                creater = this.creater;
                content = this.content;
                startTime = this.time;
                isPrivate = this.isPrivate;
				isDelete = this.isDelete;
			}
        }

        // 클래스를 List<byte>로 변환
        static public void Generate(
            MESSAGE target,
            ref ByteList destination
            )
        {
            destination.Add(DataType.MESSAGE);
            Generater.Generate(target.creater, ref destination);
            Generater.Generate(target.content, ref destination);
            Generater.Generate(target.time, ref destination);
            Generater.Generate(target.isPrivate, ref destination);
			Generater.Generate(target.isDelete, ref destination);
		}

        // List<byte>를 클래스로 변환
        static public RcdResult Convert(ByteList target)
        {
            RcdResult temp;
            MESSAGE result = new();

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.creater = (int)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.content = (string)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.time = (DateTime)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.isPrivate = (bool)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.isDelete = (bool)temp.Value;

			return new(DataType.MESSAGE, result);
        }
    }
}
