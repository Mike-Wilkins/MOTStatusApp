using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;

namespace MOTStatusWebApi.Repository
{
    public class MOTStatusDetailsRepository : IMOTStatusDetailsRepository
    {
        private readonly DataContext _context;
        public MOTStatusDetailsRepository(DataContext context)
        {
            _context = context;
        }

        public MOTStatusDetails GetRegistrationNumber(string registrationNumber)
        {
            return _context.MOTStatus.Where(mots => mots.RegistrationNumber == registrationNumber).FirstOrDefault();
        }

        public MOTStatusDetails GetStatusDetail(int Id)
        {
           return _context.MOTStatus.Where(mots => mots.Id == Id).FirstOrDefault();
        }

        public ICollection<MOTStatusDetails> GetStatusDetails()
        {
            return _context.MOTStatus.OrderBy(mots => mots.Id).ToList();
        }

        public bool StatusDetailExists(int Id)
        {
            return _context.MOTStatus.Any(mots => mots.Id == Id);
        }

        public bool StatusDetailExists(string registrationNumber)
        {
            return _context.MOTStatus.Any(mots => mots.RegistrationNumber.Equals(registrationNumber));
        }
    }
}
