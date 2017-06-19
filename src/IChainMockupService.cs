using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace chained_mockup_generator
{
    [ServiceContract]
    public interface IChainMockupService
    {        
        [OperationContract]
        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "test")]
        string HelloWorld();

        [OperationContract]        
        [WebGet(UriTemplate = "/script?Companyname={Companyname}&Phone_no={Phone_no}&Street={Street}&Zip={Zip}&City={City}&Accentcolor={Accentcolor}&Textcolor={Textcolor}&Mainpicture={Mainpicture}&Button_CTA_Text={Button_CTA_Text}&ID={ID}&LogoImageURL={LogoImageURL}", ResponseFormat = WebMessageFormat.Json)]        
        string ChainMockup(string Companyname, string Phone_no, string Street, string Zip, string City, string Accentcolor, string Textcolor, string Mainpicture, string Button_CTA_Text, string ID, string LogoImageURL);


       //localhost:57978/ChainMockupService.svc/script?Companyname=Anna%20Didden&Phone_no=798115437&Street=Gerbergasse%2014&Zip=4001&City=Basel&Accentcolor=%2347b403&Textcolor=%23444444&Mainpicture=http%3A%2F%2Fplacehold.it%2F860x500%2F3369E8%2Fffffff&Button_CTA_Text=Jetzt%20zum%20Angebot&ID=1&LogoImageURL=

        //localhost:57978/ChainMockupService.svc/script?Companyname=Anna Didden&Phone_no=798115437&Street=Gerbergasse 14&Zip=4001&City=Basel&Accentcolor=#47b403&Textcolor=#444444&Mainpicture=http://placehold.it/860x500/3369E8/ffffff&Button_CTA_Text=Jetzt zum Angebot&ID=1&LogoImageURL=

        //ChainMockup(string Companyname = "Anna Didden", string Phone_no = "798115437", string Street = "Gerbergasse 14", string Zip = "4001", string City = "Basel", string Accentcolor = "#47b403", string Textcolor = "#444444", string Mainpicture = "http://placehold.it/860x500/3369E8/ffffff", string Button_CTA_Text = "Jetzt zum Angebot", string ID = "1", string LogoImageURL = "")

        //- Request: curl params // Example: curl
        //localhost://script?Companyname=‘Anna Didden’&Phonenumber’798115437’..
        //- Response: localhost/filepath.jpg // Example:

    }


    //// Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
