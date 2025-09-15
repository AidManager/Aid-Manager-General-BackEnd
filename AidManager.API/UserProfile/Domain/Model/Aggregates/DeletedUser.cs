namespace AidManager.API.Authentication.Domain.Model.Entities;

public class DeletedUser
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Password { get; private set; }
    public string ProfileImg { get; private set; }
    public int CompanyId { get; set; }
    public string Role { get; private set; }
    
    public DateTime DeletedAt { get; private set; }

    private DeletedUser()
    {
        this.FirstName = "This user has been deleted.";
        this.LastName = "-";
        this.Age = 0;
        this.Email = "";
        this.Phone = "-";
        this.Password = "-";
        this.ProfileImg = "https://www.shutterstock.com/image-vector/user-removal-linear-style-icon-600nw-2473043843.jpg";
        this.Role = "-";
        this.DeletedAt = DateTime.Now;
    }

    public DeletedUser(User user)
    {
        this.Id = user.Id;
        this.FirstName = user.FirstName;
        this.LastName = user.LastName;
        this.Age = user.Age;
        this.Email = user.Email;
        this.Phone = user.Phone;
        this.Password = user.Password;
        this.ProfileImg = user.ProfileImg;
        this.CompanyId = user.CompanyId;
        this.Role = user.Role.ToString();
        this.DeletedAt = DateTime.Now;
    }

}