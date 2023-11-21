#define DEBUG
namespace gspApiGetter;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;





class Program
{
    public static void Main(string[] args)
    {
       
      
        var getter = new BusTableGetter();
        BusTable[] tables = getter.getBusTableFromWeb("2");
        tables[0].writeOut();
    }
}
