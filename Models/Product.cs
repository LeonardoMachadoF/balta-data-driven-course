using System.ComponentModel.DataAnnotations;

namespace DataDriven.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório!")]
    [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
    [MaxLength(60, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório!")]
    [MaxLength(1024, ErrorMessage = "Esse campo deve conter até 1024 caracteres!")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório!")]
    [Range(0, int.MaxValue, ErrorMessage = "O preço deve ser maior do que zero")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatório!")]
    [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida!")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}