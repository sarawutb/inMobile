using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class DataReportFileCenter
    {
        public virtual int ID { get; set; }
        public virtual string Name_SW { get; set; }
        public virtual string Name_User { get; set; }
        public virtual string Path { get; set; }
        public virtual string File_Type { get; set; }
        public virtual string File_Tag { get; set; }
        public virtual int Upload_By { get; set; }
        public virtual DateTime Upload_Date { get; set; }
        public byte[] FileData { get; set; }
        public virtual int Report_ID { get; set; }
        public virtual int Size_Image { get; set; }
        public virtual int? danger_level { get; set; }
        public virtual string pi_code { get; set; }
        public virtual bool Select_File { get; set; }
    }
}
