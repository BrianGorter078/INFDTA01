using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
           var s =  File.ReadLines("./data/userItem.data");

           Dictionary<string,Dictionary<int, double>> users = new Dictionary<string,Dictionary<int, double>>();


           for (int i = 0; i < s.Count(); i++)
           {
               var userid = s.ElementAt(i).Split(',').First();
               var key = int.Parse(s.ElementAt(i).Split(',').ElementAt(1));
               var value = double.Parse(s.ElementAt(i).Split(',').Last(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

               if(users.Count == 0){
                   users.Add(userid,new Dictionary<int,double>());
                   users[userid].Add(key,value);
               }else{
                    if(users.ContainsKey(userid)){
                        users[userid].Add(key,value);
                    }
                    else{
                        users.Add(userid,new Dictionary<int,double>());
                        users[userid].Add(key,value);
                    }
                }
           }
           foreach (var item in users)
           {
               foreach (var item1 in item.Value){
                   Console.WriteLine(" id " +item.Key +  " Key " + item1.Key + " Value " + item1.Value);
               }
           }
        }
    }
}
