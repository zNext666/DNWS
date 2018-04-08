using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNWS
{
    public class TwitterAPIPlugin : IPlugin
    {
        public HTTPResponse PostProcessing(HTTPResponse response)
        {
            throw new NotImplementedException();
        }

        public void PreProcessing(HTTPRequest request)
        {
            throw new NotImplementedException();
        }

        private StringBuilder GenTimeline(Twitter twitter, StringBuilder sb)
        {
            sb.Append("Say something<br />");
            sb.Append("<form method=\"post\">");
            sb.Append("<input type=\"text\" name=\"message\"></input>");
            sb.Append("<input type=\"submit\" name=\"action\" value=\"tweet\" /> <br />");
            sb.Append("</form>");
            sb.Append("Follow someone<br />");
            sb.Append("<form method=\"post\">");
            sb.Append("<input type=\"text\" name=\"following\"></input>");
            sb.Append("<input type=\"submit\" name=\"action\" value=\"following\" /> <br />");
            sb.Append("</form>");
            sb.Append(String.Format("<h3><b>{0}</b>'s timeline</h3><br />", twitter.GetUsername()));
            List<Tweet> tweets = twitter.GetUserTimeline();
            foreach (Tweet tweet in tweets)
            {
                sb.Append("[" + tweet.DateCreated + "]" + tweet.User + ":" + tweet.Message + "<br />");
            }
            sb.Append("<br /><br />");
            sb.Append("<h3>Following timeline</h3><br />");
            tweets = twitter.GetFollowingTimeline();
            if (tweets == null)
            {
                sb.Append("Your following list is empty, follow someone!");
            }
            else
            {
                foreach (Tweet tweet in tweets)
                {
                    sb.Append("[" + tweet.DateCreated + "] " + tweet.User + ":" + tweet.Message + "<br />");
                }
            }
            return sb;
        }

        protected StringBuilder GenLoginPage(StringBuilder sb)
        {
            sb.Append("<h2>Login</h2>");
            sb.Append("<form method=\"get\">");
            sb.Append("Username: <input type=\"text\" name=\"user\" value=\"\" /> <br />");
            sb.Append("Password: <input type=\"password\" name=\"password\" value=\"\" /> <br />");
            sb.Append("<input type=\"submit\" name=\"action\" value=\"login\" /> <br />");
            sb.Append("</form>");
            sb.Append("<br /><br /><br />");
            sb.Append("<h2>New user</h2>");
            sb.Append("<form method=\"get\">");
            sb.Append("Username: <input type=\"text\" name=\"user\" value=\"\" /> <br />");
            sb.Append("Password: <input type=\"password\" name=\"password\" value=\"\" /> <br />");
            sb.Append("<input type=\"submit\" name=\"action\" value=\"newuser\" /> <br />");
            sb.Append("</form>");
            return sb;
        }


        public HTTPResponse GetResponse(HTTPRequest request)
        {
            HTTPResponse response = new HTTPResponse(200);
            StringBuilder sb = new StringBuilder();

            string[] urls = request.Url.Split("/");
            string[] operators = null;
            if(urls.Length > 2) {
                operators = new string[urls.Length - 2];
                Array.Copy(urls, 2, operators, 0, urls.Length - 2);
                // Remove trailing slash
                if(operators[operators.Length - 1] == "") {
                    if(operators.Length == 1) {
                        operators = null;
                    } else {
                        List<string> foos = new List<string>(operators);
                        foos.RemoveAt(foos.Count - 1);
                        operators = foos.ToArray();
                    }
                }
            }

            if (operators == null) {
                response.status = 301;
                response.AddCustomHeader("Location", "/twitterapi/Login");
            }
            string user = request.getRequestByKey("user");
            string password = request.getRequestByKey("password");
            string action = request.getRequestByKey("action");
            string following = request.getRequestByKey("following");
            string message = request.getRequestByKey("message");


            if (user == null) // no user? show login screen
            {
                sb.Append("<h1>Twitter</h1>");
                sb = GenLoginPage(sb);
            }
            else
            {
                if (action == null) // No action? go to homepage
                {
                    try
                    {
                        Twitter twitter = new Twitter(user);
                        sb.Append(String.Format("<h1>{0}'s Twitter</h1>", user));
                        sb = GenTimeline(twitter, sb);
                    }
                    catch (Exception ex)
                    {
                        sb.Append(String.Format("Error [{0}], please go back to <a href=\"/twitter\">login page</a> to try again", ex.Message));
                    }
                }
                else
                {
                    if (action.Equals("newuser"))
                    {
                        if (user != null && password != null && user != "" && password != "")
                        {
                            try
                            {
                                Twitter.AddUser(user, password);
                                sb.Append("User added successfully, please go back to <a href=\"/twitter\">login page</a> to login");
                            }
                            catch (Exception ex)
                            {
                                sb.Append(String.Format("Error adding user with error [{0}], please go back to <a href=\"/twitter\">login page</a> to try again", ex.Message));
                            }
                        }
                    }
                    else if (action.Equals("login"))
                    {
                        if (user != null && password != null && user != "" && password != "")
                        {
                            if (Twitter.IsValidUser(user, password))
                            {
                                sb.Append(String.Format("Welcome {0}, please go back to <a href=\"/twitter?user={0}\">tweet page</a> to begin", user));
                            }
                            else
                            {
                                sb.Append("Error login, please go back to <a href=\"/twitter\">login page</a> to try again");
                            }
                        }
                    }
                    else
                    {
                        Twitter twitter = new Twitter(user);
                        sb.Append(String.Format("<h1>{0}'s Twitter</h1>", user));
                        if (action.Equals("following"))
                        {
                            try
                            {
                                twitter.AddFollowing(following);
                                sb = GenTimeline(twitter, sb);
                            }
                            catch (Exception ex)
                            {
                                sb.Append(String.Format("Error [{0}], please go back to <a href=\"/twitter\">login page</a> to try again", ex.Message));
                            }
                        }
                        else if (action.Equals("tweet"))
                        {
                            try
                            {
                                twitter.PostTweet(message);
                                sb = GenTimeline(twitter, sb);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                sb.Append(String.Format("Error [{0}], please go back to <a href=\"/twitter\">login page</a> to try again", ex.Message));
                            }
                        }
                    }
                }
            }
            response.body = Encoding.UTF8.GetBytes(sb.ToString());
            return response;
        }
    }
}