using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}

public class UploadRequestDto
{
    public UploadRequest UploadRequest { get; set; }
    public int id { get; set; }
}
public class UploadRequest
{
    public string FileName { get; set; }
    public string Extension { get; set; }
    public UploadType UploadType { get; set; }
    public byte[] Data { get; set; }
}
public enum UploadType : byte
{

    [Description(@"Images\Products")]
    Products,
    [Description(@"Images\Saloon")]
    Saloon,

    [Description(@"Images\Product")]
    Product,
     
    [Description(@"Images\Packege")]
    Packege,

    [Description(@"Images\Appointment")]
    Appointment,

    [Description(@"Images\Offer")]
    Offer,

    [Description(@"Images\Utility")]
    Utility,

}

public static class EnumExtensions
{
    public static string ToDescriptionString(this Enum val)
    {
        var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0
            ? attributes[0].Description
            : val.ToString();
    }
}

