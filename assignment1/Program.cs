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
           var s =  File.ReadAllLines("./data/userItem.data");

           var parsedRow = s.Select(line => {
                var values = line.Split(',');
                var id = values.First();
                var key = int.Parse(values[1]);
                var value = double.Parse(values.Last(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                return new { id = id, key = key, value = value };
           });   


           var usersToValues = parsedRow.Select(current => current.id).Distinct().ToDictionary(k => k, v => new Dictionary<int,double>());
        
           foreach (var item in parsedRow)
           {
               usersToValues[item.id].Add(item.key, item.value);
           }

 
           foreach (var item in usersToValues)
           {
               Console.WriteLine("ID: {0}", item.Key);
               foreach (var item1 in item.Value){
                   Console.WriteLine(" Key " + item1.Key + " Value " + item1.Value);
               }
           }
        }
    }
}
