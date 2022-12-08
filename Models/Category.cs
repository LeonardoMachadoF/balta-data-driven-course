using System.ComponentModel.DataAnnotations;

namespace DataDriven.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório!")]
    [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
    [MaxLength(60, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
    public string Title { get; set; }

}