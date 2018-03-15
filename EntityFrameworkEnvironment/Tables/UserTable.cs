using BidSoftware.ConnectedEnvironment.Tables;
using BidSoftware.Shared;
using BidSoftware.Shared.DTODefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.EntityFrameworkEnvironment.Tables
{
    class UserTable
    {
        public static IEnumerable<IDtoObject> GetTableUserPage(int startRecord, int numRecords)
        {
            try
            {
                LinkedList<BidSoftware.Shared.DTODefinition.User> li = new LinkedList<BidSoftware.Shared.DTODefinition.User>();
                BidSoftware.Shared.DTODefinition.User u;
                using (var ctx = new SI2_Entities())
                {
                    foreach (var user in ctx.vUsers.Where(p => (p.ROW_NR >= startRecord) && (p.ROW_NR <= startRecord+numRecords)))
                    {
                        u = new BidSoftware.Shared.DTODefinition.User();
                        u.Address = user.Address;
                        u.CountryNum = user.CountryNum;
                        u.Email = user.Email;
                        u.Name = user.Name;
                        u.Password = ConvertUtils.GetByteString((byte[])user.Password);
                        li.AddLast(u);
                    }
                }
                return li;
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }

        }


        internal static void AddUserRecord(IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.User rec = (BidSoftware.Shared.DTODefinition.User)record;
                ctx.sp_addUser(rec.Email, rec.Name, rec.Address, rec.Password, rec.CountryNum);
            }
        }

        internal static void UpdateUserRecord(IDtoObject record)
        {
            BidSoftware.Shared.DTODefinition.User u = (BidSoftware.Shared.DTODefinition.User)record;
            using (var ctx = new SI2_Entities())
            {
                u = (BidSoftware.Shared.DTODefinition.User)record;
                ctx.sp_editUser(u.Email, u.Name, u.Address, u.Password, u.CountryNum);
            }
        }

        internal static void DeleteUserRecord(IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.User u = (BidSoftware.Shared.DTODefinition.User)record;
                ctx.sp_deleteUser(u.Email);
            }
        }

        internal static IDtoObject GetUserRecord(object[] keys)
        {
            Shared.DTODefinition.User user;
            using (var ctx = new SI2_Entities())
            {
                user = new Shared.DTODefinition.User();
                String email = (String)keys[0];
                var auxuser = ctx.vUsers.Where(p => p.Email == email).SingleOrDefault();
                user.Email = auxuser.Email;
                user.Address = auxuser.Address;
                user.CountryNum = auxuser.CountryNum;
                user.Name = auxuser.Name;
                user.Password = ConvertUtils.GetByteString((byte[])auxuser.Password);

            }
            return user;
        }
    }
}
