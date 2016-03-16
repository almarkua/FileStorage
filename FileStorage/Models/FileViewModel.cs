using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileStorage.Domain;

namespace FileStorage.Models
{
    public class FileViewModel
    {
        private int _fileId;
        private long _size;
        private string _name, _description, _accessUrl;
        private bool _isShared;
        private Category _category;

        public FileViewModel(File file)
        {
            _fileId = file.FileId;
            _size = file.Size;
            _name = file.Name;
            _description = file.Description;
            _isShared = file.IsShared;
            _category = file.Category;
        }

        public int FileId {
            get { return _fileId; }
        }

        public string Name {
            get { return _name; }
        }

        public string Description {
            get { return _description; }
        }

        public string Size
        {
            get {return File.GetSizeString(_size); }
        }

        public bool IsShared => _isShared;

        public int CategoryId => (_category == null) ? 0 : _category.CategoryId;

        public string CategoryName => (_category == null)? "Without category" : _category.Name;
    }
}