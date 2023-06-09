namespace MarcketAppliction.Services.Interface
{
    public interface IPasswordHelper
    {
        string EncodePasswordMd5(string pass);
    }
}