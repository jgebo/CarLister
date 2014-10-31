using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace CarLister.Models
{
    public class CarViewModel
    {
        public SelectList yearList { get; set; }
        public SelectList makeList { get; set; }
        public SelectList modelList { get; set; }
        public string selectedYear { get; set; }
        public string selectedMake { get; set; }
        public string selectedModel { get; set; }
        public PagedList.IPagedList<CarLister.Models.HCL2> carsList { get; set; }
    }
}