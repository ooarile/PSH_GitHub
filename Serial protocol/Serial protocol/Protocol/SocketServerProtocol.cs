using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace Serial_protocol.Protocol
{
    internal class SocketServerProtocol
    {
        private Socket server;
        private string m_IP = "";
        private string m_Port = "";
        private string m_SocketName = "";
        private int port;
        private ConcurrentQueue<string> Que_recStr = new ConcurrentQueue<string>();
        private object obj = new object();

        public delegate void DataGetEventHandler(object sender, object eventData);
        public DataGetEventHandler DataSendEvent;
        public delegate void ConnectStatusEventHandler(object sender, object eventData);
        public ConnectStatusEventHandler ConnectStatus;
        public delegate void StatusEventHandler(object sender, object eventData);
        public StatusEventHandler Status;


        public SocketServerProtocol(string Name, string IP, string Port)
        {
            m_SocketName = Name;
            m_IP = IP;
            m_Port = Port;
            m_fnReceiveHandler = new AsyncCallback(RecieveSocket);
            m_fnAcceptHandler = new AsyncCallback(AcceptCallback);
            m_fnSendHandler = new AsyncCallback(handleDataSend);
        }

        private AsyncCallback m_fnReceiveHandler;
        private AsyncCallback m_fnSendHandler;
        private AsyncCallback m_fnAcceptHandler;
        public void SocketOpen()
        {
            //비동기 연결 및 비동기 Data Recieve
            //https://slaner.tistory.com/40

            try
            {
                #region Server
                port = Int32.Parse(m_Port); 
                // Socket EndPoint 설정(서버의 경우는 Any로 설정하고 포트 번호만 설정한다.)
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any/*IPAddress.Parse("192.168.1.222")*/, port);
                // 소켓 인스턴스 생성
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //// LigerState는 socket.close 됐을때, 전송 못보낸 Data를 몇초동안 더 보낼것인지 설정하는 것
                // 여기서 시간은 0이므로 그냥 "바로 종료" 
                server.LingerState = new LingerOption(true, 0);
                //// keepAlive 기능 적용되지 않는 것 같음...
                //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                // Keepalive 설정적용 
                SetKeepAliveValues(server, true, 10000, 1000);
                // 서버 소켓에 EndPoint 설정
                server.Bind(ipep);
                // 클라이언트 소켓 대기 버퍼
                server.Listen(/*백로그 큐 갯수*/20);
                // 콘솔 출력
                Console.WriteLine($"Server Start... port {ipep.Port}...");
                Status(this,$"Server Start... port {ipep.Port}...");
                server.BeginAccept(m_fnAcceptHandler, server);
                #endregion 
            }
            catch (System.Exception ex)
            {

            }
        }

        // Keepalive 설정함수
        // 	1) Time(TCP 연결유지시간) :10초
        // 	2) Interval(TCP 연결유지간격) :1초 -> 1000
        protected int SetKeepAliveValues(System.Net.Sockets.Socket Socket, bool On_Off, uint KeepaLiveTime, uint KeepaLiveInterval)
        {
            int Result = -1;
            //unsafe
            //{
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
                Result = Socket.IOControl(System.Net.Sockets.IOControlCode.KeepAliveValues, tcpKeepAliveSetting, null);
            //}
            return Result;
        }

        private class AsyncObject
        {
            public Byte[] Buffer;
            public Socket WorkingSocket;
            public AsyncObject(Int32 bufferSize)
            {
                this.Buffer = new Byte[bufferSize];
            }
        }

        //// 4096 바이트의 크기를 갖는 바이트 배열을 가진 AsyncObject 클래스 생성
        AsyncObject asyncObject = new AsyncObject(1024);

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket ServerSocket = null;
            Socket clientSocket = null;
            try
            {
                if (ar.AsyncState is Socket)
                {
                    ServerSocket = ar.AsyncState as Socket;

                    ClientDisconnect();                    

                    // 클라이언트로부터 접속 대기
                    clientSocket = ServerSocket.EndAccept(ar);
                    // 서버에 접속한 클라이언트 EndPoint 정보 취득
                    var ip = clientSocket.RemoteEndPoint as IPEndPoint;
                    // 콘솔 출력 - 접속 ip와 접속 시간
                    Console.WriteLine($"Client : (From: {ip.Address.ToString()}:{ip.Port}, Connection time: {DateTime.Now})");
                    Status(this, $"Client (From: {ip.Address.ToString()}:{ip.Port})");
                    ConnectStatus(this, true);

                    // 작업 중인 소켓을 저장하기 위해 sockClient 할당
                    asyncObject.WorkingSocket = clientSocket;
                    // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
                    clientSocket.BeginReceive(asyncObject.Buffer, 0, asyncObject.Buffer.Length, 
                                                SocketFlags.None, m_fnReceiveHandler, asyncObject);

                    if (ServerSocket != null)
                    {
                        // 또 다른 클라이언트의 연결을 대기한다.
                        ServerSocket.BeginAccept(m_fnAcceptHandler, ServerSocket);
                    }
                }

            }
            catch (System.Exception ex)
            {
                ClientDisconnect();
            }

        }



        //private void timer_Reconnect(object sender, ElapsedEventArgs e)
        //{
        //    if (!aply && client == null)
        //    {
        //        // 클라이언트로부터 접속 대기
        //        client = server.Accept();
        //        // 클라이언트 EndPoint 정보 취득
        //        var ip = client.RemoteEndPoint as IPEndPoint;
        //        // 콘솔 출력 - 접속 ip와 접속 시간
        //        Console.WriteLine($"Client : (From: {ip.Address.ToString()}:{ip.Port}, Connection time: {DateTime.Now})");
        //        ConnectStatusString(this, ip.Address.ToString(), ip.Port.ToString(), true);
        //    }
        //}



        public void RecieveSocket(IAsyncResult client)
        {
            string data;
            Int32 recvBytes=0;
            // 넘겨진 추가 정보를 가져옵니다.
            // AsyncState 속성의 자료형은 Object 형식이기 때문에 형 변환이 필요합니다~!
            AsyncObject ao = client.AsyncState as AsyncObject;
            try
            {
                // 자료를 수신하고, 수신받은 바이트를 가져옵니다.
                recvBytes = ao.WorkingSocket.EndReceive(client);
            }
            catch (Exception ex)
            {
                ClientDisconnect();
                return;
            }

            // 수신받은 자료의 크기가 1 이상일 때에만 자료 처리
            if (recvBytes > 0)
            {
                Byte[] msgByte = new Byte[recvBytes];
                Array.Copy(ao.Buffer, msgByte, recvBytes);


                // 받은 메세지를 출력
                Console.WriteLine("메세지 받음: {0}", Encoding.ASCII.GetString(msgByte));
                data = Encoding.ASCII.GetString(msgByte);
                DataSendEvent(this, data);
                //data = Encoding.ASCII.GetString(ao.Buffer);
                //// 메시지 공백(\0)을 제거
                //sb.Append(data.Trim('\0'));
                //if (sb.Length != 0)
                //{
                //    DataSendEvent(this, sb.ToString());

                //    // 버퍼 초기화
                //    sb.Length = 0;
                //}
            }
            else  //recvBytes가 0이면 끊긴것으로 간주
            {
                ClientDisconnect();
                return;
            }
            try
            {
                // 자료 처리가 끝났으면~
                // 이제 다시 데이터를 수신받기 위해서 수신 대기를 해야 합니다.
                // Begin~~ 메서드를 이용해 비동기적으로 작업을 대기했다면
                // 반드시 대리자 함수에서 End~~ 메서드를 이용해 비동기 작업이 끝났다고 알려줘야 합니다!
                ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                Console.WriteLine("자료 수신 대기 도중 오류 발생! 메세지: {0}", ex.Message);

                ClientDisconnect();
                return;
            }
        }
        public void SendMessage(string data)
        {
            // 추가 정보를 넘기기 위한 변수 선언
            // 크기를 설정하는게 의미가 없습니다.
            // 왜냐하면 바로 밑의 코드에서 문자열을 유니코드 형으로 변환한 바이트 배열을 반환하기 때문에
            // 최소한의 크기르 배열을 초기화합니다.
            AsyncObject ao = new AsyncObject(1);

            // 문자열을 바이트 배열으로 변환
            ao.Buffer = Encoding.ASCII.GetBytes(data);
            if (asyncObject.WorkingSocket is Socket)
            {
                // 사용된 소켓을 저장
                ao.WorkingSocket = asyncObject.WorkingSocket;
                // 전송 시작!
                asyncObject.WorkingSocket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnSendHandler, ao);
            }            
        }
        private void handleDataSend(IAsyncResult ar)
        {
            Socket client;
            Int32 sentBytes; 
            try
            {
                // 넘겨진 추가 정보를 가져옵니다.
                AsyncObject ao = ar.AsyncState as AsyncObject;
                if (ao.WorkingSocket is Socket)
                {
                    client = asyncObject.WorkingSocket;
                    sentBytes = client.EndSend(ar);

                    if (sentBytes > 0)
                    {
                        //// <응답>클라이언트로 메시지 송신
                        //client.Send(ao.Buffer);
                        string str = Encoding.ASCII.GetString(ao.Buffer);
                        Console.WriteLine("메세지 보냄: {0}", str);
                    }
                }

            }
            catch (System.Exception ex)
            {
                ClientDisconnect();
            }

        }

            public void Disconnect()
        {
            try
            {
                lock (obj)
                {
                    if (server != null)
                    {
                        //if (server.Connected)
                        //    server.Disconnect(true);

                        server.Close();
                        server = null;
                        ConnectStatus(this, false);
                    }
                    
                }

                ClientDisconnect();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            //th_ConnectionStatus.Abort();
            //RecThread.Abort();
            //QueThread.Abort();

        }
        public void ClientDisconnect()
        {
            try
            {
                lock (obj)
                {
                    if (asyncObject.WorkingSocket is Socket)
                    {
                        //if (ClientSocket.Connected)
                        //    ClientSocket.Disconnect(true);
                        asyncObject.WorkingSocket.Close();
                        asyncObject.WorkingSocket = null;

                        ConnectStatus(this, false);

                    }
                }
               
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public string SocketName
        {
            get { return m_SocketName; }
        }

    }
}
