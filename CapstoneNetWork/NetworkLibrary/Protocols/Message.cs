namespace Protocol
{
    public class MessageProtocol
    {

        public class MESSAGE
        {
            public int seqNo;
            public int targetSeq;
            public string content;

			public MESSAGE()
            {
                seqNo = 0;
                targetSeq = 0;
                content = "";
			}
            public MESSAGE(
                int seqNo=0,
                int targetSeq=0,
                string content=""
				)
            {
                this.seqNo = seqNo;
                this.targetSeq = targetSeq;
                this.content = content;
			}

            public void Set(
                int seqNo,
                int targetSeq,
                string content
                )
            {
                this.seqNo = seqNo;
                this.targetSeq = targetSeq;
                this.content = content;
            }

            // 데이터를 각 변수에 저장
            public void Get(
                out int seqNo,
                out int targetSeq,
                out string content
				)
            {
                seqNo = this.seqNo;
                targetSeq = this.targetSeq;
                content = this.content;
			}
        }

        // 클래스를 List<byte>로 변환
        static public void Generate(
            MESSAGE target,
            ref ByteList destination
            )
        {
            destination.Add(DataType.MESSAGE);
            Generater.Generate(target.seqNo, ref destination);
            Generater.Generate(target.targetSeq, ref destination);
            Generater.Generate(target.content, ref destination);
		}

        // List<byte>를 클래스로 변환
        static public RcdResult Convert(ByteList target)
        {
            RcdResult temp;
            MESSAGE result = new();

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.seqNo = (int)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.targetSeq = (int)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.content = (string)temp.Value;

			return new(DataType.MESSAGE, result);
        }
    }
}
