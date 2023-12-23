using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class RmGHSMatchDtlVM
    {
        public string RMID { get; set; }
        public int FileCenterID { get; set; }
        public int DangerLevel { get; set; }
        public int ReviseBy { get; set; }
        public DateTime ReviseDate { get; set; }
    }
}
