using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serial_protocol.Protocol.AsyncSocket
{
	abstract class IocpClient : IocpSocketBase
	{
#if false
		#region /**** setup logger ****/
			protected static Serilog.ILogger _logger = null;                                                                                        // 			protected static log4net.ILog _logger = null;
			public static void SetupLogger(string repo, string name, string path) { _logger = Serilog.helper.Create(path); }            // 			public static void SetupLogger(string repo, string name, string path) { _logger = log4net.helper.Create(repo, name, path); }
			private static bool _write_raw_packet = true;
		#endregion
#endif

		// ManualResetEvent instances signal completion.  
		private ManualResetEvent _sync_event_recvDone = new ManualResetEvent(false);  //connect 에도 공용으로 사용
		private ManualResetEvent _sync_event_sendDone = new ManualResetEvent(true);

		private bool _stop_thread = false;
		private bool _pause_thread = false;
		private Thread _thread = null;

		private IocpEventArgs _socketObject = null;

		private int _reconnectInterval_msec = 0;     // 콜백이 1초정도마다 호출 --> 1초 기본 시간 + Alpha

		// 연결 상태 확인
		public string IpAddress { get; private set; } = string.Empty;
		public int Port { get; private set; } = 0;
		public bool IsConnected { get; private set; } = false;

		public void Start(int msecTryInterval = 1000)       // (string ip/*ex "127.0.0.1"*/, Int32 port/*ex 65535*/, Int32 msecTryInterval = 1000)
		{
			// 재접속 시도 지연 시간 -> 이것보다 좀 더 지연된다.
			_reconnectInterval_msec = Math.Max(1000, msecTryInterval);

			// Create Socket Object
			try
			{
				IpAddress = _dstip;
				Port = _dstport;
				// 					IpAddress = ip;
				// 					Port = port;

				// 로컬 환경에서 시간이 매우 느림 - 원인은 잘 모르겠다...웹 접속이 아닌경우? 게이트웨이 설정 없을때?
				//IPAddress ipAddress = Helper.IPAddressContainer.GetAddress(ip);
				IPAddress ipAddress = IPAddress.Parse(IpAddress);
				IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);

				// Create a TCP/IP socket.
				var CreateSocketObject = new Func<IocpEventArgs>(() =>
				{
					Socket socket = null;
					try
					{
						socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

						socket.LingerState = new LingerOption(true, 0);
						socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

						// 							socket.SetKeepAlive(true, 10000, 1000);

						// 특정 로컬 IP/포트에 바인딩 - 강제 지정
						if ("0.0.0.0" != _myip && 0 != _myport)
						{
							IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(_myip), _myport);
							socket.Bind(localEndPoint);
						}

						return new IocpEventArgs(_nodeID, socket);
					}
					catch (System.Exception ex)
					{
						_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|CreateAsyncSocketObject : Failed", ex);
					}
					return null;
				});

				// Thread for Connection and Receive
				_thread = new Thread(() =>
				{
					// const Int32 timeoutConnectToHost = 2000;
					while (false == _stop_thread)
					{
						// Connect to a remote device.  
						try
						{
							// 강제 종료 기능
							if (_pause_thread)
							{
								//Thread.Sleep(1000);
								MySleep(ref _stop_thread, _reconnectInterval_msec);
								continue;
							}

							if (null == _socketObject)
							{
								IsConnected = false;
								_socketObject = CreateSocketObject();
							}

							if (null != _socketObject)
							{
								_sync_event_recvDone.Reset();
								{
									if (false == IsConnected)
									{
										_socketObject.Socket.BeginConnect(ipEndPoint, new AsyncCallback(ConnectCallBack), _socketObject.Socket);
									}
									else
									{
										_socketObject.Socket.BeginReceive(_socketObject.Buffer, 0, _socketObject.Buffer.Length, 0, new AsyncCallback(ReceiveCallback), _socketObject);
									}
								}
								// ConnectCallBack 또는 ReceiveCallback 함수에서 이벤트를 Set할 것이다.
								_sync_event_recvDone.WaitOne();

								// 강제 종료 함수 실행 또는 APP. 종료 이벤트 발생되면...
								if (_socketObject.FlagForceDisconnect || _stop_thread)
								{
									DeleteAsyncSocketObject();
								}
							}

							// 스레드 종료시에는 대기 없음
							if (false == _stop_thread)
							{
								// 종료시에 늦게 종료되는 이유-> 여기 머무를 때 종료하면 무한 대기 할 듯...
								if (false == IsConnected || null == _socketObject)
								{
									// Thread.Sleep(_msec_connRetryInterval);
									MySleep(ref _stop_thread, _reconnectInterval_msec);
								}
							}
						}
						catch (System.Exception ex)
						{
							_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|CreateAsyncSocketObject : Failed", ex);

							// 스레드 종료 타이밍에 시퀀스 꼬임...
							if (false == _stop_thread)
							{
								// 프로그램 종료시 발생 회피
								// ex : {"스레드가 중단되었습니다."}
								DeleteAsyncSocketObject();
							}
						}
					}
				});
				_thread.IsBackground = true;
				_thread.Start();
			}
			catch (System.Exception ex)
			{
				_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
			}
		}

		public void Stop(Int32 wait_msec)
		{
			_reconnectInterval_msec = 0;
			_stop_thread = true;

			_sync_event_recvDone.Set();
			_sync_event_sendDone.Set();

			// 				ForceDisconnect();

			if (_thread.Join(wait_msec))
			{
				_logger?.Debug($"SOCK|CLT|{IpAddress}:{Port}|Client Thread : Safely termminated.");
			}
			else
			{
				_logger?.Debug($"SOCK|CLT|{IpAddress}:{Port}|Client Thread : The timeout has elapsed and Thread1 will resume.");
			}

			try
			{
				if (null != _thread)
				{
					if (_thread.IsAlive)
						_thread.Abort();
					_thread = null;
				}
			}
			catch (System.Exception ex)
			{
				_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
			}

			try
			{
				DeleteAsyncSocketObject();
			}
			catch (System.Exception ex)
			{
				_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
			}

		}

		private void DeleteAsyncSocketObject()
		{
			IsConnected = false;

			if (null == _socketObject)
				return;

			_socketObject.Socket.Discard();

			// 				lock (_socketObject.Locker)
			// 					_socketObject.SB.Clear();
			// 
			// 				if (!_socketObject.PacketQ.IsEmpty)
			// 				{
			// 					Tuple<DateTime, string> tuple;
			// 					while (_socketObject.PacketQ.TryDequeue(out tuple)) { }
			// 				}

			_socketObject = null;
		}

		public void ForceDisconnect()
		{
			// 소켓 종료 시킴...
			_pause_thread = true;
			{
				try
				{
					if (null != _socketObject)
					{
						_socketObject.FlagForceDisconnect = true;
						_socketObject.Socket.Shutdown(SocketShutdown.Both);
					}
				}
				catch (System.Exception ex)
				{
					_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
				}

			}
			_pause_thread = false;
		}

		private void ConnectCallBack(IAsyncResult ar)
		{
			// BeginConnect 호출 후 일정시간(1초) 뒤에 콜백 들어온다...
			try
			{
				IsConnected = false;

				// Complete the connection sequence, excection if failed  
				Socket client = ar.AsyncState as Socket;
				if (null == client)
					return;

				// 접속 실패시 아래 함수에서 예외 발생
				// 2021.8.22 종료시에도 여기서 예외 발생 - 확인하고 처리해야 할 것..
				client.EndConnect(ar);

				// 다른 함수 실행하면 client.Connected 값이 바뀐다....
				bool isConnected = client.Connected;

				// 접속 직후 전송시 서버쪽 준비 시간을 줄까??
				//				Thread.Sleep(1000);

				// 성공하면 아래 함수까지 도착
				// client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

				if (isConnected)
				{
					_logger?.Debug($"SOCK|CLT|{IpAddress}:{Port}|CONN:SUCC|BIND:{client.LocalEndPoint.ToString()}|DEST:{client.RemoteEndPoint.ToString()}");
				}
				else
				{
					_logger?.Debug($"SOCK|CLT|{IpAddress}:{Port}|CONN:FAIN|DEST:{client.RemoteEndPoint.ToString()}");
				}

				IsConnected = isConnected;

				base.OnConnected(null);
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				// 로그보다 변수 업데이트가 먼저임...
				IsConnected = false;

				if (System.Net.Sockets.SocketError.ConnectionRefused != ex.SocketErrorCode)
				{
					var s = ar?.AsyncState as System.Net.Sockets.Socket;
					if (null != s)
						_logger?.Information($"SOCK|CLT|ConnectionRefused|{s.LocalEndPoint.ToString()}=>{IpAddress}:{Port}");
					else
						_logger?.Information($"SOCK|CLT|ConnectionRefused|LocalEndPoint=>{IpAddress}:{Port}");
				}

				// Thread.Sleep(1000);
				MySleep(ref _stop_thread, _reconnectInterval_msec);
			}
			catch (System.Exception ex)
			{
				// 로그보다 변수 업데이트가 먼저임...
				IsConnected = false;
				_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
			}
			finally
			{
				_sync_event_recvDone.Set();
			}
		}


		private void ReceiveCallback(IAsyncResult ar)
		{
			// Retrieve the state object and the handler socket from the asynchronous state object.  
			IocpEventArgs sockObj = ar.AsyncState as IocpEventArgs;
			if (null == sockObj) return;
			Socket socket = sockObj.Socket;

			try
			{
				// Read data from the client socket.
				sockObj.RecvBytes = socket.EndReceive(ar);
				if (0 < sockObj.RecvBytes)
				{
					base.OnReceived(sockObj);
					if (_write_raw_packet)
						_logger?.Debug($"SOCK|CLT|RX|{socket.RemoteEndPoint.ToString()}|{sockObj.Buffer.ToHex(" ", sockObj.RecvBytes)}");
					// 						_logger?.Debug($"{socket.RemoteEndPoint.ToString()}->{socket.LocalEndPoint.ToString()}|RX|{sockObj.Buffer.ToHex(" ", sockObj.RecvBytes)}");
				}
				else
				{
					// 상대편 정상 종료시 0 바이트 수신
					if (/*IsConnected &&*/ false == _stop_thread)   // 종료할 때는 여기서 하지 않는다...
					{
						_logger?.Debug($"SOCK|CLT|{IpAddress}:{Port}|Disconnected : {socket.RemoteEndPoint.ToString()} : 0 Byte Recv");
						socket.DisconnectEx(true);
						// 접속 끊고 잠깐 대기
						// IsConnected = false;
						// Thread.Sleep(2000);
					}
				}
			}
			catch (System.Exception ex)
			{
				// 상대편 비정상/강제 종료시 예외 발생하는 듯...
				if (/*IsConnected &&*/ false == _stop_thread)   // 종료할 때는 여기서 하지 않는다...
				{
					_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");       //	_logger?.Debug($"{IpAddress}:{Port}|Exception", ex);
					socket.DisconnectEx(true);
					//						IsConnected = false;
					// 접속 끊고 잠깐 대기
					//						Thread.Sleep(2000);
				}
			}
			finally
			{
				_sync_event_recvDone.Set();
			}
		}


		public override int? SendSync(string strData, System.Text.Encoding encoding)
		{
			// Convert the string data to byte data using User Set (ASCII, UTF8, Unicode) encoding.  
			var bytesArray = encoding.GetBytes(strData);
			return SendSync(bytesArray, bytesArray.Length);
		}

		public override int? SendSync(byte[] byteData, int bytes)
		{
			if (false == IsConnected || null == _socketObject)
			{
				_logger?.Debug($"SOCK|CLT|{IpAddress}:{Port}|SendSync(...) IsConnected {IsConnected}");
				return null;
			}

			int? sentbytes = null;
			try
			{
				sentbytes = _socketObject.Socket.SendSync(byteData, Math.Min(byteData.Length, bytes));
			}
			catch (System.Exception ex)
			{
				_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
			}
			return sentbytes;
		}

		public override void SendAsync(string strData, System.Text.Encoding encoding)
		{
			// Convert the string data to byte data using User Set (ASCII, UTF8, Unicode) encoding.  
			var bytesArray = encoding.GetBytes(strData);
			SendAsync(bytesArray, bytesArray.Length);
		}

		public override void SendAsync(byte[] byteData, int bytes)
		{
			try
			{
				if (false == IsConnected || null == _socketObject)
					return;

				// Begin sending the data to the remote device.  
				_socketObject.Socket.BeginSend(byteData, 0, Math.Min(byteData.Length, bytes), 0, new AsyncCallback(ar => //(IAsyncResult ar) =>
				{
					try
					{
						// Retrieve the socket from the state object.  
						var socket = ar.AsyncState as Socket;
						var bytesSent = socket?.EndSend(ar);
					}
					catch (System.Exception ex)
					{
						_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
					}
					finally
					{
						_sync_event_sendDone.Set();
					}
				}), _socketObject.Socket);
			}
			catch (System.Exception ex)
			{
				_logger?.Debug(ex, $"SOCK|CLT|{IpAddress}:{Port}|Exception");
			}
		}

	}
}
