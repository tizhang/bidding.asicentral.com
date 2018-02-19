using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class UserManager
    {
        public static User GetUser(int id)
        {
            Data.User user = null;
            using (var context = new Data.BiddingContext())
            {
                user = context.Users.FirstOrDefault(i => i.UserId == id);
            }
            if (user == null) return null;
            var item = Mapper.Map<User>(user);
            return item;
        }

        public static User GetUser(string name)
        {
            Data.User user = null;
            using (var context = new Data.BiddingContext())
            {
                user = context.Users.FirstOrDefault(i => i.Name == name);
            }
            if (user == null) return null;
            var item = Mapper.Map<User>(user);
            return item;
        }

        public static List<string> MapAcessibleGroups(string OwnerGroups)
        {
            var retGroups = new List<string>();
            var groups = OwnerGroups.Split(',');
            foreach (var group in groups)
            {
                switch (group)
                {
                    case "DIST":
                        retGroups.Add("SPLR");
                        break;
                    case "SPLR":
                        retGroups.Add("DIST");
                        retGroups.Add("ADMG");
                        break;
                    case "ADMG":
                        retGroups.Add("SPLR");
                        retGroups.Add("DIST");
                        retGroups.Add("ADMG");
                        break;
                }
            }
            return retGroups.Distinct().ToList();
        }

    }
}
