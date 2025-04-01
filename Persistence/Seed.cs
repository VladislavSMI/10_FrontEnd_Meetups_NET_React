using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any() && !context.Activities.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@test.com"
                    },
                };

                foreach (var user in users)
                {

                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }



                var activities = new List<Activity>
                {
                    new Activity
                    {
                        Title = "Introduction to front-end web development",
                        Date = DateTime.Now.AddMonths(2),
                        Description = "Front-end web development",
                        Category = "frontend",
                        City = "Luxembourg",
                        Venue = "Zoom",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            }
                        }
                    },
                    new Activity
                    {
                        Title = "Introduction to back-end web development",
                        Date = DateTime.Now.AddMonths(3),
                        Description = "Back-end web development",
                        Category = "backend",
                        City = "Luxembourg",
                        Venue = "Zoom",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            }
                        }
                    },
                    new Activity
                    {
                        Title = "Introduction to full-stack web development",
                        Date = DateTime.Now.AddMonths(1),
                        Description = "Full-stack web development",
                        Category = "fullstack",
                        City = "Luxembourg",
                        Venue = "Zoom",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            }
                        }
                    },

                    new Activity
                    {
                        Title = "JavaScript meetups",
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "General JavaScript meeting",
                        Category = "javascript",
                        City = "Luxembourg",
                        Venue = "Zoom",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            }
                        }
                    },
                    new Activity
                    {
                        Title = "React meetups",
                        Date = DateTime.Now.AddMonths(-1),
                        Description = "General React meeting",
                        Category = "react",
                        City = "Paris",
                        Venue = "Zoom",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Angular meetups",
                        Date = DateTime.Now.AddMonths(3),
                        Description = "General Angular meeting",
                        Category = "angular",
                        City = "Paris",
                        Venue = "Zoom",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Back-end with NodeJS",
                        Date = DateTime.Now.AddMonths(1),
                        Description = "Intorduction to NodeJS as back-end for React app",
                        Category = "nodejs",
                        City = "London",
                        Venue = "Cowork Garden park",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Back-end with C# .net",
                        Date = DateTime.Now.AddMonths(2),
                        Description = "Introduction to C# .net back-end services for React app",
                        Category = "net",
                        City = "Amsterdam",
                        Venue = "Cohost venues",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "UseEffect and UseState in React",
                        Date = DateTime.Now.AddMonths(3),
                        Description = "Dive deep into two most common React hooks",
                        Category = "react",
                        City = "Amsterdam",
                        Venue = "Channel meetups group",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Next Steps for React",
                        Date = DateTime.Now.AddMonths(4),
                        Description = "Discussion about future of React",
                        Category = "react",
                        City = "Berlin",
                        Venue = "Fly High coworks",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = true
                            }
                        }
                    },
                    new Activity
                    {
                        Title = "Things that every JavaScript developer must know",
                        Date = DateTime.Now.AddMonths(5),
                        Description = "What every JavaScript developer must know in oder to be effective in development",
                        Category = "javascript",
                        City = "Vienna",
                        Venue = "General cowork Vienna",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Thnigs JavaScript developers should know",
                        Date = DateTime.Now.AddMonths(6),
                        Description = "Things JavaScript developer should know",
                        Category = "javascript",
                        City = "Berlin",
                        Venue = "Uptown cowork",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "The ultimate guide to JavaScipt testing",
                        Date = DateTime.Now.AddMonths(7),
                        Description = "How to test",
                        Category = "javascript",
                        City = "Berlin",
                        Venue = "All purpose meetups room",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Understanding and using ES modules in NodeJS",
                        Date = DateTime.Now.AddMonths(8),
                        Description = "Activity 8 months in future",
                        Category = "nodejs",
                        City = "London",
                        Venue = "Cowork next to tree",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    }
                };

                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
        }
    }
}
