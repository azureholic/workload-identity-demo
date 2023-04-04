using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkloadIdentity.Web.Pages.SqlDemo;
public class SqlDemoViewModel
{
   
   
    public string SelectedIdentity { get; set; }
    public string SelectedDatabase { get; set; }
    public string Error { get; set; }
    public string JsonData { get; set; }
    public string Token { get; set; }

    public List<SelectListItem> Identities { get; } = new List<SelectListItem>();
    

    public List<SelectListItem> Databases { get; } = new List<SelectListItem>()
    {
        new SelectListItem {Value = "db-customer01", Text = "db-customer01"},
        new SelectListItem {Value = "db-customer02", Text = "db-customer02"}
    };

   
}
