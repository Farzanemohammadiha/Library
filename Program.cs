using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    static void Main()
    {
        Library myLibrary = new Library();


        myLibrary.AddBook(new Book(1, "Crime and Punishment", "Dostoevsky"));
        myLibrary.AddBook(new Book(2, "1984", "George Orwell"));
        myLibrary.AddBook(new Book(3, "The Great Gatsby", "F. Scott Fitzgerald"));
        myLibrary.AddBook(new Book(4, "To Kill a Mockingbird", "Harper Lee"));

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n****** Library Menu ******");
            Console.WriteLine("1: Register a member");
            Console.WriteLine("2: Add a book (by admin)");
            Console.WriteLine("3: Borrow a book");
            Console.WriteLine("4: Return a book");
            Console.WriteLine("5: Search books");
            Console.WriteLine("6: Show all books");
            Console.WriteLine("7: Show members");
            Console.WriteLine("0: Exit");
            Console.Write("Choose an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter member name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter family name: ");
                    string family = Console.ReadLine();
                    Console.Write("Enter member codeID: ");
                    int id = int.Parse(Console.ReadLine());
                    myLibrary.RegisterMember(new Member(name, family, id));
                    break;

                case "2":
                    Console.Write("Enter admin password to add book: ");
                    string password = Console.ReadLine();
                    if (password != "admin123")
                    {
                        Console.WriteLine("Access denied.");
                        break;
                    }
                    Console.Write("Enter book ID: ");
                    int bookId = int.Parse(Console.ReadLine());
                    Console.Write("Enter book title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter author name: ");
                    string author = Console.ReadLine();
                    myLibrary.AddBook(new Book(bookId, title, author));
                    break;

                case "3":
                    Console.Write("Enter member codeID: ");
                    int borrowMemberId = int.Parse(Console.ReadLine());
                    Console.Write("Enter book ID: ");
                    int borrowBookId = int.Parse(Console.ReadLine());
                    myLibrary.BorrowBook(borrowMemberId, borrowBookId);
                    break;

                case "4":
                    Console.Write("Enter member ID: ");
                    int returnMemberId = int.Parse(Console.ReadLine());
                    Console.Write("Enter book ID: ");
                    int returnBookId = int.Parse(Console.ReadLine());
                    myLibrary.ReturnBook(returnMemberId, returnBookId);
                    break;


                case "5":
                    Console.Write("Enter a keyword for book title: ");
                    string keyword = Console.ReadLine();
                    myLibrary.SearchBooks(keyword);
                    break;

                case "6":
                    myLibrary.ShowBooks();
                    break;

                case "7":
                    myLibrary.ShowMembers();
                    break;

                case "0":
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Book(int id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
            IsBorrowed = false;
            BorrowDate = null;
            ReturnDate = null;
        }
    }

    public class Member
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public int CodeId { get; set; }
        public List<Book> BorrowedBooks { get; set; }

        public Member(string name, string familyName, int codeId)
        {
            Name = name;
            FamilyName = familyName;
            CodeId = codeId;
            BorrowedBooks = new List<Book>();
        }
    }

    public class Library
    {
        private List<Book> Books;
        private List<Member> Members;

        public Library()
        {
            Books = new List<Book>();
            Members = new List<Member>();
        }

        public void AddBook(Book book)
        {
            book.IsBorrowed = false;
            Books.Add(book);  
            Console.WriteLine("Book added successfully!");
        }

        public void RegisterMember(Member member)
        {
            Members.Add(member);
            Console.WriteLine("Member registered successfully!");
        }

        public void BorrowBook(int memberCodeId, int bookId)
        {
            int limitedBooks = 3;
            foreach (Member m in Members)
            {
                if (m.BorrowedBooks.Count > limitedBooks)
                {
                    Console.WriteLine($"Member {m.CodeId} can't borrow more than {limitedBooks} books.");
                }
            }



            Member member = null;
            foreach (Member m in Members)
            {
                if (m.CodeId == memberCodeId)
                {
                    member = m;
                    break;
                }
            }



            Book book = null;
            foreach (Book b in Books)
            {
                if (b.Id == bookId)
                {
                    book = b;
                    break;
                }

            }



            if (member == null)
            {
                Console.WriteLine("Member not found!");
                return;
            }
            if (book == null)
            {
                Console.WriteLine("Book not found!");
                return;
            }
            if (book.IsBorrowed)
            {
                Console.WriteLine("Book already borrowed.");
                return;
            }

            member.BorrowedBooks.Add(book);
            book.IsBorrowed = true;
            book.BorrowDate = DateTime.Now;
            book.ReturnDate = null;
            Console.WriteLine($"{member.Name} borrowed \"{book.Title}\" on {book.BorrowDate.Value:yyyy/MM/dd HH:mm}");


            








        }

        public void ReturnBook(int memberCodeId, int bookId)
        {
            Member member = null;

            foreach (Member m in Members)
            {
                if (m.CodeId == memberCodeId)
                {
                    member = m;
                    break;
                }
            }
            if (member == null)
            {
                Console.WriteLine("Member not found!");
                return;
            }

            Book book = null;

            foreach (Book b in member.BorrowedBooks)
            {
                if (b.Id == bookId)
                {
                    book = b;
                    break;
                }
            }
            if (book == null)
            {
                Console.WriteLine("This member did not borrow the book.");
                return;
            }

            member.BorrowedBooks.Remove(book);
            book.IsBorrowed = false;
            book.ReturnDate = DateTime.Now;
            Console.WriteLine($"{member.Name} returned \"{book.Title}\" on {book.ReturnDate.Value:yyyy/MM/dd HH:mm}");



            if (book.BorrowDate.HasValue && book.ReturnDate.HasValue)
            {
                DateTime borrowDate = book.BorrowDate.Value;
                DateTime returnDate = book.ReturnDate.Value;

                int totalDays = (returnDate - borrowDate).Days;
                int extraDays = totalDays - 30;

                if (extraDays > 0)
                {
                    int fine = extraDays * 5000;
                    Console.WriteLine($" {fine} Toman");
                }
                else
                {
                    Console.WriteLine("no fine.");
                }
            }
            else
            {
                Console.WriteLine("can not find any date.");
            }
        }

        public void ShowBooks()
        {
            Console.WriteLine("\nBooks in the library:");
            foreach (Book b in Books)
            {
                string status;
                if (b.IsBorrowed)
                    status = "Borrowed";
                else
                    status = "Available";


                string borrowDate;
                if (b.BorrowDate.HasValue)
                    borrowDate = b.BorrowDate.Value.ToString("yyyy/MM/dd HH:mm");
                else
                    borrowDate = "—";


                string returnDate;
                if (b.ReturnDate.HasValue)
                    returnDate = b.ReturnDate.Value.ToString("yyyy/MM/dd HH:mm");
                else
                    returnDate = "—";

                Console.WriteLine($"ID: {b.Id}, Title: {b.Title}, Author: {b.Author}, Status: {status}, Borrowed: {borrowDate}, Returned: {returnDate}");
            }
        }

        public void ShowMembers()
        {
            Console.WriteLine("\nRegistered members:");
            foreach (Member m in Members)
            {
                Console.WriteLine($"ID: {m.CodeId}, Name: {m.Name} {m.FamilyName}, Borrowed Books: {m.BorrowedBooks.Count}");
            }
        }




        public void SearchBooks(string keyword)
        {
            bool found = false;

            foreach (Book b in Books)
            {
                if (b.Title.ToLower().Contains(keyword.ToLower()) ||
                    b.Author.ToLower().Contains(keyword.ToLower()))
                {
                    Console.WriteLine($"Found: {b.Title} by {b.Author}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("book not found.");
            }
        }
    }
}
