using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace FileStorage.Models
{
    public class EditFileViewModel
    {
        [HiddenInput]
        public int FileId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}