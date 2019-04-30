using System;
using System.Collections.Generic;
using System.Linq;
using TimeLink.Models;

namespace TimeLink.Services
{
    public class SetDBData
    {
        public static void GenerateDBData()
        {
            SetDataToACCAUNT();
        }

        private static void SetDataToACCAUNT()
        {

            MyDataModel context = new MyDataModel();

            if (!context.T_ACCOUNT.Any())
            {
                List<T_ACCOUNT> states = new List<T_ACCOUNT>()
                {   new T_ACCOUNT(){Email = "test@test.ee", Password="qwe123*", Active=false},
                    new T_ACCOUNT(){Email = "test@test.com", Password = "qwe123*", Active=true},
                    new T_ACCOUNT(){Email = "administrator@timelink.com", Password = "qwe123*", Active=false},
                };

                context.T_ACCOUNT.AddRange(states);
                context.SaveChanges();
            }
        }
    }
}