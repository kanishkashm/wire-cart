using WireCart.Data;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Repositories
{
    public class ContactRepository : IContactRepository
    {
        protected readonly WireCartDataContext _dbContext;

        public ContactRepository(WireCartDataContext dbContext)
        {
            _dbContext = dbContext ;
        }

        public async Task<Contact> SendMessage(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> Subscribe(string address)
        {
            // implement your business logic
            var newContact = new Contact();
            newContact.Email = address;
            newContact.Message = address;
            newContact.Name = address;

            _dbContext.Contacts.Add(newContact);
            await _dbContext.SaveChangesAsync();

            return newContact;
        }
    }
}
