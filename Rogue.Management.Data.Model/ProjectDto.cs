namespace Rogue.Management.Data.Model;

public class ProjectDto
{
    public ProjectDto(string name)
    {
        this.Name = name;
    }

    public int Id { get; set; }

    public UserDto Owner { get; set; } = null!;

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
}
