using System;
using System.Collections.Generic;

#nullable enable
namespace Validators.ConsoleApp
{
    /*
     * Contravariant generic interface by validators type so the Validate(...) method can accept "better" types
     */
    public interface IValidator<in TValidatedType>
    {
        // 
        public IReadOnlyCollection<ValidationError> Validate(TValidatedType value);
    }

    /*
     * Base generic validator class defining required Validate(...) function 
     */
    public abstract class Validator<TValidatedType> : IValidator<TValidatedType>
    {
        public abstract IReadOnlyCollection<ValidationError> Validate(TValidatedType value);
    }

    /*
     * Base class for validators theirs Validate(...) method accepts only strings
     */
    public abstract class StringValidator : Validator<string> { }

    /*
     * Validator checking if given string is string.Empty or consists of white-space characters
     */
    public sealed class NonBlankStringValidator : StringValidator
    {
        // validation method which checks given string using string.IsNullOrWhiteSpace from stdLibrary and return errorList or emptyList
        public override List<ValidationError> Validate(string value)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(value))
            {
                validationErrors.Add(new ValidationError($"\"{value}\" is empty or just whitespaces."));
            }

            return validationErrors;
        }
    }

    /*
     * Validator checking if given string lenght is in range given in class construction by RangeValidator<int>
     */
    public sealed class StringLengthValidator : StringValidator
    {
        public RangeValidator<int> _rangeValidator { get; init; }

        public StringLengthValidator(RangeValidator<int> validator)
        {
            this._rangeValidator = validator;
        }

        // validation method which checks given string if fulfils lenght condition by RangeValidator<int> and return errorList or emptyList
        public override List<ValidationError> Validate(string value)
        {
            List<ValidationError> validationErrors = this._rangeValidator.Validate(value.Length);
            if (validationErrors.Count is > 0)
            {
                string newErrorText = $"\"{value}\" lenght {validationErrors[0].Reason}";
                validationErrors[0] = new ValidationError(newErrorText);
            }

            return validationErrors;
        }
    }

    /*
     * Generic Validator checking if given comparable *type* is in given borders initialized in instance construction
     */
    public sealed class RangeValidator<TType> : Validator<TType>
        where TType : IComparable<TType>
    {
        public required TType Min { get; init; }
        public required TType Max { get; init; }

        // validation method which checks if given comparable *type* is between Min&Max borders and return errorList or emptyList
        public override List<ValidationError> Validate(TType value)
        {
            var validationErrors = new List<ValidationError>();

            if (value.CompareTo(this.Min) is < 0)
            {
                validationErrors.Add(new ValidationError($"{value} is less than minimum {this.Min}."));
            }
            else if (value.CompareTo(this.Max) is > 0)
            {
                validationErrors.Add(new ValidationError($"{value} is greater than maximum {this.Max}."));
            }

            return validationErrors;
        }
    }

    /*
     * Validator checking if given object? is null 
     */
    public sealed class NotNullValidator : Validator<object?>
    {
        // validation method which checks if instance is null and return errorList or emptyList
        public override List<ValidationError> Validate(object? value)
        {
            var validationErrors = new List<ValidationError>();
            if (value is null)
            {
                validationErrors.Add(new ValidationError($"\"{value}\" is null."));
            }

            return validationErrors;
        }
    }

    /*
     * Validator checking if given Order type (string Id, int Amount, decimal TotalPrice, string? Comment) is valid instance
     */
    public class OrderValidator : Validator<Order>
    {
        // validation method which checks if Order instance fulfils all neccessary conditions using overloaded Validate(...) methods and return listOfAllErrors or emptyList
        public override List<ValidationError> Validate(Order value)
        {
            var allErrors = new List<ValidationError>();
            allErrors.AddRange(Validate(value.Amount, new RangeValidator<int> { Min = 1, Max = 10 }));
            allErrors.AddRange(Validate(value.Id, new NonBlankStringValidator(), new StringLengthValidator(new RangeValidator<int> { Min = 1, Max = 8 })));
            allErrors.AddRange(Validate(value.TotalPrice, new RangeValidator<decimal> { Min = 0.01M, Max = 999.99M }));
            allErrors.AddRange(Validate(value.Comment, new NotNullValidator()));
            return allErrors;
        }

        // helper validation method which checks if amount is in given range by Validate(...) using RangeValidator<int> 
        protected virtual List<ValidationError> Validate(int amount, RangeValidator<int> rangeIntValidator) => rangeIntValidator.Validate(amount);

        // helper validation method which check if id fulfils string validators (eg.: is not empty/white-spaces, lenght of string in given range, etc.)
        protected virtual List<ValidationError> Validate(string id, params StringValidator[] stringValidators)
        {
            List<ValidationError> validationErrors = new List<ValidationError>();
            foreach (var validator in stringValidators)
            {
                validationErrors.AddRange(validator.Validate(id));
            }

            return validationErrors;
        }

        // helper validation method which check if totalPrice is in given range by Validate(...) using RangeValidator<decimal>
        protected virtual List<ValidationError> Validate(decimal totalPrice, RangeValidator<decimal> rangeDecimalValidator) => rangeDecimalValidator.Validate(totalPrice);

        // helper validation method which check if comment is not null
        protected virtual List<ValidationError> Validate(string? comment, NotNullValidator notNullValidator) => notNullValidator.Validate(comment);
    }

    /*
     * SHOULD BE *BETTER* IMPLEMENT IMPLEMENTATION OF OrderValidator USING EASIER SYNTAX
     *  1. NOT *NEW* KEYWORD NEEDED IN OTHER VALIDATOR INITIALIZATION
     *  2. NOT GENERIC TYPE IN <...> REQUIRED TO SET
     *  3. SHOULD BE INHERITED FROM Validator<Order>
     *  4. MAKE CLASS PUBLIC AFTERWARD
     * TODO: better implementation as written above 
     */
    internal class AdvancedOrderValidator : OrderValidator
    {
        // TODO: new implementation as written in class comment
        public override List<ValidationError> Validate(Order value)
        {
            return base.Validate(value);
        }
    }

    /*
     * Extension for ICollection<ValidationError> implementing Print(...) method
     */
    public static class ValidationErrorListExtension
    {
        // extension method for ICollection<ValidationError> that prints all errors stored in this collection to stdOut
        public static void Print(this IReadOnlyCollection<ValidationError> validationErrorsList)
        {
            if (validationErrorsList.Count is 0)
            {
                Console.WriteLine("  >>> Validation successful >>>");
            }
            else
            {
                Console.WriteLine("  >>> Validation failed >>>");
                foreach (var error in validationErrorsList)
                {
                    Console.WriteLine(error.ToString());
                }
            }
            Console.Write("\n");
        }
    }
}