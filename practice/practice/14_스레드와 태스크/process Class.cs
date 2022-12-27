using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace practice._14_스레드와_태스크
{
    internal class process_Class
    {
        /*
         * 프로세스와 스레드
         * 프로세스(Process)
         *      실행파일의 데이터와 코드가 메모리에 적재되어 동작하는 것
         *      word.exe가 실행파일 이라면, 이 실행 파일을 실행한 것이 프로세스
         * 스레드(Thread)
         *      스레드는 운영체제가 CPU 시간을 할당하는 기본단위
         *      프로레스가 밧줄이라면 스레드는 밧줄을 이루는 실
         */
        public void main()
        {
            Thread th = new Thread(new ThreadStart(DoSomthing));
            th.Start();
            th.Abort(); //스레드 멈추기 : 강제종료
            th.Interrupt(); //스레드 멈추기 : WaitJoinSleep 상테에 진입했을 때 ThreadInterruptedException예외를 일으켜 스레드를 중단시킴
            th.Join();  //스레드 종료 대기
        }
        public void DoSomthing()
        {
            try
            {
                for (int i = 0; i < 10000; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(10);
                }

            }
            catch (ThreadAbortException e)
            {
                // Abort
            }
            catch (ThreadInterruptedException e)
            {
                // Interrupt
            }
        }
        #region 스레드
        /* 
         * 스레드의 상태변화
         * Unstarted : 스레드 생성 직후
         * Running : 실행중
         * WaitSleepJoin : 블록된 상태
         * Suspend : 일시 중단 상태
         * Aborted : 취소상태
         * Stopped : 정지상태
         */

        /*
         * 스레드간의 동기화
         * Lock 키워드
         * Monitor 클래스
         */

        readonly object thisLock = new object();
        public void LockIncrease()
        {
            int loopCount = 1000;
            int count = 0;
            while (loopCount-- > 0)
            {
                lock (thisLock)
                {
                    count++;
                }
            }
        }

        public void MonitorIncrease()
        {
            int loopCount = 1000;
            int count = 0;
            while (loopCount-- > 0)
            {
                Monitor.Enter(thisLock);
                try
                {
                    count++;
                }
                finally
                {
                    Monitor.Exit(thisLock);

                }
            }
        }
        #endregion

        #region 테스크
        /*
         * System.Threading.Tasks.Task 클래스
         *      Action : 대리자를 실행
         *      Start() 메소드 : Action 대리자 비동기 실행
         *      Factory.StartNew() 메소드 : Task 객체생성 및 Action 대리자 비동기 실행
         *      Wait() 메소드 : Action 대리자 실행 완료 대기
         */
        void Func()
        {
            var myTask = Task.Factory.StartNew(
                () =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("테스크");
                }
                );

            Console.WriteLine("펑션");
            myTask.Wait();

            var myTask_ = Task<List<int>>.Factory.StartNew(
               () =>
               {
                   Thread.Sleep(1000);
                   List<int> list = new List<int>();
                   list.Add(0);
                   list.Add(1);
                   list.Add(2);
                   return list;
               }
               );
            var myList = new List<int>();
            myList.Add(5);
            myList.Add(6);
            myList.Add(7);
            myTask_.Wait();
            myList.AddRange(myTask_.Result.ToArray());


        }
        #endregion

        #region async와 await
        /*
         * async 한정자
         *      async 한정자는 메소드, 이벤트 처리기, 태스크, 람다식 등을 수식함
         *      C# 컴파일러가 async 한정자로 수식한 코드의 호출자를 만날 때 호출결과를 기다리지 않고 바로 다음
         *      코드로 이동하도록 실행코드를 생성
         * async로 한정하는 메소든는 반환 형식이 Task나 Task<TResult>, 또는 void 여야함
         *      실행하고 잊어버릴(Shoot and Forget)작업을 담고 있는 메소드라면 반환형식을 void로 선언
         *      작업이 완료될 때까지 기다리는 메소드라면 Task, Task<TResult>로 선언
         */
        void Caller()
        {
            Console.WriteLine("승");        // 1
            MyMethodAsync();
            Console.WriteLine("환");        // 3
        }
        async void MyMethodAsync()
        {
            Console.WriteLine("1");         // 2
            await Task.Run(async () =>
            {
                Console.WriteLine("k");     // 3
                Console.WriteLine("j");
            });
            Console.WriteLine("2");
        }

        // Task 우선순위 지정해 주는 예문
        void ex()
        {
            //// https://github.com/moljac/HolisticWare.WebSite.Notes/blob/master/dotnet-netfx/csharp/async-await/task-run-vs-task-factory-startnew.1.md
            //// Task.Run과 Task.Factory.StartNew의 차이 설명

            //Task t = Task.Run(() => Processing(matchData, List_PID_DATA, List_GAP_DATA, List_PITCH_DATA));
            //Task ts = Task.Factory.StartNew(() => Processing(matchData, List_PID_DATA, List_GAP_DATA, List_PITCH_DATA));
            /*Task.Factory.StartNew(() => Processing(matchData, List_PID_DATA, List_GAP_DATA, List_PITCH_DATA),
                                    CancellationToken.None, TaskCreationOptions.None, PriorityScheduler.Highest);
            */
        }
        #endregion
    }

}
