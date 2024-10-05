using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface ICommand
    {
        Task Execute();
    }
}
