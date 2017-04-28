using System;
using System.Threading.Tasks;
using Greatbone.Core;

namespace Greatbone.Sample
{
    [User]
    public abstract class ShopVarWork : Work
    {
        protected ShopVarWork(WorkContext wc) : base(wc)
        {
        }

        public void _icon_(ActionContext ac)
        {
            string shopid = ac[this];

            using (var dc = Service.NewDbContext())
            {
                if (dc.Query1("SELECT icon FROM shops WHERE id = @1", p => p.Set(shopid)))
                {
                    var byteas = dc.GetByteAs();
                    if (byteas.Count == 0) ac.Give(204); // no content 
                    else
                    {
                        StaticContent cont = new StaticContent(byteas);
                        ac.Give(200, cont);
                    }
                }
                else ac.Give(404); // not found           
            }
        }
    }

    public class PubShopVarWork : ShopVarWork
    {
        public PubShopVarWork(WorkContext wc) : base(wc)
        {
            CreateVar<PubItemVarWork, string>();
        }

        public void @default(ActionContext ac)
        {
            string shopid = ac[this];

            using (var dc = ac.NewDbContext())
            {
                // query for the shop record
                const int proj = -1 ^ Shop.BIN ^ Shop.TRANSF ^ Shop.SECRET;
                dc.Sql("SELECT ").columnlst(Shop.Empty, proj)._("FROM shops WHERE id = @1");
                if (dc.Query1(p => p.Set(shopid)))
                {
                    var shop = dc.ToObject<Shop>(proj);

                    // query for item records of the shop
                    Item[] items = null;
                    dc.Sql("SELECT ").columnlst(Item.Empty, proj)._("FROM items WHERE shopid = @1");
                    if (dc.Query(p => p.Set(shopid)))
                    {
                        items = dc.ToArray<Item>(proj);
                    }

                    ac.GivePage(200, m =>
                    {
                        m.Add("<div data-sticky-container>");
                        m.Add("<div class=\"sticky\" style=\"width: 100%\" data-sticky  data-options=\"anchor: page; marginTop: 0; stickyOn: small;\">");
                        m.Add("<div class=\"title-bar\">");
                        m.Add("<div class=\"title-bar-left\">");
                        m.Add("<a href=\"../\" onclick=\"return dialog(this, 2);\">");
                        m.Add(shop.name);
                        m.Add("</a>");
                        m.Add("</div>");
                        m.Add("<div class=\"title-bar-right\">");
                        m.Add("<a class=\"float-right\" href=\"/my//cart/\"><span class=\"fa-stack fa-lg\"><i class=\"fa fa-circle fa-stack-2x\"></i><i class=\"fa fa-shopping-cart fa-stack-1x fa-inverse\"></i></span></a>");
                        m.Add("</div>");
                        m.Add("</div>");
                        m.Add("</div>");
                        m.Add("</div>");

                        m.Add("<div>");
                        m.Add("<p>");
                        m.Add(shop.city);
                        m.Add(shop.addr);
                        m.Add("</p>");
                        m.Add("<p>");
                        m.Add(shop.descr);
                        m.Add("</p>");
                        m.Add("</div>");

                        // display items

                        if (items == null)
                        {
                            m.Add("没有上架商品");
                            return;
                        }
                        for (int i = 0; i < items.Length; i++)
                        {
                            Item item = items[i];
                            m.Add("<form id=\"item");
                            m.Add(i);
                            m.Add("\">");
                            m.Add("<div class=\"row\">");

                            m.Add("<div class=\"small-4 columns\"><a href=\"#\"><span></span><img src=\"");
                            m.Add(item.name);
                            m.Add("/_icon_\" alt=\"\" class=\" thumbnail\"></a></div>");
                            m.Add("<div class=\"small-8 columns\">");
                            m.Add("<p>&yen;");
                            m.Add(item.price);
                            m.Add("</p>");
                            m.Add("<p>");
                            m.Add(item.descr);
                            m.Add("</p>");

                            m.Add("<a class=\"button warning\" href=\"");
                            m.Add(item.name);
                            m.Add("/add\" onclick=\"return dialog(this,2)\">加入购物车</a>");
                            m.Add("</div>");

                            m.Add("</div>");
                            m.Add("</form>");
                        }
                    });
                }
                else
                {
                    ac.Give(404); // not found
                }
            }
        }
    }

    [Ui("设置")]
    public class OprShopVarWork : ShopVarWork
    {
        public OprShopVarWork(WorkContext wc) : base(wc)
        {
            Create<OprPaidOrderWork>("paid");

            Create<OprPackedOrderWork>("packed");

            Create<OprAbortedOrderWork>("aborted");

            Create<OprSentOrderWork>("assigned");

            Create<OprDoneOrderWork>("done");

            Create<OprItemWork>("item");

            Create<OprRepayWork>("repay");
        }

        public void @default(ActionContext ac)
        {
            ac.GiveFrame(200);
        }
    }

    [Ui("设置")]
    public class DvrShopVarWork : ShopVarWork
    {
        public DvrShopVarWork(WorkContext wc) : base(wc)
        {
            Create<DvrSentOrderWork>("assigned");

            Create<DvrDoneOrderWork>("done");
        }

        public void @default(ActionContext ac)
        {
            ac.GiveFrame(200);
        }
    }

    public class MgrShopVarWork : ShopVarWork
    {
        public MgrShopVarWork(WorkContext wc) : base(wc)
        {
        }

        [Ui("修改", UiMode.AnchorDialog)]
        public async Task edit(ActionContext ac)
        {
            if (ac.GET)
            {
                string id = ac[this];
                string city = ac[typeof(CityVarWork)];
                using (var dc = ac.NewDbContext())
                {
                    const int proj = -1 ^ Shop.BIN ^ Shop.PRIME;
                    dc.Sql("SELECT ").columnlst(Shop.Empty, proj)._("FROM shops WHERE id = @1 AND city = @2");
                    if (dc.Query1(p => p.Set(id).Set(city)))
                    {
                        ac.GiveFormPane(200, dc.ToObject<Item>(proj), proj);
                    }
                    else
                    {
                        ac.Give(500); // internal server error
                    }
                }
            }
            else // post
            {
                var shop = await ac.ReadObjectAsync<Shop>();
                shop.id = ac[this];
                using (var dc = ac.NewDbContext())
                {
                    const int proj = -1 ^ Shop.BIN;
                    dc.Sql("INSERT INTO shops")._(Shop.Empty, proj)._VALUES_(Shop.Empty, proj)._("");
                    if (dc.Execute(p => shop.WriteData(p, proj)) > 0)
                    {
                        ac.Give(201); // created
                    }
                    else
                    {
                        ac.Give(500); // internal server error
                    }
                }
            }
        }

        [Ui("图片", UiMode.AnchorCrop, Circle = true)]
        public async Task icon(ActionContext ac)
        {
            string id = ac[this];
            string city = ac[typeof(CityVarWork)];
            if (ac.GET)
            {
                using (var dc = Service.NewDbContext())
                {
                    if (dc.Query1("SELECT icon FROM shops WHERE id = @1 AND name = @2", p => p.Set(id).Set(city)))
                    {
                        var byteas = dc.GetByteAs();
                        if (byteas.Count == 0) ac.Give(204); // no content
                        else
                        {
                            StaticContent cont = new StaticContent(byteas);
                            ac.Give(200, cont);
                        }
                    }
                    else ac.Give(404); // not found
                }
            }
            else // post
            {
                var frm = await ac.ReadAsync<Form>();
                ArraySegment<byte> icon = frm[nameof(icon)];
                using (var dc = Service.NewDbContext())
                {
                    if (dc.Execute("UPDATE shops SET icon = @1 WHERE id = @2 AND city = @3", p => p.Set(icon).Set(id).Set(city)) > 0)
                    {
                        ac.Give(200); // ok
                    }
                    else
                    {
                        ac.Give(500); // internal server error
                    }
                }
            }
        }
    }

    public class AdmShopVarWork : ShopVarWork
    {
        public AdmShopVarWork(WorkContext wc) : base(wc)
        {
        }
    }
}