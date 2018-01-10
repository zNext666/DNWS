using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DNWS
{
  public class HTTPRequest
  {
    protected string _url;
    protected string _filename;
    protected static Dictionary<string, string> _propertyListDictionary = null;
    protected static Dictionary<string, string> _requestListDictionary = null;

    protected string _body;

    protected int _status;

    protected string _method;

    public string Url
    {
      get { return _url;}
    }

    public string Filename
    {
      get { return _filename;}
    }

    public string Body
    {
      get {return _body;}
    }

    public int Status
    {
      get {return _status;}
    }

    public string Method
    {
      get {return _method;}
    }
    public HTTPRequest(string request)
    {
      _propertyListDictionary = new Dictionary<string, string>();
      string[] lines = Regex.Split(request, "\\n");

      if(lines.Length == 0) {
        _status = 500;
        return;
      }

      string[] statusLine = Regex.Split(lines[0], "\\s");
      if(statusLine.Length != 4) { // too short something is wrong
        _status = 401;
        return;
      }
      if (!statusLine[0].ToLower().Equals("get"))
      {
        _method = "GET";
      } else if(!statusLine[0].ToLower().Equals("post")) {
        _method = "POST";
      } else {
        _status = 501;
        return;
      }
      _status = 200;

      _url = statusLine[1];
      string[] urls = Regex.Split(_url, "/");
      _filename = urls[urls.Length - 1];
      string[] parts = Regex.Split(_filename, "[?]");
      if (parts.Length > 1)
      {
        //Ref: http://stackoverflow.com/a/4982122
        _requestListDictionary = parts[1].Split('&').Select(x => x.Split('=')).ToDictionary(x => x[0].ToLower(), x => x[1]);
      } else{
        _requestListDictionary = new Dictionary<string, string>();
      }

      if(lines.Length == 1) return;

      for(int i = 1; i != lines.Length; i++) {
        string[] pair = Regex.Split(lines[i], ": "); //FIXME
        if(pair.Length == 0) continue;
        if(pair.Length == 1) { // handle post body
          if(pair[0].Length > 1) { //FIXME, this is a quick hack
            Dictionary<string, string> _bodys = pair[0].Split('&').Select(x => x.Split('=')).ToDictionary(x => x[0].ToLower(), x => x[1]);
            _requestListDictionary = _requestListDictionary.Concat(_bodys).ToDictionary(x=>x.Key, x=>x.Value);
          }
        } else { // Length == 2, GET url request
          addProperty(pair[0], pair[1]);
        }
      }
    }
    public string getPropertyByKey(string key)
    {
      if(_propertyListDictionary.ContainsKey(key.ToLower())) {
        return _propertyListDictionary[key.ToLower()];
      } else {
        return null;
      }
    }
    public string getRequestByKey(string key)
    {
      if(_requestListDictionary.ContainsKey(key.ToLower())) {
        return _requestListDictionary[key.ToLower()];
      } else {
        return null;
      }
    }

    public void addProperty(string key, string value)
    {
      _propertyListDictionary[key.ToLower()] = value;
    }
    public void addRequest(string key, string value)
    {
      _requestListDictionary[key.ToLower()] = value;
    }
  }
}