
namespace bioTCache.Firebase
{
    using System;
    using System.Net.Http;

    public class FirebaseDB
    {
        public string RootNode { get; set; }
       
        public FirebaseDB(string i_BaseURL)
        {
            this.RootNode = i_BaseURL;
        }

        public FirebaseDB Node(string i_Node)
        {
            if (i_Node.Contains("/"))
            {
                throw new FormatException("Node must not contain '/', use NodePath instead.");
            }

            return new FirebaseDB(this.RootNode + '/' + i_Node);
        }

        public FirebaseDB NodePath(string i_NodePath)
        {
            return new FirebaseDB(this.RootNode + '/' + i_NodePath);
        }
      
        public FirebaseResponse Get()
        {
            return new FirebaseRequest(HttpMethod.Get, RootNode).Execute();
        }

        public FirebaseResponse Put(string i_Json)
        {
            return new FirebaseRequest(HttpMethod.Put, RootNode, i_Json).Execute();
        }

        public FirebaseResponse Post(string i_Json)
        {
            return new FirebaseRequest(HttpMethod.Post, RootNode, i_Json).Execute();
        }

        public FirebaseResponse Delete()
        {
            return new FirebaseRequest(HttpMethod.Delete, this.RootNode).Execute();
        }

        public override string ToString()
        {
            return this.RootNode;
        }
    }
}
