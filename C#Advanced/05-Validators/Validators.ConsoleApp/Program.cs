using System;
using System.Collections.Generic;
using Validators.ConsoleApp;

#nullable enable

/*
 * Error message type containing the errror-string and its implementation of ToString()
 */
public struct ValidationError
{
	public string Reason { get; init; }

	public ValidationError(string reason)
	{
		Reason = reason;
	}

	public new string ToString() => $"    > {this.Reason}";
}

/*
 * Order type containing (string Id, int Amount, decimal TotalPrice, string? Comment)
 */
public record class Order
{
	public required string Id { get; set; }
	public int Amount { get; set; }
	public decimal TotalPrice { get; set; }
	public string? Comment { get; set; }
}

/*
 * Simulation of *better* order type testing contravaration
 */
public record class SuperOrder : Order { }

/*
 * implementatin given in file: Validators.cs
 */
//class OrderValidator : Validator<Order> {
//	// TODO:
//	// ... Validate(Order value) ... {
//	//	var allErrors = new List<ValidationError>();
//	//	allErrors.AddRange(Validate(value.Amount, new RangeValidator<int> { Min = 1, Max = 10 }));
//	//	allErrors.AddRange(Validate(value.Id, new NonBlankStringValidator(), new StringLengthValidator(new RangeValidator<int> { Min = 1, Max = 8 })));
//	//	allErrors.AddRange(Validate(value.TotalPrice, new RangeValidator<decimal> { Min = 0.01M, Max = 999.99M }));
//	//	allErrors.AddRange(Validate(value.Comment, new NotNullValidator()));
//	//	return allErrors;
//	// }
//}

//class AdvancedOrderValidator : Validator<Order>
//{
//	// TODO:
//	// ... Validate(Order value) ... {
//	//	  Similar syntax as for OrderValidator, but more compact:
//	//	  + without need to specify inferable types <int>, <decimal> ...
//	//	  + without need for new ...
//	// }
//}

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("--- plain Validators ---");

		var nonBlankStringValidator = new NonBlankStringValidator();
		nonBlankStringValidator.Validate("   ").Print();
		nonBlankStringValidator.Validate("hello").Print();

		var rangeValidator = new RangeValidator<int> { Min = 1, Max = 6 };
		rangeValidator.Validate(7).Print();
		rangeValidator.Validate(1).Print();

		var stringLengthValidator = new StringLengthValidator(new RangeValidator<int> { Min = 5, Max = 6 });
		stringLengthValidator.Validate("Jack").Print();
		stringLengthValidator.Validate("hello-world").Print();
		stringLengthValidator.Validate("hello").Print();

		var notNullValidator = new NotNullValidator();
		object? obj = null;
		notNullValidator.Validate(obj).Print();
		string? s = null;
		notNullValidator.Validate(s).Print();
		Order? order = null;
		notNullValidator.Validate(order).Print();
		s = "hello";
		notNullValidator.Validate(s).Print();

		Console.WriteLine("--- AdvancedOrderValidator.Validate() ---");

		AdvancedOrderValidator advancedValidator = new AdvancedOrderValidator();

		var o1 = new Order { Id = "    ", Amount = 5 };
		advancedValidator.Validate(o1).Print();

		var o2 = new Order { Id = "AC405", Amount = 5 };
		advancedValidator.Validate(o2).Print();

		var o3 = new Order { Id = "AC405", Amount = 600 };
		advancedValidator.Validate(o3).Print();

		var o4 = new Order { Id = "", Amount = 600 };
		advancedValidator.Validate(o4).Print();

		var o5 = new Order { Id = "AC405-12345678", Amount = 5, TotalPrice = 42, Comment = "Best order ever" };
		advancedValidator.Validate(o5).Print();

		var o6 = new Order { Id = "AC405", Amount = 5, TotalPrice = 42, Comment = "Best order ever" };
		advancedValidator.Validate(o6).Print();

		Console.WriteLine("--- OrderValidator.Validate() ---");

		OrderValidator orderValidator = new OrderValidator();

		orderValidator.Validate(o1).Print();
		orderValidator.Validate(o2).Print();
		orderValidator.Validate(o3).Print();
		orderValidator.Validate(o4).Print();
		orderValidator.Validate(o5).Print();
		orderValidator.Validate(o6).Print();

		Console.WriteLine("--- ValidateSuperOrders() ---");

		var s1 = new SuperOrder { Id = "SO501", Amount = 5, TotalPrice = 42, Comment = "Super order 1" };
		var s2 = new SuperOrder { Id = "SO502", Amount = 700, TotalPrice = 41, Comment = "Super order 2" };
		var s3 = new SuperOrder { Id = "", Amount = 800, Comment = "Super order 2" };

		var orders = new List<SuperOrder> { s1, s2, s3 };
		ValidateSuperOrders(orders, orderValidator);

		Console.WriteLine("--- ValidateAll() ---");

		ValidateAll(orders, orderValidator);

		Console.WriteLine("--- ValidateAll<SuperOrder>() ---");

		ValidateAll<SuperOrder>(orders, orderValidator);
	}

	// static method validating and printing list of Orders 
	static void ValidateSuperOrders(IEnumerable<SuperOrder> orders, IValidator<SuperOrder> validator)
	{
		foreach (var o in orders)
		{
			validator.Validate(o).Print();
		}
	}

    // static method validating and printing list of same validators (contravariant) 
    static void ValidateAll<T>(IEnumerable<T> orders, IValidator<T> validator)
	{
		foreach (var o in orders)
		{
			validator.Validate(o).Print();
		}
	}
}
