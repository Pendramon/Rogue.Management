namespace Rogue.Management.View.Model;

public class Project
{
    public Project(string name)
    {
        this.Name = name;
    }

    public User Owner { get; set; } = null!;

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
}
