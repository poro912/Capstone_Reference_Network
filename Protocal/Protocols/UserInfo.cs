﻿using Protocol.trash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Protocol.Protocols
{
	public class UserInfoProtocol
	{
		public class User
		{
			public int code;
			public string id;
			public string name;
			public string nick;
			public string phone;

			public User() { }
			public User(
				int code,
				string id,
				string name,
				string nick,
				string phone)
			{
				this.code = code;
				this.id = id;
				this.name = name;
				this.nick = nick;
				this.phone = phone;
			}

			public void Get(
				out int code,
				out string id,
				out string name,
				out string nick,
				out string phone)
			{
				code = this.code;
				id = this.id;
				name = this.name;
				nick = this.nick;
				phone = this.phone;
			}
			public void set(int code, string id, string name, string nick, string phone)
			{
				this.code = code;
				this.id = id;
				this.name = name;
				this.nick = nick;
				this.phone = phone;
			}
		}

		// byte 데이터를 생성
		static public void Generate(User target, ref ByteList destination)
		{
			destination.Add(DataType.USER_INFO);
			Generater.Generate(target.code, ref destination);
			Generater.Generate(target.id, ref destination);
			Generater.Generate(target.name, ref destination);
			Generater.Generate(target.nick, ref destination);
			Generater.Generate(target.phone, ref destination);
			return;
		}

		static public RcdResult Convert(ByteList target)
		{
			RcdResult temp;
			User result = new();

			// code 값 입력 받음
			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.code = (int)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.id = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.name = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.nick = (string)temp.Value;

			temp = Converter.Convert(target);
			if (temp.Value != null)
				result.phone = (string)temp.Value;

			return new (DataType.USER_INFO, result);
		}
	}
}