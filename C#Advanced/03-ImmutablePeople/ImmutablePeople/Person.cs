using System;
using System.Collections.Generic;

#nullable enable

namespace ImmutablePeople
{
    /*
     * contract implementing the Default getter property, that should create new class with with all its properties 
     *  set to default value (null) - static init
     */
    public interface IDefault<T> where T : class
    {
        public static abstract T Default { get; }
    }

    /*
     * base abstract class for Student, Teacher ... that creates contract of Cloneability and implementing common properties
     */
    public abstract class Person : ICloneable
    {
        protected abstract string _role { get; }
        public string? Name { get; set; }
        public string? Password { get; set; }

        // if objects property Name is set, then return its part - FirstName; ASSUMING THE FIRSTNAME IS THE FIRST WORD IN NAME!!!
        public virtual string GetFirstName()
        {
            if (this.Name is null)
            {
                throw new InformationNotFilledException("The name of person is missing.");
            }

            string[] slicedName = this.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return slicedName[0];
        }

        // contract with inheriting classes to implement its ToString()
        public new abstract string ToString();

        // the possibility of iheriting classes to create a clone of itself (new heap instance)
        public object Clone() { return this.MemberwiseClone(); }
    }

    /*
     * class that directly implements 'Person' with possibility of Default that returns Student instance with all properties set to null
     */
    public class Student : Person, IDefault<Student>
    {
        protected override string _role => "Student";
        public DateOnly? DateEnrolled { get; set; }
        public static Student Default { get => new Student() { Name = null, Password = null, DateEnrolled = null }; }

        // implementation of ToString() method according to the task format
        public override string ToString() => $"{this._role} {this.Name} has password \"{this.Password}\"";
    }

    /*
     * class that directly implements 'Person' with possibility of Default that returns Teacher instance with all properties set to null
     */
    public class Teacher : Person, IDefault<Teacher>
    {
        protected override string _role => "Teacher";
        public uint? CoursesHeld { get; set; }
        public static Teacher Default { get => new Teacher() { Name = null, Password = null, CoursesHeld = null }; }

        public override string ToString() => $"{this._role} {this.Name} has password \"{this.Password}\"";
    }

    /*
     * support class for **fluent syntax** on class 'Person', that creates clone of given instace, do stuff and returns it
     */
    public static class PersonExtension
    {
        // returns new cloned Person-ish object with changed Name given by string argument 'name'
        public static TPerson WithName<TPerson>(this TPerson person, string name) where TPerson : Person
        {
            TPerson clonedObejct = (TPerson)person.Clone();
            clonedObejct.Name = name;
            return clonedObejct;
        }

        // returns new cloned Person-ish object with changed Password given by string argument 'password'
        public static TPerson WithPassword<TPerson>(this TPerson person, string password) where TPerson : Person
        {
            TPerson clonedObejct = (TPerson)person.Clone();
            clonedObejct.Password = password;
            return clonedObejct;
        }
    }

    /*
     * support class for **fluent syntax** on class 'Student', that creates clone of given instace, do stuff and returns it
     */
    public static class StudentExtension
    {
        // returns new cloned Student-ish object with changed DateEnrolled given by string argument 'date'
        public static TStudent WithDateEnrolled<TStudent>(this TStudent student, DateOnly date) where TStudent : Student 
        {
            TStudent clonedObejct = (TStudent)student.Clone();
            clonedObejct.DateEnrolled = date;
            return clonedObejct;
        }
    }

    /*
     * support class for **fluent syntax** on class 'Teacher', that creates clone of given instace, do stuff and returns it
     */
    public static class TeacherExtension
    {
        // returns new cloned Teacher-ish object with changed CoursesHeld given by string argument 'coursesCount'
        public static TTeacher WithCoursesHeld<TTeacher>(this TTeacher teacher, uint coursesCount) where TTeacher : Teacher
        {
            TTeacher clonedObejct = (TTeacher)teacher.Clone();
            clonedObejct.CoursesHeld = coursesCount;
            return clonedObejct;
        }
    }

    /*
     *  support List **fluent syntax**, that creates clone of given instace
     */
    public static class ListExtension
    {
        // print on stdout all the List Person-ish instances given by its implementation of ToString()
        public static void PrintAll<TType>(this List<TType> peopleList) where TType : Person
        {
            foreach (TType person in peopleList)
            {
                Console.WriteLine(person.ToString());
            }
        }

        // returns an updated List (new clone) of Person-ish instances, where is 'Password' changed if firstName matches the Persons property Name
        public static List<TType> WithPasswordResetByFirstName<TType>(this List<TType> peopleList, string firstName, string newPassword)
            where TType : Person
        {
            List<TType> clonedPeopleList = new List<TType>(peopleList.Count);
            foreach (TType person in peopleList)
            {
                TType clonedPerson = (TType)person.Clone();

                if (person.GetFirstName() == firstName)
                {
                    clonedPerson.Password = newPassword;
                }
                clonedPeopleList.Add(clonedPerson);
            }

            return clonedPeopleList;
        }
    }

    /*
     * exception thrown when trying do stuff with unfilled property
     */
    public class InformationNotFilledException : ApplicationException 
    { 
        public InformationNotFilledException(string message) : base(message) { }
    }
}