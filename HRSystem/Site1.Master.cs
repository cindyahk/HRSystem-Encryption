using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace HRSystem
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Session["LoggedInUser"] == null)

                    Response.Redirect("~/Login/Login.aspx");

                else
                {
                    Response.ClearHeaders();
                    Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                    Response.AddHeader("Pragma", "no-cache");

                    //disable caching
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetNoStore();
                }
            }
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/Login/Login.aspx");
        }

        protected void Menu1_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (!(bool)Session["SystemSet"])//disable User Management for those without system rights
            {
                SiteMapNode mapNode = (SiteMapNode)e.Item.DataItem;

                if (mapNode.Title == "User Management")
                {
                    MenuItem parent = e.Item.Parent;
                    if (parent != null)
                    {
                        parent.ChildItems.Remove(e.Item);
                    }
                }
            }
        }

    
       
    }
}