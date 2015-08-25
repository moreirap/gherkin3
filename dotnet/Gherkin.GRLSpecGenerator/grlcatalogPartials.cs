using System.Xml.Serialization;

namespace Gherkin.GRLCatalogueGenerator
{
    public partial class grlcatalogIntentionalelement : IElementWithIdentity     {}

    public partial class grlcatalogActor : IElementWithIdentity {}

    public partial class grlcatalogLinkdefContribution : IElementWithIdentity 
    {
        private string _id;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
    public partial class grlcatalogLinkdefDecomposition : IElementWithIdentity 
    {
        private string _id;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

    public partial class grlcatalogLinkdefDependency : IElementWithIdentity
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string id { get;  set; }
    }

    public partial class grlcatalogActorContIE : IElementWithIdentity
    {
        private string _id;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

    }

    public partial class grlcatalog 
    {
        public static string GetContributionAsQuantity(string contributionType)
        {
            switch (contributionType)
            {
                case "Break":
                    return "-100";
                case "Help":
                    return "25";
                case "Hurt":
                    return "-25";
                case "Make":
                    return "100";
                case "SomeNegative":
                    return "-75";
                case "SomePositive":
                    return "75";
                case "Unknown":
                    return "0";
                default:
                    return "0";
            }
        }
    }
}
