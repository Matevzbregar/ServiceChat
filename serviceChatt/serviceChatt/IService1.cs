using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace serviceChatt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Login", ResponseFormat = WebMessageFormat.Json)]
        bool Login();


        [OperationContract]
        [WebInvoke(UriTemplate = "Send/{message}")]
        void Send(string message);

        [OperationContract(Name = "Vsi")]
        [WebInvoke(UriTemplate = "Messages", ResponseFormat = WebMessageFormat.Json)]
        List<Message> Messages();

        [OperationContract(Name = "Dolocen")]
        [WebGet(UriTemplate = "Messages/{id}", ResponseFormat = WebMessageFormat.Json)]
        List<Message> Messages(string id);
    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int Id { get; set; }
    }
}