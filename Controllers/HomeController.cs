using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using Task.Models;

namespace Task.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var searchTerm = string.Empty;
            searchTerm = string.IsNullOrEmpty(Request["query"])
                ? string.Empty
                : Request["query"];
            return View(GetBooks(searchTerm));
        }

        public List<Book> GetBooks(string query)
        {
            List<Book> books = new List<Book>();
            List<Book> results = new List<Book>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            foreach (XmlNode node in xmldoc.SelectNodes("/Inventory/Book"))
            {
                //Fetch the Node values and assign it to Model.
                books.Add(new Book
                {
                    id = int.Parse(node["Id"].InnerText),
                    Title = node["Title"].InnerText,
                    Author = node["Author"].InnerText,
                    ISBN = node["ISBN"].InnerText,
                    Publisher = node["Publisher"].InnerText,
                    Year = int.Parse(node["Year"].InnerText)
                });
            }
            if (query.Length > 0)
            {
                HashSet<int> index = new HashSet<int>();
                for (int i = 0; i < books.Count; i++)
                {
                    if (books[i].Title.ToLower().Contains(query) || books[i].Author.ToLower().Contains(query) || books[i].ISBN.Contains(query))
                    {
                        index.Add(i);
                    }
                }
                foreach (var item in index)
                {
                    results.Add(books[item]);
                }
            }
            else results = books;
            
            return results;
        }

        public ActionResult Edit(string id)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            Book book = new Book();
            foreach (XmlNode node in xmldoc.SelectNodes("/Inventory/Book"))
            {
                string name = node["Id"].InnerText;
                if (node["Id"].InnerText == id)
                {
                    Book model = new Book
                    {
                        id = int.Parse(node["Id"].InnerText),
                        Title = node["Title"].InnerText,
                        Author = node["Author"].InnerText,
                        ISBN = node["ISBN"].InnerText,
                        Publisher = node["Publisher"].InnerText,
                        Year = int.Parse(node["Year"].InnerText)
                    };
                    book = model;
                }
            }


            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            foreach (XmlNode node in xmldoc.SelectNodes("/Inventory/Book"))
            {
                if (int.Parse(node["Id"].InnerText) == book.id)
                {
                    node["Title"].InnerText = book.Title;
                    node["Author"].InnerText = book.Author;
                    node["ISBN"].InnerText = book.ISBN;
                    node["Publisher"].InnerText = book.Publisher;
                    node["Year"].InnerText = book.Year.ToString();
                }
            }
            xmldoc.Save(Server.MapPath("~/Library.xml"));
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            Book book = new Book();
            foreach (XmlNode node in xmldoc.SelectNodes("/Inventory/Book"))
            {
                string name = node["Id"].InnerText;
                if (node["Id"].InnerText == id)
                {
                    Book model = new Book
                    {
                        id = int.Parse(node["Id"].InnerText),
                        Title = node["Title"].InnerText,
                        Author = node["Author"].InnerText,
                        ISBN = node["ISBN"].InnerText,
                        Publisher = node["Publisher"].InnerText,
                        Year = int.Parse(node["Year"].InnerText)
                    };
                    book = model;
                }
            }
            return View(book);
        }

        public ActionResult Delete(string id)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            Book book = new Book();
            foreach (XmlNode node in xmldoc.SelectNodes("/Inventory/Book"))
            {
                string name = node["Id"].InnerText;
                if (node["Id"].InnerText == id)
                {
                    Book model = new Book
                    {
                        id = int.Parse(node["Id"].InnerText),
                        Title = node["Title"].InnerText,
                        Author = node["Author"].InnerText,
                        ISBN = node["ISBN"].InnerText,
                        Publisher = node["Publisher"].InnerText,
                        Year = int.Parse(node["Year"].InnerText)
                    };
                    book = model;
                }
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Delete(Book book)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            foreach (XmlNode node in xmldoc.SelectNodes("/Inventory/Book"))
            {
                if (node["Id"].InnerText == book.id.ToString())
                {
                    node.ParentNode.RemoveChild(node);
                }
                    
            }
            xmldoc.Save(Server.MapPath("~/Library.xml"));
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Server.MapPath("~/Library.xml"));
            XmlElement ParentElement = xmldoc.CreateElement("Book");
            XmlElement id = xmldoc.CreateElement("Id");
            id.InnerText = book.id.ToString();
            XmlElement title = xmldoc.CreateElement("Title");
            title.InnerText = book.Title;
            XmlElement author = xmldoc.CreateElement("Author");
            author.InnerText = book.Author;
            XmlElement isbn = xmldoc.CreateElement("ISBN");
            isbn.InnerText = book.ISBN;
            XmlElement publisher = xmldoc.CreateElement("Publisher");
            publisher.InnerText = book.Publisher;
            XmlElement year = xmldoc.CreateElement("Year");
            year.InnerText = book.Year.ToString();

            ParentElement.AppendChild(id);
            ParentElement.AppendChild(title);
            ParentElement.AppendChild(author);
            ParentElement.AppendChild(isbn);
            ParentElement.AppendChild(publisher);
            ParentElement.AppendChild(year);
            xmldoc.DocumentElement.AppendChild(ParentElement);
            xmldoc.Save(Server.MapPath("~/Library.xml"));
            return RedirectToAction("Index");
        }
    }
}