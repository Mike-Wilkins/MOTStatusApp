using MOTStatusWebApi.Data;

namespace MOTStatusWebApi.Interfaces
{
    public interface IMOTStatusDetailsRepository
    {
        ICollection<MOTStatusDetails> GetStatusDetails();

        MOTStatusDetails GetStatusDetail(int Id);
        MOTStatusDetails GetRegistrationNumber(string registrationNumber);
        bool StatusDetailExists(int Id);
        bool StatusDetailExists(string registrationNumber);

        bool Add(MOTStatusDetails details);
        bool Save();

        bool Update(MOTStatusDetails details);
        bool Delete(MOTStatusDetails details);
    }
}
