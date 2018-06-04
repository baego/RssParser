using System;
using System.Data.Entity;
using System.Linq;
using System.Xml;

namespace RssParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string link = "C:\\Интерфакс.xml";

            SampleContext context = new SampleContext();
            Parser parser = new Parser();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Table]");
            context.SaveChanges();
            parser.getNewArticles(link);

            foreach (DB.Table article in context.News)
            {
                Console.WriteLine("Новость №" + article.Id + "\n" 
                    + article.Head + "\n" + article.Synopsis + "\n"
                    + "Ссылка:" + article.Link + "\n" + "\n");
            }
            Console.ReadLine();
        }
    }
   
        public class Parser
        {
            SampleContext context = new SampleContext();
            string head;
            string syn;
            string link;

            int countNum;
            public bool getNewArticles(string fileSource)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileSource);

                    XmlNodeList nodeList;
                    XmlNode root = doc.DocumentElement;
                    nodeList = root.ChildNodes;
                    int count = 0;

                    foreach (XmlNode chanel in nodeList)
                    {
                        foreach (XmlNode chanel_item in chanel)
                        {
                            if (chanel_item.Name == "item")
                            {
                                XmlNodeList itemsList = chanel_item.ChildNodes;

                                foreach (XmlNode item in itemsList)
                                {
                                    if (item.Name == "title")
                                    {
                                        head = item.InnerText;
                                    }
                                    if (item.Name == "link")
                                    {
                                        link = item.InnerText;
                                    }
                                    if (item.Name == "description")
                                    {
                                        syn = item.InnerText;
                                    }
                                    context.News.Add(new DB.Table()
                                    {
                                        Id = count,
                                        Head = head,
                                        Link = link,
                                        Synopsis = syn
                                    });

                                    context.SaveChanges();
                                }
                                count += 1;
                            }


                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
           
        }
    public class SampleContext : DbContext
    {
        public SampleContext() : base("RssDB")
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<SampleContext>());
        }

        public DbSet<DB.Table> News { get; set; }
    }

}


