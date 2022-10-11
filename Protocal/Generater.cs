﻿
using Protocol.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
	static public class Generater
	{
		// List<byte>

		// 일반 자료형
		// int
		static public void Generate(int target, ref ByteList destination)
		{
			// 해석하기 위한 데이터 삽입
			destination.Add(DataType.INT);

			// int 형 데이터를 해당 byte 형으로 변환
			destination.AddRange(BitConverter.GetBytes(target));
		}

		// string
		static public void Generate(string target, ref ByteList destination)
		{
			// 해석하기 위한 데이터 삽입
			destination.Add(DataType.STRING);

			// 문자열의 길이 삽입
			destination.AddRange(BitConverter.GetBytes(target.Length));

			// 문자열의 내용 삽입
			destination.AddRange(Encoding.UTF8.GetBytes(target));
		}

		// bool
		static public void Generate(bool target, ref ByteList destination)
		{
			// 해석하기 위한 데이터 삽입
			destination.Add(DataType.BOOL);

			// int 형 데이터를 해당 byte 형으로 변환
			destination.AddRange(BitConverter.GetBytes(target));
		}

		// 제어 데이터형
		// Login
		static public void Generate(LoginProtocol.Login target, ref ByteList destination)
		{
			LoginProtocol.Generate(target, ref destination);
			return;
		}

		// Login
		static public void Generate(LogoutProtocol.Logout target, ref ByteList destination)
		{
			LogoutProtocol.Generate(target, ref destination);
			return;
		}
		// Login
		static public void Generate(isConnectProtocol.IsConnect target, ref ByteList destination)
		{
			isConnectProtocol.Generate(target, ref destination);
			return;
		}

		// 클래스 형
		// User
		static public void Generate(UserInfoProtocol.User target, ref ByteList destination)
		{
			UserInfoProtocol.Generate(target, ref destination);
			return;
		}

		// Message
		static public void Generate(MessageProtocol.Message target, ref ByteList destination)
		{
			MessageProtocol.Generate(target, ref destination);
			return;
		}


	}
}



/*
	// 전송 데이터 생성기
	// 값 -> List<byte>
	static public class Generater
	{
		// 전송에 필요한 형태로 변환
		// int 를 byte 형으로 변환
		static public void Generate(ref int target, ref byte[] destination)
		{
			// 해석하기 위한 데이터 삽입
			destination.Append(DataType.INT);

			// int 형 데이터를 해당 byte 형으로 변환
			BitConverter.GetBytes(target).CopyTo(destination, destination.Length);
		}
		static public void Generate(ref string target, ref byte[] destination)
		{
			destination.Append(DataType.STRING);


			// 해석하기 위한 데이터 삽입
			foreach (var i in BitConverter.GetBytes(destination.Length))
			{
				destination.Append(i);
			}

		}
	}
	*/