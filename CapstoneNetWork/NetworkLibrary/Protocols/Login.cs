using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Protocol
{
    public class LoginProtocol
    {
        public class LOGIN
        {
            public int seqNo;
            public int studentID;
            public string name;
            public string nickName;
            public LOGIN() {
                seqNo = 0;
                studentID = 0;
                name = "";
                nickName = "";
            }
            public LOGIN(
                int seqNo,
                int studentID,
                string name,
                string nickName
                )
            {
                this.seqNo = seqNo;
                this.studentID = studentID;
                this.name = name;
                this.nickName = nickName;
            }

            public void Get(
                out int seqNo,
                out int studentID,
                out string name,
                out string nickName
                )
            {
                seqNo = this.seqNo;
                studentID = this.studentID;
                name = this.name;
                nickName = this.nickName;
            }

            public void Set(
                int seqNo,
                int studentID,
                string name,
                string nickName
                )
            {
                this.seqNo = seqNo;
                this.studentID = studentID;
                this.name = name;
                this.nickName = nickName;
            }
        }

        static public void Generate(
            LOGIN target,
            ref ByteList destination
            )
        {
            destination.Add(DataType.LOGIN);
            Generater.Generate(target.seqNo, ref destination);
            Generater.Generate(target.studentID, ref destination);
            Generater.Generate(target.name, ref destination);
            Generater.Generate(target.nickName, ref destination);
        }

        static public RcdResult Convert(ByteList target)
        {
            RcdResult temp;
            LOGIN result = new();

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.seqNo = (int)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.studentID = (int)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.name = (string)temp.Value;

            temp = Converter.Convert(target);
            if (temp.Value != null)
                result.nickName = (string)temp.Value;

            return new(DataType.LOGIN, result);
        }

    }
}
