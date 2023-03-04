using WireCart.Entities;

namespace WireCart.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> SendMessage(Contact contact);
        Task<Contact> Subscribe(string address);
    }
}
