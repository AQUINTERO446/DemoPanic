namespace DemoPanic.Interface
{
    using System.Threading.Tasks;
    public interface IDialer
    {
        Task<bool> DialAsync(string number);
    }
}
