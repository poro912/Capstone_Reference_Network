﻿#pragma warning restore CS1998

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Threading;


namespace Protocol
{
	// Receive에 사용할 델리게이트
	public delegate void DataReceived();

	/// <summary>
	/// 통신을 위한 클래스 
	/// Sender와 Receiver를 동시에 포함한다
	/// 전송 후 수신 완료 메시지도 만들어준다.
	/// </summary>
	public class Communicater
	{
		// 공통 변수
		private NetworkStream? stream;
		// 스트림 저장 멤버
		public NetworkStream? Stream { get; private set; }

		// Sender 변수
		// 전송할 byte[1024] 데이터를 저장하는 큐
		private ConcurrentQueue<byte[]> send_queue;
		private Semaphore send_semaphore;
		private Task send_task;

		// Receiver 변수
		// 입력받은 byte[1024] 데이터를 저장하는 큐
		private ConcurrentQueue<byte[]> receive_queue;
		// 임시 변수
		private byte[] received_byte;
		private bool ReceiveRun = false;

		public static readonly byte[] zeroByte = new byte [1024];

		public bool receiveRun { get { return ReceiveRun; } }

		// 이벤트 발생기
		private event DataReceived? _receiveEvent;
		public event DataReceived receiveEvent
		{
			add
			{
				_receiveEvent += value;
			}
			remove	
			{
				_receiveEvent -= value;
			}
		}

		private event DataReceived? _stopEvent;
		public event DataReceived stopEvent
		{
			add
			{
				_stopEvent += value;
			}
			remove
			{
				_stopEvent -= value;
			}
		}

		public Communicater()
		{
			// send 변수
			send_queue = new ConcurrentQueue<byte[]>();
			send_semaphore = new(1, 1);
			send_task = new Task(this.SendSatrt);
			// receive 변수
			receive_queue = new ConcurrentQueue<byte[]>();
			ReceiveRun = false;
			received_byte = new byte[1024];
		}

		~Communicater()
		{
			Console.WriteLine("Communicater\t: 메모리에서 제거됨");
		}

		public Communicater(NetworkStream stream)
		{
			// 공통 변수
			this.Stream = stream;
			// send 변수
			send_queue = new ConcurrentQueue<byte[]>();
			send_semaphore = new(1, 1);
			send_task = new Task(this.SendSatrt);
			// receive 변수
			receive_queue = new ConcurrentQueue<byte[]>();
			ReceiveRun = false;
			received_byte = new byte[1024];
		}

		// 스트림을 설정한다.
		public void SetStream(NetworkStream stream)
		{
			this.stream = stream;
		}
		// ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡSenderㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
		
		
		// 큐에 데이터를 삽입한 후 비동기로 데이터 전송
		public void Send(ByteList data)
		{
			send_queue.Enqueue(data.ToArray());
			//send_task = new Task(this.SendSatrt);
			//send_task.Start();
			SendSatrt();
		}
		public void Send(byte[] data)
		{
			send_queue.Enqueue(data);
			//send_task = new Task(this.SendSatrt);
			//send_task.Start();
			SendSatrt();
		}

		// 데이터 연속전송
		private async void SendSatrt()
		{
			send_task = new Task(this.SendSatrt);
			if (!send_semaphore.WaitOne(10))
				return;

			// 큐가 빌때까지 반복실행
			while (!send_queue.IsEmpty)
			{
				// 만약 읽어온 항목이 있다면
				if (send_queue.TryDequeue(out byte[]? data))
				{
					SendProcess(data);
				}
				else
					break;
			}
			
			send_semaphore.Release();
		}

		// byte 형태의 데이터 전송
		// 하나의 바이트 배열을 전송 할 때 사용
		public void SendProcess(Byte[] data)
		{
			
			// 들어온 데이터가 없다면 종료
			if (data.Length.Equals(0))
				return;
			if (this.stream == null)
			{
				StopReceive();
				return;
			}

			try
			{
				// 현재 1024 를 넘기는 데이터는 받을 수 없는 형태로 작성되어있으므로
				// 무조건 1024 바이트로만 데이터를 전소한다.
				// 추후 1024 바이트를 넘기는 데이터를 받을 수 있게 되면 이곳을 변경해야 한다. 
				this.stream.Write(data, 0, data.Length);
				this.stream.Write(zeroByte, 0, 1023 - data.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("Communicater\t: 데이터 송신 중 문제 발생");
				Console.WriteLine(e.ToString());
				throw;
			}
		}

		// ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡReceiverㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

		// 수신 시작
		public void StartReceive()
		{
			if(false == this.ReceiveRun)
			{
				this.ReceiveRun = true;
				ReceiveProcess();
			}
		}
		// 수신 중지
		public void StopReceive()
		{
			Console.WriteLine("Communicater\t: StopReceive");
			this.ReceiveRun = false;
			if(_receiveEvent != null)
				_receiveEvent();
			if (null != _stopEvent)
				_stopEvent();
		}
		
		// Receive.Data()
		// 데이터를 읽어오기 시작한다.
		private void ReceiveProcess()
		{
			Console.WriteLine("Communicater\t: ReceiveProcess");

			// 실행 상태가 아니라면종료
			if (!this.ReceiveRun)
			{
				Console.WriteLine("Communicater\t: not in running state");
				if(null != _stopEvent)
					_stopEvent();
				
				return;
			}

			// 스트림이 설정되어 있지 않다면 종료
			if (this.stream == null)
			{
				StopReceive(); 
				return;
			}
				
			if(this.Stream != null)
			{
				if (this.Stream.Socket.Connected == false)
				{
					StopReceive();
					return;
				}
			}

			// 새로운 저장소 할당
			received_byte = new byte[1024];

			try
			{
				// 데이터를 받기 시작한다.
				// 받은 데이터를 received_byte에 저장한다.
				// 데이터 수신을 완료하면 SaveData를 호출한다.
				this.stream.BeginRead(
					this.received_byte, 0, this.received_byte.Length,
					new AsyncCallback(SaveDataEnQueue), null);
			}
			catch (System.IO.IOException)
			{
				Console.WriteLine("Communicater\t: 연결이 끊어졌습니다.");
				//throw;
			}
			catch (Exception ex)
			{
				Console.Write("Communicater\t: Receive 오류 발생 \t: ");
				Console.WriteLine(ex.ToString());
				//throw;
			}
		}

		// 데이터 삽입
		private void SaveDataEnQueue(IAsyncResult ar)
		{
			Console.WriteLine("Communicater\t: 데이터 들어옴");
			Console.WriteLine("Communicater\t: 데이터 길이 : " + received_byte.Length);
			string receive_data = Encoding.Default.GetString(this.received_byte);
			Console.WriteLine("Communicater\t: 내용 : " + receive_data);

			if (received_byte[0] == 0)
			{
				StopReceive();
				return;
			}
			if (receive_data.Equals(""))
			{
				StopReceive();
				return;
			}
				
			// 데이터를 받아오는 대로 queue에 저장
			receive_queue.Enqueue(this.received_byte);
			received_byte = new byte [1024]; 

			// 처리 끝 다음 데이터를 받을 준비를 함
			// 다음 데이터 받을 준비를 먼저 한 후 이벤트를 호출한다.
			ReceiveProcess();

			// 이벤트 호출
			// 이벤트가 할당되어 있으며, 큐가 비어있지 않은 경우
			if (_receiveEvent != null)
				if (!this.IsEmpty())
					_receiveEvent();
		}

		// get 이후 NULL값 확인을 해야함
		// byte[] 형태로 반환하는 pop
		public void Receive(out byte[]? destination)
		{
			this.receive_queue.TryDequeue(out destination);
		}
		public RcdResult Receive()
		{
			this.receive_queue.TryDequeue(out byte[]? temp);
			if (temp == null)
				return new(0, 0);
			return Protocol.Converter.Convert(temp);
		}

		// get 이후 NULL값 확인을 해야함
		// ByteList 형태로 반환하는 pop
		public void Receive(out ByteList destination)
		{
			byte[]? temp;
			this.receive_queue.TryDequeue(out temp);
			if (temp != null)
				destination = temp.ToList();
			else
				destination = new ByteList();
		}

		// 큐가 비었는지 확인
		public bool IsEmpty()
		{
			return this.receive_queue.IsEmpty;
		}
	}
}
