using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serial_protocol.Protocol.AsyncSocket
{
    static class SocketExtensions
	{
		public static void DisconnectEx(this Socket s, bool reuse = false)
		{
			try
			{
				// System.Threading.Thread.Sleep(1);
				try
				{
					if (null != s)
						s?.Shutdown(SocketShutdown.Both);
				}
				catch (System.Exception)
				{
					//	Logger("AsyncSockClient.Start : Clear Socket...fail Step2", ex); }
				}
				// System.Threading.Thread.Sleep(1);
				try
				{
					if (null != s && s.Connected)
						s?.Disconnect(reuse);
					// 아래 조건에서도 System.NullReferenceException: '개체 참조가 개체의 인스턴스로 설정되지 않았습니다.' 발생...
					// 	if (null != s)
					// 		s?.Disconnect(reuse);
				}
				catch (System.Exception)
				{
					// Logger("AsyncSockClient.Start : Clear Socket...fail Step3", ex); }
				}
				// System.Threading.Thread.Sleep(1);
			}
			catch (System.Exception/*ex*/)
			{
				//             return false;
			}
		}

		public static void Discard(this Socket s)
		{
			try
			{
				// System.Threading.Thread.Sleep(1);
				try { if (null != s) s.Shutdown(SocketShutdown.Both); } catch (System.Exception) { }    // System.Threading.Thread.Sleep(1);
				try { if (null != s && s.Connected) s.Disconnect(false); } catch (System.Exception) { }         // Logger("AsyncSockClient.Start : Clear Socket...fail Step3", ex); }			// System.Threading.Thread.Sleep(1);
				try { if (null != s) s.Close(); } catch (System.Exception) { }                          //{ Logger("AsyncSockClient.Start : Clear Socket...fail Step4", ex); }			//	System.Threading.Thread.Sleep(1);
				try { if (null != s) s.Dispose(); } catch (System.Exception) { }                        // { Logger("AsyncSockClient.Start : Clear Socket...fail Step5", ex); }			//	System.Threading.Thread.Sleep(1);
				s = null;
			}
			catch (System.Exception/*ex*/)
			{
				//             return false;
			}
		}


		public static int SendSync(this Socket s, byte[] byteData)
		{
			return SendSync(s, byteData, byteData.Length);
		}

		public static int SendSync(this Socket s, byte[] byteData, int bytes)
		{
			try
			{
				// 			if (false == socket.IsConnected())
				return s.Send(byteData, bytes, System.Net.Sockets.SocketFlags.None);
			}
			catch (SocketException)
			{
				return 0;
			}
		}


		public static int SendAsync(this Socket s, byte[] byteData)
		{
			return SendAsync(s, byteData, byteData.Length);
		}

		private static int SendAsync(this Socket s, byte[] byteData, int bytes)
		{
			// Begin sending the data to the remote device.  
			s.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(ar => //(IAsyncResult ar) =>
			{
				try
				{
					// Retrieve the socket from the state object.  
					var socket = ar.AsyncState as Socket;

					// Complete sending the data to the remote device.  
					var bytesSent = socket?.EndSend(ar);
				}
				catch (System.Exception /*ex*/)
				{
				}
			}), s);

			return bytes;

		}

		// Keepalive 설정함수
		// 	1) Time(TCP 연결유지시간) :10초
		// 	2) Interval(TCP 연결유지간격) :1초 -> 1000
		public static int SetKeepAlive(this Socket s, bool On_Off, uint KeepaLiveTime, uint KeepaLiveInterval)
		{
			int Result = -1;
			// 		unsafe
			{
				byte[] tcpKeepAliveSetting = new byte[12];
				// 0~4 On_Off // Enable
				byte[] OnOff = BitConverter.GetBytes(Convert.ToUInt32(On_Off));
				Array.Copy(OnOff, 0, tcpKeepAliveSetting, 0, 4);
				// Keepalive Time
				byte[] aliveTime = BitConverter.GetBytes(KeepaLiveTime);
				Array.Copy(aliveTime, 0, tcpKeepAliveSetting, 4, 4);
				// Keepalive Interval
				byte[] aliveInterval = BitConverter.GetBytes(KeepaLiveInterval);
				Array.Copy(aliveInterval, 0, tcpKeepAliveSetting, 8, 4);
				Result = s.IOControl(System.Net.Sockets.IOControlCode.KeepAliveValues, tcpKeepAliveSetting, null);
			}
			return Result;
		}
	}
}
