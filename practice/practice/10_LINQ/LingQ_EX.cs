using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._10_LINQ
{
    public class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }
    public class ArrProduct
    {
        public string Product { get; set; }
        public string Star { get; set; }
    }


    internal class LingQ_EX
    {
        List<int> list = new List<int>();
        Dictionary<int, string> ht = new Dictionary<int, string>();
        Profile[] arrProfile =
        {
            new Profile(){Name="정우성",Height=186 },
            new Profile(){Name="김태희",Height=156 },
            new Profile(){Name="고현정",Height=172 },
            new Profile(){Name="하정우",Height=160 },
            new Profile(){Name="이문세",Height=182 }
        };
        ArrProduct[] arrProduct =
        {
            new ArrProduct(){Product="비트",Star="정우성"},
            new ArrProduct(){Product="CF 다수",Star="김태희"},
            new ArrProduct(){Product="아이리스",Star="김태희"},
            new ArrProduct(){Product="모래시계",Star="고현정"},
            new ArrProduct(){Product="Solo예찬",Star="이문세"}
        };

        public void func()
        {/*
            ht.Add(1, "A");
            ht.Add(2, "Bcbds");
            ht.Add(3, "Csadfxczv");
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);*/

            var a = from profile in arrProfile
                    where profile.Height < 180
                    select new { Name = profile.Name, InchHeight = profile.Height * 0.393 };


            #region group by 절
            var listProfile = from profile in arrProfile
                              group profile by profile.Height < 180 into g
                              select new { GroupKey = g.Key, Profiles = g };

            foreach (var i in listProfile)
            {
                Console.WriteLine(i.Profiles);
                foreach (var j in i.Profiles)
                {

                    Console.WriteLine(j.Name);
                }
            }
            #endregion

            #region 내부 join 절 // 공통된 내용만 출력
            var listProfile_ = from profile in arrProfile
                               join product in arrProduct
                               on profile.Name equals product.Star
                               select new { Name = profile.Name, Work = product.Product, Height = profile.Height };

            foreach (var i in listProfile_)
            {
                Console.WriteLine("{0},{1},{2}", i.Name, i.Work, i.Height);
            }
            #endregion

            #region 외부 join 절 // 기준 내용으로 정리하여 출력
            //orderby절 : ascending(오름차순), descending(내림차순)
            var listProfile__ = from profile in arrProfile
                                join product in arrProduct
                                on profile.Name equals product.Star into ps
                                from sub_product in ps.DefaultIfEmpty(new ArrProduct() { Product = "없음" })
                                orderby profile.Height descending
                                select new { Name = profile.Name, Work = sub_product.Product, Height = profile.Height };

            foreach (var i in listProfile__)
            {
                Console.WriteLine("{0},{1},{2}", i.Name, i.Work, i.Height);

            }
            #endregion

        }
    }
}
