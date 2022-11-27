using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Protocol.LoginProtocol;
using static Protocol.MessageProtocol;

namespace Protocol
{
    public class GameStartProtocol
    {
        public class GameStart 
        {
            public int meanless = 0;
        }

		static public void Generate(
			GameStart target,
			ref ByteList destination
			)
		{
			destination.Add(DataType.GAME_START);
			Generater.Generate(target.meanless, ref destination);
		}

		// List<byte>를 클래스로 변환
		static public RcdResult Convert(ByteList target)
		{
			RcdResult temp;
			GameStart result = new();

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.meanless = (int)temp.Value;

			return new(DataType.MESSAGE, result);
		}
	}
}
