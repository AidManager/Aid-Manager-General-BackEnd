using AidManager.API.Authentication.Domain.Model.Commands;

namespace AidManager.API.Authentication.Domain.Model.Aggregates;

public class Company
{
    public int Id { get; private set; }
     public string CompanyName { get; private set; }
     public string Country { get; private set; }
     public string Email {get; private set; }
     public int ManagerId { get; private set; }
     
     public string TeamRegisterCode { get; private set; }

     private Company() { }
     public Company(CreateCompanyCommand command)
     {
        CompanyName = command.CompanyName;
        Country = command.Country;
        Email = command.Email;
        ManagerId = command.UserId;
        TeamRegisterCode = GenerateRegisterCode();
     }
     
        public void Update(EditCompanyIdCommand command)
        {
            CompanyName = command.BrandName;
            Country = command.Country;
            Email = command.Email;
            TeamRegisterCode = GenerateRegisterCode();
        }

     private string GenerateRegisterCode()
     {
         Guid guid = Guid.NewGuid();
         string base64Guid = Convert.ToBase64String(guid.ToByteArray());
         string shortGuid = base64Guid.Replace("/", "_").Replace("+", "-").Substring(0, 15);
         return shortGuid;
     }
}