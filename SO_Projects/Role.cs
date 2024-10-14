namespace Lab4_FileManagement;

public class Role
{
    public int RoleId { get; set; } 
    public string RoleName { get; set; } 
    
    public Role(int RoleId, string RoleName)
    {
        this.RoleId = RoleId;
        this.RoleName = RoleName;
    }
}