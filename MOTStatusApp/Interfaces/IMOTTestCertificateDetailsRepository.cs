using MOTStatusWebApi.Models;

namespace MOTStatusWebApi.Interfaces
{
    public interface IMOTTestCertificateDetailsRepository
    {
        ICollection<MOTTestCertificateDetails> GetTestCertificateDetails();
        MOTTestCertificateDetails GetTestCertificateDetail(int Id);
        MOTTestCertificateDetails GetRegistrationNumber(string registrationNumber);
        bool Add(MOTTestCertificateDetails testCertificateDetails);
        bool Save();
        bool TestDetailExists(int Id);
        bool TestDetailExists(string registrationNumber);
        bool Update(MOTTestCertificateDetails details);
        bool Delete(MOTTestCertificateDetails details);
    }
}
