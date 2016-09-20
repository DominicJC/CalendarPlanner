/*
 * Data importing utility class.
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Planner.Objects;


namespace Planner.DataLayer
{
    internal class DataLayer
    {
        private static string _connString;
        private static string _sql;

        public static string ConnectionString
        {
            get {
                return _connString ??
                       (_connString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString);
            }
        }

        public static List<Person> GetBirthdays()
        {
            List<Person> birthdays = new List<Person>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                _sql = @"select [FirstName], [LastName], [DOB], [Display] from [Person]";
                
                using(SqlCommand command = new SqlCommand(_sql, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        {
                            Person birthday = new Person
                            {
                                FirstName = reader.GetString(0),
                                LastName = reader.GetString(1),
                                Dob = reader.GetDateTime(2),
                                Display = reader.GetBoolean(3)
                            };
                            birthdays.Add(birthday);
                        }
                    }
                }
            }

            return birthdays;
        }

        public static List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                _sql = @"select * from [Event]";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        {
                            Event newEvent = new Event
                            {
                                Id = reader.GetInt32(0),
                                EventType = reader.GetString(1),
                                EventName = reader.GetString(2),
                                StartDate = reader.GetDateTime(3),
                                EndDate = reader.GetDateTime(4)
                            };
                            if (!reader.IsDBNull(6))
                            {
                                newEvent.Notes = reader.GetString(6);
                            }
                            events.Add(newEvent);
                        }
                    }
                }
            }

            return events;
        }

        public static List<Event> GetHomePageEvents(DateTime start, DateTime end)
        {
            List<Event> events = new List<Event>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                _sql = @"select * from [Event] where [StartDate] between @start and @end";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("start", start.Date);
                    command.Parameters.AddWithValue("end", end.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        {
                            Event newEvent = new Event
                            {
                                Id = reader.GetInt32(0),
                                EventType = reader.GetString(1),
                                EventName = reader.GetString(2),
                                StartDate = reader.GetDateTime(3),
                                EndDate = reader.GetDateTime(4)
                            };
                            if (!reader.IsDBNull(6))
                            {
                                newEvent.Notes = reader.GetString(6);
                            }
                            events.Add(newEvent);
                        }
                    }
                }
            }

            return events;
        }

        public static Event GetEvent(int id)
        {
            Event newEvent = new Event();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                _sql = @"select * from [Event] where ID=@id";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        { 
                            newEvent.Id = reader.GetInt32(0);
                            newEvent.EventType = reader.GetString(1);
                            newEvent.EventName = reader.GetString(2);
                            newEvent.StartDate = reader.GetDateTime(3);
                            newEvent.EndDate = reader.GetDateTime(4);
                            if (!reader.IsDBNull(6))
                            {
                                newEvent.Notes = reader.GetString(6);
                            }                        
                        }
                    }
                }
            }

            return newEvent;
        }

        public static List<Person> GetContacts()
        {
            List<Person> contacts = new List<Person>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                _sql = @"Select * from [Person]";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        {
                            Person newPerson = new Person
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Dob = reader.GetDateTime(3),
                                Display = reader.GetBoolean(4),
                                Address = reader.GetString(5),
                                Suburb = reader.GetString(6),
                                Town = reader.GetString(7),
                                Country = reader.GetString(8),
                                Postcode = reader.GetString(9),
                                IsUser = reader.GetBoolean(10)
                            };
                            contacts.Add(newPerson);
                        }
                    }
                }
            }

            return contacts;
        }

        public static Dictionary<int, string> GetUser()
        {
            Dictionary<int, string> users = new Dictionary<int, string>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                _sql = @"Select [ID], [FirstName] from [Person] Where [IsUser]=1";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        {
                            Person currentUser = new Person
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1)
                            };
                            users.Add(currentUser.Id, currentUser.FirstName);
                        }
                    }
                }
            }

            return users;
        }

        public static void AddEvent(Event events)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                _sql = @"Insert into [Event] (EventType, EventName, StartDate, EndDate, Notes)
                Values (@EventType, @EventName, @StartDate, @EndDate, @Notes);";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("EventType", events.EventType);
                    command.Parameters.AddWithValue("EventName", events.EventName);
                    command.Parameters.AddWithValue("StartDate", events.StartDate);
                    command.Parameters.AddWithValue("EndDate", events.EndDate);
                    // Set DBNull if no notes
                    SqlParameter noteParam = new SqlParameter("Notes", System.Data.SqlDbType.VarChar);
                    if (events.Notes == null)
                    {
                        noteParam.Value = DBNull.Value;
                    }
                    else noteParam.Value = events.Notes;
                    command.Parameters.Add(noteParam);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateEvent(Event events)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                _sql = @"UPDATE [Event] 
                SET EventType = @EventType, EventName = @EventName, StartDate = @StartDate, EndDate = @EndDate, Notes = @Notes
                WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("EventType", events.EventType);
                    command.Parameters.AddWithValue("EventName", events.EventName);
                    command.Parameters.AddWithValue("StartDate", events.StartDate);
                    command.Parameters.AddWithValue("EndDate", events.EndDate);
                    // Set DBNull if no notes
                    SqlParameter noteParam = new SqlParameter("Notes", System.Data.SqlDbType.VarChar);
                    if (events.Notes == null)
                    {
                        noteParam.Value = DBNull.Value;
                    }
                    else noteParam.Value = events.Notes;
                    command.Parameters.Add(noteParam);
                    command.Parameters.AddWithValue("ID", events.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteEvent(Event events)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                _sql = @"DELETE FROM [Event] 
                WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("ID", events.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddPerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                _sql = @"Insert into [Person] (FirstName, LastName, DOB, Display, Address, Suburb, Town, Country, PostCode, IsUser)
                Values (@FirstName, @LastName, @DOB, @Display, @Address, @Suburb, @Town, @Country, @PostCode, @IsUser);";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("FirstName", person.FirstName);
                    command.Parameters.AddWithValue("LastName", person.LastName);
                    command.Parameters.AddWithValue("DOB", person.Dob);
                    command.Parameters.AddWithValue("Display", person.Display);
                    command.Parameters.AddWithValue("Address", person.Address);
                    command.Parameters.AddWithValue("Suburb", person.Suburb);
                    command.Parameters.AddWithValue("Town", person.Town);
                    command.Parameters.AddWithValue("Country", person.Country);
                    // Set DBNull if no postcode.
                    SqlParameter postCodeParam = new SqlParameter("PostCode", System.Data.SqlDbType.VarChar);
                    if (person.Postcode == null)
                    {
                        postCodeParam.Value = DBNull.Value;
                    }
                    else postCodeParam.Value = person.Postcode;
                    command.Parameters.Add(postCodeParam);
                    command.Parameters.AddWithValue("IsUser", person.IsUser);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdatePerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                _sql = @"UPDATE [Person] 
                SET FirstName = @FirstName, LastName = @LastName, DOB = @DOB, Display = @Display, Address = @Address, 
                Suburb = @Suburb, Town = @Town, Country = @Country, PostCode = @PostCode, IsUser = @IsUser
                WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("FirstName", person.FirstName);
                    command.Parameters.AddWithValue("LastName", person.LastName);
                    command.Parameters.AddWithValue("DOB", person.Dob);
                    command.Parameters.AddWithValue("Display", person.Display);
                    command.Parameters.AddWithValue("Address", person.Address);
                    command.Parameters.AddWithValue("Suburb", person.Suburb);
                    command.Parameters.AddWithValue("Town", person.Town);
                    command.Parameters.AddWithValue("Country", person.Country);
                    // Set DBNull if no postcode.
                    SqlParameter postCodeParam = new SqlParameter("PostCode", System.Data.SqlDbType.VarChar);
                    if (person.Postcode == null)
                    {
                        postCodeParam.Value = DBNull.Value;
                    }
                    else postCodeParam.Value = person.Postcode;
                    command.Parameters.Add(postCodeParam);
                    command.Parameters.AddWithValue("IsUser", person.IsUser);
                    command.Parameters.AddWithValue("ID", person.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeletePerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                _sql = @"DELETE FROM [Person] 
                WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(_sql, connection))
                {
                    command.Parameters.AddWithValue("ID", person.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

