using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;//dotnet add package Newtonsoft.Json --version 12.0.2
//ref https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.0&tabs=visual-studio
namespace DNWS
{
    public class TwitterAPI : IPlugin
    {
        Twitter app = null;

        public HTTPResponse PostProcessing(HTTPResponse response)
        {
            throw new NotImplementedException();
        }

        public void PreProcessing(HTTPRequest request)
        {
            throw new NotImplementedException();
        }
        //response normal html
        public HTTPResponse GetResponse(HTTPRequest request)
        {
            HTTPResponse response = null;
            StringBuilder sb = new StringBuilder();
            /*
            if(request.Method == "GET"){//GET
                if(request.Status == 200){  
                }else{
                  response.Status == 400;
                }           
            }else if(request.Method == "POST"){//POST

            }else if(request.Method == "DELETE"){//DELETE

            }else{
                throw new NotImplementedException();
            } 
            */
            sb.Append("<html><head><title>TwitterAPI</title></head><body>");
            sb.Append("<h1>Twitter API</h1>");
            sb.Append((request.Url).ToString());
            sb.Append("</body></html>");
            response = new HTTPResponse(200);
            response.body = Encoding.UTF8.GetBytes(sb.ToString());
            return response;
        }
        }
}