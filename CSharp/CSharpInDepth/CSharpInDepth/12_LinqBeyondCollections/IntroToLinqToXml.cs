using CSharpInDepth.Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpInDepth._12_LinqBeyondCollections
{
    class IntroToLinqToXml : Study
    {
        public override string StudyName
        {
            get { return "Introduces basics of Linq to Xml"; }
        }

        protected override void PerformStudy()
        {
            //ListAllUsers();
            //ListProjectAndSubscribersWithUsers();
            RetrieveUsersFromXml();
        }

        private void RetrieveUsersFromXml()
        {
            var element = new XElement("users",
                from user in SampleData.AllUsers
                select
                    new XElement("user",
                            new XAttribute("name", user.Name),
                            new XAttribute("type", user.UserType)
                            )
               );

            // If an attribute is not found, it will silently assign "null" instead of throwing any exception which is very useful as we can write a query without checking
            // whether the attribute is present or not.

            var users = element.Elements().Select(e => new { Name = e.Attribute("name"), UserType = (string)e.Attribute("type") , NotFound=(string)e.Attribute("NotFound")});

            // A simple way to retrieve the users
            users = element.Descendants("user").Select(e => new { Name = e.Attribute("name"), UserType = (string)e.Attribute("type"), NotFound = (string)e.Attribute("NotFound") });

            foreach(var user in users)
            {
                Console.WriteLine("{0}-{1}",user.Name,user.NotFound);
            }
        }

        private void ListProjectAndSubscribersWithUsers()
        {
            var element = new XElement("defect-system");

            var projects = new XElement("projects",
                from project in SampleData.AllProjects
                join subscriber in SampleData.AllSubscriptions 
                        on project equals subscriber.Project
                        into subscriptions
                select new XElement("project", 
                            new XAttribute("name", project.Name)
                            ,new XElement("subscribers", subscriptions.Select(sub=> new XElement("subscriber", new XAttribute("email",sub.EmailAddress))))
                            ));

            var users = new XElement("users",
                        SampleData.AllUsers.Select((user,index)=> 
                            new XElement("user",
                                new XAttribute("name",user.Name),
                                new XAttribute("type",user.UserType),
                                new XAttribute("id",index + 1)
                                )
                        ));

            element.Add(projects, users);
                        
            Console.WriteLine(element.ToString());

        }

        private void ListAllUsers()
        {
            var element = new XElement("users",
                from user in SampleData.AllUsers
                where user.UserType == UserType.Developer
                select
                    new XElement("user",
                            new XAttribute("name", user.Name),
                            new XAttribute("type", user.UserType)
                            )
               );

            Console.WriteLine(element.ToString());
        }
    }
}
