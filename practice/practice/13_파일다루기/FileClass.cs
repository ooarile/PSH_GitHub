using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._13_파일다루기
{
    internal class FileClass
    {
        public void FileWrite()
        {
            using (StreamWriter sw = new StreamWriter(new FileStream("test1.txt", FileMode.Create)))
            {
                sw.WriteLine("hi");
                sw.WriteLine("Hello");
                sw.WriteLine("01");
            }

            FileStream fs = new FileStream("test1.txt", FileMode.Create);
            StreamWriter swr = new StreamWriter(fs);
            swr.WriteLine("hi");
            swr.WriteLine("Hello");
            swr.WriteLine("01");
            swr.Close();
        }



        public void FileRead()
        {
            int a; float b; string c;
            using (StreamReader sw = new StreamReader(new FileStream("test1.txt", FileMode.Open, FileAccess.Read)))
            {
                a = int.Parse(sw.ReadLine());
                b = float.Parse(sw.ReadLine());
                c = sw.ReadLine();
            }

            FileStream fs = new FileStream("test1.txt", FileMode.Open,FileAccess.Read);
            StreamReader swr = new StreamReader(fs);
            int value = int.Parse(swr.ReadLine());
            float fvalue = float.Parse(swr.ReadLine());
            string dvalue = swr.ReadLine();
            while(swr.EndOfStream==false)
            {
                Console.WriteLine(swr.ReadLine());
            }
            swr.Close();
            Console.WriteLine("{0} {1} {2}", value, fvalue, dvalue);
        }

        public void DirectoryRead()
        {
            string[] MappingDatafilePath;
            FileInfo MappingFileInfo;
            DateTime MappingFile_LastWrite_dtTime;
            int FileLineNum = 0, readLineNum = 0, ImageNum = 0; ;
            string strMappingData;

            // 해당폴더 .txt 파일들 "경로+이름"을 string 배열로 저장
            MappingDatafilePath = System.IO.Directory.GetFiles("..\\logs\\", "*.txt");  
            // 파일 개수 만큼 반복문
            for(int i=0; i<MappingDatafilePath.Length;i++)
            {
                //파일 정보 불러오기
                MappingFileInfo = new FileInfo(MappingDatafilePath[i]);     
                //파일의 수정한 날짜를 DateTime으로 가져오기
                MappingFile_LastWrite_dtTime = MappingFileInfo.LastWriteTime;
                //매핑 Data 정보의 txt 파일 마지막 Write시간이 n초가 안지났으면 Continue
                if (DateTime.Now < MappingFile_LastWrite_dtTime + TimeSpan.FromSeconds(5))                
                {
                    continue;
                }

                //파일 내부 줄 개수 파악
                FileLineNum = File.ReadAllLines(MappingDatafilePath[i]).Length;
                try
                {
                    using (StreamReader reader = new StreamReader(MappingDatafilePath[i]))
                    {
                        readLineNum = 0;

                        while ((strMappingData = reader.ReadLine()) != null)
                        {
                            readLineNum++;
                            Console.WriteLine("{0}", strMappingData);
                        }
                    }
                }
                catch (Exception ex)
                { 
                    //MappingData txt파일 삭제
                    File.Delete(MappingDatafilePath[i]);   
                    continue;
                }
            }
        }

    }
}
