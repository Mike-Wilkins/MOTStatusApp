using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using MOTStatusWebApi.Models;

namespace MOTStatusWebApi.Repository
{
    public class MOTTestCertificateDetailsRepository : IMOTTestCertificateDetailsRepository
    {
        private readonly DataContext _context;
        public MOTTestCertificateDetailsRepository(DataContext context)
        {
            _context = context;
        }

        public bool Add(MOTTestCertificateDetails testCertificateDetails)
        {
           _context.MOTTestCertificateDetails.Add(testCertificateDetails);
            return Save();
        }

        public bool Delete(MOTTestCertificateDetails details)
        {
            _context.MOTTestCertificateDetails.Remove(details);
            return Save();
        }

        public MOTTestCertificateDetails GetRegistrationNumber(string registrationNumber)
        {
            return _context.MOTTestCertificateDetails.Where(mots => mots.RegistrationNumber == registrationNumber).FirstOrDefault();
        }

        public MOTTestCertificateDetails GetTestCertificateDetail(int Id)
        {
            return _context.MOTTestCertificateDetails.Where(mots => mots.Id == Id).FirstOrDefault();
        }

        public ICollection<MOTTestCertificateDetails> GetTestCertificateDetails()
        {
            return _context.MOTTestCertificateDetails.OrderBy(mots => mots.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TestDetailExists(int Id)
        {
            return _context.MOTTestCertificateDetails.Any(mots => mots.Id == Id);
        }

        public bool TestDetailExists(string registrationNumber)
        {
            return _context.MOTTestCertificateDetails.Any(mots => mots.RegistrationNumber.Equals(registrationNumber));
        }

        public bool Update(MOTTestCertificateDetails details)
        {
            _context.MOTTestCertificateDetails.Update(details);
            return Save();
        }
    }
}
