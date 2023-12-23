using Innovation.Mobile.App.Models;
using System.IO;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface IPrintService
    {
        bool PrintImage(Stream img);
        bool PrintImageA5(Stream img);
        bool PrintPdfFile(Stream file);
        bool PrintPdfFile(Stream file, MediaSize size = MediaSize.Unknown);
        string CommandPrint(string port, string address, MaterialReceivePlanDtlBarcode obj);
        string CommandPrintPickingList(string port, string address, MaterialPickingDtlVM obj);
    }
    public enum MediaSize
    {
        A4,
        A5,
        FourXSix,
        FourXFour,
        Unknown
    }
}
