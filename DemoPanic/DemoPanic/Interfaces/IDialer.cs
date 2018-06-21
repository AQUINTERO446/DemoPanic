namespace DemoPanic.Interfaces
{
    using System.Threading.Tasks;
    public interface IDialer
    {
        Task<bool> DialAsync(string number);
    }
}
