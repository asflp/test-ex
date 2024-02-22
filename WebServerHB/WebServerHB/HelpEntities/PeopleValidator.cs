using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using WebServerHB.Entities;

namespace WebServerHB.HelpEntities;

public class PeopleValidator: AbstractValidator<People>
{
    private List<string> _categories = new() { "Друзья", "Семья", "Коллеги", "Другие" };
    
    public PeopleValidator()
    {
        RuleFor(people => people.Name).NotEmpty()
            .WithMessage("Имя и фамилия не могут быть пустым")
            .Length(5, 200)
            .WithMessage("Имя и фамилия должны быть от 5 до 200 символов");
        
        RuleFor(people => people.Category).NotEmpty()
            .WithMessage("Категория человека не может быть пустой")
            .Must(c => _categories.Contains(c))
            .WithMessage("Несуществующая категория");

        RuleFor(people => people.Date).NotEmpty()
            .WithMessage("Текст вопроса не может быть пустым")
            .Must(BeAValidDate)
            .WithMessage("Неверный формат даты");
    }
    
    private bool BeAValidDate(string value)
    {
        return DateOnly.TryParse(value, out _);
    }
    
    public string? GetErrors(ValidationResult validationResult)
    {
        if (!validationResult.IsValid)
        {
            var dict = new Dictionary<string, string>();
            
            foreach (var failure in validationResult.Errors)
            {
                dict.TryAdd(failure.PropertyName, failure.ErrorMessage);
            }

            return JsonSerializer.Serialize(dict);
        }

        return null;
    }
}