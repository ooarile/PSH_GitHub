using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serial_protocol.Protocol
{
    internal class SocketClientProtocol
    {
        private string m_IP = "";
        private string m_Port = "";
        private string m_SocketName = "";
        private bool isConnect = false; 
        private AsyncCallback m_fnReceiveHandler;
        private AsyncCallback m_fnSendHandler;
        private object obj = new object();

        public delegate void DataGetEventHandler(object sender, object eventData);
        public DataGetEventHandler DataSendEvent;
        public delegate void ConnectStatusEventHandler(object sender, object eventData);
        public ConnectStatusEventHandler ConnectStatus;
        public delegate void StatusEventHandler(object sender, object eventData);
        public StatusEventHandler Status;

        public SocketClientProtocol(string Name, string IP, string Port)
        {
            m_SocketName = Name;
            m_IP = IP;
            m_Port = Port;

            // 비동기 작업에 사용될 대리자를 초기화합니다.
            m_fnReceiveHandler = new AsyncCallback(handleDataReceive);
            m_fnSendHandler = new AsyncCallback(handleDataSend);
        }
        public class AsyncObject
        {
            public Byte[] Buffer;
            public Socket WorkingSocket;
            public AsyncObject(Int32 bufferSize)
            {
                this.Buffer = new Byte[bufferSize];
            }
        }
        // 4096 바이트의 크기를 갖는 바이트 배열을 가진 AsyncObject 클래스 생성
        AsyncObject asyncObject = new AsyncObject(1024);

        public void SocketOpen()
        {
            //비동기 연결 및 비동기 Data Recieve
            //https://slaner.tistory.com/52

            Socket client = null;
            try
            {
                IPAddress address = IPAddress.Parse(m_IP);
                int port = Int32.Parse(m_Port);
                IPEndPoint ipep = new IPEndPoint(address, port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipep);
                isConnect = true;
                ConnectStatus(this, isConnect);
            }
            catch (System.Exception ex)
            {
                isConnect = false;
                ConnectStatus(this, isConnect);
            }

            if (isConnect)
            {
                // 작업 중인 소켓을 저장하기 위해 sockClient 할당
                asyncObject.WorkingSocket = client;
                // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
                asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, asyncObject.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, asyncObject);
                Console.WriteLine("연결 성공!");
            }
            else
            {
                Console.WriteLine("연결 실패!");
            }
        }
        public void Disconnect()
        {
            try
            {
                lock (obj)
                {
                    if (asyncObject.WorkingSocket is Socket)
                    {
                        //if (asyncObject.WorkingSocket.Connected)
                            //asyncObject.WorkingSocket.Disconnect(true);
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

            //th_ConnectionStatus.Abort();
            //RecThread.Abort();
            //QueThread.Abort();

        }

        private void handleDataReceive(IAsyncResult ar)
        {
            string data;
            // 넘겨진 추가 정보를 가져옵니다.
            // AsyncState 속성의 자료형은 Object 형식이기 때문에 형 변환이 필요합니다~!
            AsyncObject ao = ar.AsyncState as AsyncObject;
            // 받은 바이트 수 저장할 변수 선언
            Int32 recvBytes =0;
            try
            {
                // 자료를 수신하고, 수신받은 바이트를 가져옵니다.
                recvBytes = ao.WorkingSocket.EndReceive(ar);
            }
            catch
            {
                // 예외가 발생하면 함수 종료!
                Disconnect();
                return;
            }

            // 수신받은 자료의 크기가 1 이상일 때에만 자료 처리
            if (recvBytes > 0 )
            {
                // 공백 문자들이 많이 발생할 수 있으므로, 받은 바이트 수 만큼 배열을 선언하고 복사한다.
                Byte[] msgByte = new Byte[recvBytes];
                Array.Copy(ao.Buffer, msgByte, recvBytes);

                // 받은 메세지를 출력
                Console.WriteLine("메세지 받음: {0}", Encoding.ASCII.GetString(msgByte));
                
                data = Encoding.ASCII.GetString(msgByte);
                DataSendEvent(this, data);
                //// 메시지 공백(\0)을 제거
                //sb.Append(data.Trim('\0'));
                //if (sb.Length != 0)
                //{
                //    DataSendEvent(this, sb.ToString());

                //    // 버퍼 초기화
                //    sb.Length = 0;
                //}
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

                Disconnect();
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
            // 넘겨진 추가 정보를 가져옵니다.
            AsyncObject ao = ar.AsyncState as AsyncObject;
            // 보낸 바이트 수를 저장할 변수 선언
            Int32 sentBytes;
            try
            {
                // 자료를 전송하고, 전송한 바이트를 가져옵니다.
                sentBytes = ao.WorkingSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                Console.WriteLine("자료 송신 도중 오류 발생! 메세지: {0}", ex.Message);

                Disconnect();
                return;
            }

            if (sentBytes > 0)
            {
                // 여기도 마찬가지로 보낸 바이트 수 만큼 배열 선언 후 복사한다.
                Byte[] msgByte = new Byte[sentBytes];
                Array.Copy(ao.Buffer, msgByte, sentBytes);

                Console.WriteLine("메세지 보냄: {0}", Encoding.ASCII.GetString(msgByte));
                //ao.WorkingSocket.Send(msgByte);
            }
        }
       

    }
}
