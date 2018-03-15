using BidSoftware.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.EntityFrameworkEnvironment.Tables
{
    class ItemTable
    {
        internal static IEnumerable<Shared.DTODefinition.IDtoObject> GetTableItemPage(int startRecord, int numRecords)
        {
            try
            {
                int idx = 0;
                LinkedList<BidSoftware.Shared.DTODefinition.Item> li = new LinkedList<BidSoftware.Shared.DTODefinition.Item>();
                using (var ctx = new SI2_Entities())
                {
                    foreach (var item in ctx.vItems.Where(p => (p.ROW_NR >= startRecord) && (p.ROW_NR <= startRecord + numRecords)))
                    {
                        BidSoftware.Shared.DTODefinition.Item it = new BidSoftware.Shared.DTODefinition.Item();
                        it.ItemId = item.ItemId;
                        it.Description = item.Description;
                        it.ItemConditionId = item.ItemConditionId;
                        it.UserEmail = item.UserEmail;
                        li.AddLast(it);
                    }
                }
                return li;
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        internal static void AddItemRecord(Shared.DTODefinition.IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.Item rec = (BidSoftware.Shared.DTODefinition.Item)record;
                ctx.sp_addItem(rec.Description, rec.ItemConditionId, rec.UserEmail);
            }
        }

        internal static void UpdateItemRecord(Shared.DTODefinition.IDtoObject record)
        {
            BidSoftware.Shared.DTODefinition.Item i = (BidSoftware.Shared.DTODefinition.Item)record;
            using (var ctx = new SI2_Entities())
            {
                i = (BidSoftware.Shared.DTODefinition.Item)record;
                ctx.sp_editItem(i.ItemId, i.Description, i.ItemConditionId, i.UserEmail);
            }
        }

        internal static void DeleteItemRecord(Shared.DTODefinition.IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.Item i = (BidSoftware.Shared.DTODefinition.Item)record;
                ctx.sp_deleteItem(i.ItemId);
            }
        }

        internal static Shared.DTODefinition.IDtoObject GetItemRecord(object[] keys)
        {
            Shared.DTODefinition.Item item;
            using (var ctx = new SI2_Entities())
            {
                item = new Shared.DTODefinition.Item();
                int itemid = (int)keys[0];
                var auxitem = ctx.vItems.Where(p => p.ItemId == itemid).SingleOrDefault();

                item.ItemId = auxitem.ItemId;
                item.Description = auxitem.Description;
                item.ItemConditionId = auxitem.ItemConditionId;
                item.UserEmail = auxitem.UserEmail;
            }
            return item;
        }
    }
}
