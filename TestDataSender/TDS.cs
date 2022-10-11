﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Protocol;
using Protocol.Protocols;

using static Protocol.Protocols.UserInfoProtocol;
using sConvert = System.Convert;

namespace TestDataSender
{
	// 해당 클래스는 출력 스트림이 생성되고 바인딩 된 상태에서 사용해야 함
	public class TDS
	{
		public Thread thread;

		public Send send;

		// variable
		int i = 0;
		string str = "";

		// Controll
		public isConnectProtocol.IsConnect isConnect = new();
		public LoginProtocol.Login login = new();
		public LogoutProtocol.Logout logout = new();

		// Class
		public MessageProtocol.Message message = new();
		public UserInfoProtocol.User user = new();

		private List<byte> send_byte;

		private char send_way = '0';		// 전송 방법 기존데이터 / 신규 데이터
		private char send_type = '0';		// 전송 데이터 일반변수 / 클래스
		private char send_ctrl = '0';
		private char send_data = '0';           // 일반 변수 중 어떤 변수 protocol에 저장된 상수로 전송
		private char send_class = '0';

		public TDS(Send send)
		{
			send_byte = new();

			this.send = send;

			isConnect.Set(1);
			login.Set("admin", "1234");
			logout.Set(0, "admin");
			message.Set(0, 0, "hello", DateTime.Now);
			user.set(0, "admin", "admin", "admin", "010-0000-0000");
			

			thread = new(run);
			thread.Start();
		}

		public void run()
		{
			Thread.Sleep(1000);
			while(true)
			{
				Console.WriteLine("\n1 : 저장된 데이터\t 2 : 새로운 데이터");
				Console.WriteLine("Q/q : 종료");
				Console.Write("어떤 방식으로 전송하십니까? : ");
				send_way = Console.ReadKey().KeyChar;
				Console.WriteLine();

				if ('q' == send_way || 'Q' == send_way)
					break;
				if ('1' == send_way || '2' == send_way)
				{
					try
					{
						data_select_run();
					}
					catch
					{
						send_way = 'q';
						Console.WriteLine("문제 발생");
						break;
					}
				}
			}
		}

		public void data_select_run()
		{
			while(true)
			{
				Console.WriteLine("\n1 : 기본 변수\t 2 : 컨트롤\t 3 : 메소드\t");
				Console.WriteLine("Q/q : 돌아가기");
				Console.Write("어떤 자료를 전송하십니까 : ");
				send_type = Console.ReadKey().KeyChar;
				Console.WriteLine();

				try
				{
					if ('q' == send_type || 'Q' == send_type)
						break;
					else if ('1' == send_type)
						SendVariable();
					else if ('2' == send_type)
						SendControll();
					else if ('3' == send_type)
						SendClass();
				}
				catch
				{
					send_type = 'q';
					throw;
				}
			}
		}

		// 변수 전송
		public void SendVariable()
		{
			while(true)
			{
				Console.WriteLine("\nint : i\t sting : s");
				Console.WriteLine("Q/q : 돌아가기");
				Console.Write("어떤 데이터를 전송합니까? : ");
				send_data = Console.ReadKey().KeyChar;
				Console.WriteLine();

				if ('q' == send_data || 'Q' == send_data)
					break;

				send_byte.Clear();

				// 새로운데이터 전송이라면 데이터를 받음
				if (send_way == '2')
				{
					if(send_data == 'i' || send_data == 'I')
					{
						i = sConvert.ToInt32(Console.ReadLine());
					}
					else if (send_data == 's' || send_data == 'S')
					{
						str = Console.ReadLine();
					}
				}

				if (send_data == 'i' || send_data == 'I')
					Generater.Generate(i, ref send_byte);
				else if (send_data == 's' || send_data == 'S')
					Generater.Generate(str, ref send_byte);

				try
				{
					send.Data(send_byte);
					Console.WriteLine("다시전송 : r");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					send_data = 'q';
					throw;
				}

			}
		}

		public void SendControll()
		{
			while (true)
			{
				Console.WriteLine("\nLogin : i\t Logout : o");
				Console.WriteLine("InConnect : c\t ");
				Console.WriteLine("Q/q : 돌아가기");
				Console.Write("어떤 데이터를 전송합니까? : ");
				send_ctrl = Console.ReadKey().KeyChar;
				Console.WriteLine();

				if ('q' == send_ctrl || 'Q' == send_ctrl)
					break;

				send_byte.Clear();

				// 새로운데이터 전송이라면 데이터를 받음
				if (send_way == '2')
				{
					if(send_ctrl == 'i' || send_ctrl == 'I')
					{
						Console.Write("id\t: ");
						login.id = Console.ReadLine();
						Console.Write("pw\t: ");
						login.pw = Console.ReadLine();
						Generater.Generate(login, ref send_byte);
					}
					else if(send_ctrl == 'o' || send_ctrl == 'O')
					{
						Console.Write("usercode\t: ");
						logout.usercode = sConvert.ToInt32(Console.ReadLine());
						Console.Write("id\t: ");
						logout.id = Console.ReadLine();
						Generater.Generate(logout, ref send_byte);
					}
					else if (send_ctrl == 'c' || send_ctrl == 'C')
					{
						Console.Write("Connect info\t: ");
						isConnect.connectInfo = sConvert.ToInt32(Console.ReadLine());
						Generater.Generate(isConnect, ref send_byte);
					}
				}


				if (send_ctrl == 'i' || send_ctrl == 'I')
					Generater.Generate(login, ref send_byte);
				else if (send_ctrl == 'o' || send_ctrl == 'O')
					Generater.Generate(logout, ref send_byte);
				else if (send_ctrl == 'c' || send_ctrl == 'C')
					Generater.Generate(isConnect, ref send_byte);

				try
				{
					send.Data(send_byte);
					Console.WriteLine("다시전송 : r");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					send_ctrl = 'q';
					throw;
				}
			}
		}

		//클래스 전송
		public void SendClass()
		{
			while (true)
			{
				Console.WriteLine("\nUserInfo : u\t Message : m\t ");
				Console.WriteLine("Q/q : 돌아가기");
				Console.Write("어떤 데이터를 전송합니까? : ");
				send_class = Console.ReadKey().KeyChar;
				Console.WriteLine();

				if ('q' == send_class || 'Q' == send_class)
					break;

				send_byte.Clear();

				// 새로운데이터 전송이라면 데이터를 받음
				if (send_way == '2')
				{
					if(send_class == 'u' || send_class == 'U')
					{
						Console.Write("code\t: ");
						user.code = sConvert.ToInt32(Console.ReadLine());
						Console.Write("id\t: ");
						user.id = Console.ReadLine();
						Console.Write("name\t: ");
						user.name = Console.ReadLine();
						Console.Write("nick\t: ");
						user.nick = Console.ReadLine();
						Console.Write("phone\t: ");
						user.phone = Console.ReadLine();

						Generater.Generate(user, ref send_byte);
					}
					else if(send_class == 'm' || send_class == 'M')
					{
						Console.Write("usercode\t: ");
						message.usercode = sConvert.ToInt32(Console.ReadLine());
						Console.Write("servercode\t: ");
						message.servercode = sConvert.ToInt32(Console.ReadLine());
						Console.Write("context\t: ");
						message.context = Console.ReadLine();

						message.date = DateTime.Now;

					}
				}

				if (send_class == 'u' || send_class == 'U')
					Generater.Generate(user, ref send_byte);
				else if (send_class == 'm' || send_class == 'M')
					Generater.Generate(message, ref send_byte);

				try
				{
					send.Data(send_byte);
					Console.WriteLine("다시전송 : r");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					send_class = 'q';
					throw;
				}
			}
		}

	}
}
