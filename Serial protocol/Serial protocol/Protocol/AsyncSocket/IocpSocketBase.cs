// MSDN 비동기 클라이언트 소켓 예제 수정
// https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/asynchronous-client-socket-example


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial_protocol.Protocol.AsyncSocket
{
	public static class Helper
	{
		private static int[] HexValue = new int[]
		{
				0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C,
				0x0D, 0x0E, 0x0F
		};
		private static string HexAlphabet = "0123456789ABCDEF";

		public static string[] HexTbl = Enumerable.Range(0, 256).Select(v => v.ToString("X2")).ToArray();

		public static string ToHex(this IEnumerable<byte> array)
		{
			var s = new System.Text.StringBuilder();
			foreach (var v in array)
				s.Append(HexTbl[v]);
			return s.ToString();
		}

		public static string ToHex(this byte[] array, string saparator = "", int length = 0)
		{
			if (0 == length)
				length = array.Length;
			var sb = new System.Text.StringBuilder(length * (2 + saparator.Length));

			if ("" == saparator)
			{
				for (int i = 0; i < length; i++)
					sb.Append(HexTbl[array[i]]);
			}
			else
			{
				for (int i = 0; i < length; i++)
				{
					sb.Append(HexTbl[array[i]]);
					sb.Append(saparator);
				}
			}
			return sb.ToString();
		}

		public static string ByteArrayToHexString(this byte[] array, int length = 0)
		{
			if (0 == length)
				length = array.Length;

			var sb = new System.Text.StringBuilder(array.Length * 2);
			for (int i = 0; i < length; i++)
			{
				sb.Append(HexAlphabet[(int)(array[i] >> 4)]);
				sb.Append(HexAlphabet[(int)(array[i] & 0xF)]);
			}
			return sb.ToString();
		}

		public static byte[] HexStringToByteArray(this string Hex)
		{
			byte[] Bytes = new byte[Hex.Length / 2];
			for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
			{
				Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
								  HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
			}

			return Bytes;
		}
	}
	abstract class IocpSocketBase
    {
        #region /****** setup logger ******/
        protected static Serilog.ILogger _logger = null;
        protected static bool _write_raw_packet = true;
        public static void SetupLogger(bool writeRawPacket, bool syncLogging, bool writeToConsole, string path, int? retainDays = null/*unlimit*/)
        {
            _logger = Protocol.AsyncSocket.helper.Initialize(syncLogging, writeToConsole, path, retainDays);
            _write_raw_packet = writeRawPacket;
        }
        #endregion
        protected int _nodeID;
        protected string _myip;
        protected int _myport;
        protected string _dstip;
        protected int _dstport;
        public bool IsServer { get; private set; }

        // 데이터 수신 특정 이벤트 발생
        public event EventHandler<IocpEventArgs> EventConnected = null;
        public event EventHandler<IocpEventArgs> EventReceived = null;
        public void Setup(int nodeID, bool isServer, string myip, int myport, string dstip, int dstport/*, EventHandler<EventArgs> packetReceived*/)
        {
            _nodeID = nodeID;
            _myip = myip;
            _myport = myport;
            _dstip = dstip;
            _dstport = dstport;
            // Server 소켓을 만들어 Listen 하는가? 아니면  Client 소켓을 만들어 Connection 하는가?
            IsServer = isServer;
        }
        protected virtual void OnConnected(IocpEventArgs e) { EventConnected?.Invoke(this, e); }
        protected virtual void OnReceived(IocpEventArgs e) { EventReceived?.Invoke(this, e); }

        protected void MySleep(ref bool exit, int msec_sleep, int msec_interval = 50)
        {
            var start = Environment.TickCount;
            while (msec_sleep <= (Environment.TickCount - start))
            {
                if (true == exit)
                    break;
                System.Threading.Thread.Sleep(msec_interval);
            }
        }

        public abstract int? SendSync(string strData, System.Text.Encoding encoding);
        public abstract int? SendSync(byte[] byteData, int bytes);
        public abstract void SendAsync(string strData, System.Text.Encoding encoding);
        public abstract void SendAsync(byte[] byteData, int bytes);

    }
}
