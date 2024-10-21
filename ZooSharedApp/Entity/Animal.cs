using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Entity.Animal;

public class Animal{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set;}
    public required string? Name { get; set;}
    public required string? Class {get; set;}
    public bool IsMale {get; set;}

    override public string ToString(){
        string gender = IsMale ? "Male" : "Female";
        return "Id: \"" + Id + "\" Name: \"" + Name + "\" Class: \"" + Class + "\" Gender: \"" + gender +"\"";
    }
}