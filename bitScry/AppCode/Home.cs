using bitScry.Models.Home;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.AppCode
{
    public static class Home
    {
        public static List<Blog> GetBlogSummaries(string connectionString)
        {
            List<Blog> blogs = new List<Blog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp__get_blog_summaries"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Blog blog = new Blog();

                            blog.Title = reader[0].ToString();
                            blog.Date = (DateTime)reader[1];
                            blog.Name = reader[2].ToString();

                            blogs.Add(blog);
                        }
                    }

                    connection.Close();
                }
            }

            return blogs;
        }

        public static async Task SendEmail(string fromEmail, string toEmail, string messageText, string subject, string templateId, string apiKey)
        {
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(fromEmail);
            EmailAddress to = new EmailAddress(toEmail);
            string plainTextContent = messageText;
            string htmlContent = messageText;
            SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            message.TemplateId = templateId;
            SendGrid.Response response = await client.SendEmailAsync(message);
        }

        public static string GetProxyUrl(Uri uri)
        {
            string functionUrl = "https://function.bitscry.com/api/imageproxy";
            functionUrl = functionUrl + "?url=" + uri.ToString();

            return functionUrl;
        }

        public static string GetProxyUrl(Uri uri, string code)
        {
            string functionUrl = "https://function.bitscry.com/api/imageproxy";
            functionUrl = functionUrl + "?code=" + code;
            functionUrl = functionUrl + "&url=" + uri.ToString();

            return functionUrl;
        }
    }
}
