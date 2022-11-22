using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Protocol;

namespace ClientToServer
{
	// 서버에 대한 연결 정보를 갖고 있기위한 클래스
	public class Server : Communicater
	{
		// 서버의 tcpclient 정보 저장
		public TcpClient tcpclient;

		// 서버의 stream 저장
		private NetworkStream? stream;
		public static string? setAddress = null;
		public readonly string address;
		public readonly int port;

		static private Server? instance;
		static public Server Instance
		{
			get
			{
				// instance == null 이라면
				instance ??= new Server();
				return instance;
			}
		}

		private Server(
			string address = Default.Network.Address,
			int port = Default.Network.port
			)
		{
			Console.WriteLine("CtoS\t: Connect 객체 생성");
			// Address 설정
			if (null != setAddress)
				this.address = setAddress;
			else
				this.address = address;

			this.port = port;
			Console.WriteLine("Default Address : " + this.address);

			Console.WriteLine("CtoS\t: 서버측 데이터 삽입 완료");
			this.tcpclient = new();

			Console.WriteLine("CtoS\t: 객체 생성 완료");

			// 송수신 시작
			this.Start();
		}

		~Server()
		{
			this.tcpclient.Close();
		}

		// 통신을 시작할 때 실행한다.
		public void Start()
		{
			Console.Write("CtoS\t: 커넥트 실행 \t");
			try
			{
				//this.tcpclient.Connect("127.0.0.1", 8090);
				this.tcpclient.Connect(this.address, this.port);

				// 스트림을 현재 객체에 저장
				this.stream = tcpclient.GetStream();

				// 송수신(Communicator) 스트림 설정
				this.SetStream(stream);

				//수신을 시작한다.
				this.StartReceive();
			}
			catch (Exception e)
			{
				Console.Write("\n커넥트 에러 발생 \t");
				Console.WriteLine(e.ToString());
				return;
			}
			Console.WriteLine("성공");
		}
	}
}
