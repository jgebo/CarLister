using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PagedList.Mvc;
using PagedList;
using System.Web.Mvc;

namespace CarLister.Models
{

    public class HCLViewModel
    {
        [Display(Name = "id")]
        public int id { get; set; }
        [Display(Name = "MakeId")]
        public string make { get; set; }
        [Display(Name = "ModelId")]
        public string model { get; set; }
        [Display(Name = "TrimId")]
        public string trim { get; set; }
        [Display(Name = "YearId")]
        public string year { get; set; }
        [Display(Name = "Pics")]
        public string picurl { get; set; }

        [Display(Name = "Makes")]
        public List<HCLListViewModel> trimmedlist;
    }

    public class HCLListViewModel
    {
        public int id { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string trim { get; set; }
        public string year { get; set; }
        public string picurl { get; set; }

        [Display(Name = "Makes")]
        public System.Web.Mvc.SelectList Makes { get; set; }
        public string[] SelectedMake { get; set; }
    }

    public class jsViewModel
    {
        public string key { get; set; }
        public string model { get; set; }
        public List<jsListViewModel> trimlist;
    }
    public class jsListViewModel
    {
        public string key { get; set; }
        public string model { get; set; }
        public string realId { get; set; }
    }
}
