using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
namespace ConsoleDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GetDimensionOrder(1);
        }
        public static UserInfo GetDimensionOrder(int ID)
        {
            Database db = Database.GetDatabase("test");
            //UserInfo result = new UserInfo();
            UserInfo result= SafeProcedure.ExecuteAndGetInstance
                       (db, "dbo.GetUserInfo",
                        parameters =>
                        {
                            parameters.AddWithValue("@ID", ID.ToString());
                        },
                        (IRecord record, UserInfo item) =>
                        {
                            item.ID = record.GetOrDefault<int>("ID",0);
                            item.UserName = record.GetOrDefault<string>("Name", string.Empty);
                            item.Password = record.GetOrDefault<string>("Password", string.Empty);
                        });
                         
            return result;
        }
    }
    public class UserInfo
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
