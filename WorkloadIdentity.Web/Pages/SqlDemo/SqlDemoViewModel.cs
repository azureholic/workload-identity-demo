using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkloadIdentity.Web.Pages.SqlDemo;
public class SqlDemoViewModel
{
    public string SelectedIdentity { get; set; }
    public string SelectedDatabase { get; set; }
    public string Error { get; set; }
    public string JsonData { get; set; }

    public List<SelectListItem> Identities { get; } = new List<SelectListItem>()
    {
        new SelectListItem {Value = "4fd658a4-5631-4ef3-bc86-872484099189", Text = "Customer 01"},
        new SelectListItem {Value = "2192a328-5042-43c0-9f52-b7ad81b40a65", Text = "Customer 02"}
    };

    public List<SelectListItem> Databases { get; } = new List<SelectListItem>()
    {
        new SelectListItem {Value = "db-customer01", Text = "db-customer01"},
        new SelectListItem {Value = "db-customer02", Text = "db-customer02"}
    };
}
