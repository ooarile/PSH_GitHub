// MSDN 비동기 클라이언트 소켓 예제 수정
// https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/asynchronous-client-socket-example

using System;
using System.Net.Sockets;
using System.Text;

namespace Serial_protocol.Protocol.AsyncSocket
{
    internal class IocpEventArgs : EventArgs
	{
		public IocpEventArgs(int nodeID, Socket sock, int bufbytes = 65536) 
		{ 
			this.NodeID = nodeID;
			this.Socket = sock;
			Buffer = new byte[bufbytes];
		}

		// identifier of each socket/packet
		public int NodeID { get; set; } = -1;

		// Socket.  
		public Socket Socket { get; private set; } = null;

		// Temporary Receive buffer.
		public byte[] Buffer = null;

		// Size of receive buffer.  
		public int RecvBytes { get; set; } = 0;

		public object Locker { get; set; } = new object();
		public StringBuilder sBuilder { get; set; } = new StringBuilder();

		// 소켓 close 처리 플래그
		public bool FlagForceDisconnect { get; set; } = false;
	}
}
