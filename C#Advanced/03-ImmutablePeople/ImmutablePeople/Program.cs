using System;
using System.Collections.Generic;

namespace ImmutablePeople {

	// IMPORTANT NOTE 1: It is strictly forbidden to use "records" (i.e. record classes or record structs) to implement Person, Student, Teacher or other types !!!

	class Program {

		// IMPORTANT NOTE 2: You should NOT change anything in Main method as part of your solution! Put you implementation in other methods and types.
		// IMPORTANT NOTE 3: This is NOT the complete assignment - required functional and non-functional requirements for your implementation, that cannot be deduced from the example usage below,
		//					 are part of your discussion with the customer - i.e. presented in your labs/practicals class!
		static void Main(string[] args) {
			var studentA = Student.Default
							.WithName("Pavel Rozek").WithPassword("lavice")
							.WithDateEnrolled(new DateOnly(2015, 9, 21));

			var studentB = Student.Default
							.WithName("Marie Pichova").WithPassword("zidle")
							.WithDateEnrolled(new DateOnly(2018, 9, 25));

			var teacher = Teacher.Default
							.WithName("Pavel Jezek").WithPassword("stul")
							.WithCoursesHeld(5);

			List<Person> people = new List<Person> { studentA, studentB, teacher };
			Console.WriteLine("+++ People:");
			people.PrintAll();
			Console.WriteLine();

			var updatedPeople = people.WithPasswordResetByFirstName(firstName: "Pavel", newPassword: "pohovka");
			Console.WriteLine("+++ Updated people:");
			updatedPeople.PrintAll();
			Console.WriteLine();

			Console.WriteLine("+++ People:");
			people.PrintAll();
			Console.WriteLine();

			Console.WriteLine("+++ Just students:");
			var justStudents = new List<Student> { studentA, studentB };
			justStudents.PrintAll();
			Console.WriteLine();

			var updatedStudents = justStudents.WithPasswordResetByFirstName(firstName: "Marie", newPassword: "letadlo");
			Console.WriteLine("+++ Updated students:");
			updatedStudents.PrintAll();
		}
	}

}