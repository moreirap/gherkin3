﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
namespace Gherkin.GRLCatalogueGenerator
{
    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.33440.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("grl-catalog", Namespace = "", IsNullable = false)]
    public partial class grlcatalog
    {

        private grlcatalogIntentionalelement[] elementdefField;

        private grlcatalogLinkdef linkdefField;

        private grlcatalogActor[] actordefField;

        private grlcatalogActorContIE[] actorIElinkdefField;

        private string catalognameField;

        private string descriptionField;

        private string authorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("element-def", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("intentional-element", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public grlcatalogIntentionalelement[] elementdef
        {
            get
            {
                return this.elementdefField;
            }
            set
            {
                this.elementdefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("link-def", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public grlcatalogLinkdef linkdef
        {
            get
            {
                return this.linkdefField;
            }
            set
            {
                this.linkdefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("actor-def", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("actor", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public grlcatalogActor[] actordef
        {
            get
            {
                return this.actordefField;
            }
            set
            {
                this.actordefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("actor-IE-link-def", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("actorContIE", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public grlcatalogActorContIE[] actorIElinkdef
        {
            get
            {
                return this.actorIElinkdefField;
            }
            set
            {
                this.actorIElinkdefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("catalog-name")]
        public string catalogname
        {
            get
            {
                return this.catalognameField;
            }
            set
            {
                this.catalognameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogIntentionalelement
    {

        private string idField;

        private string typeField;

        private string decompositiontypeField;

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string decompositiontype
        {
            get
            {
                return this.decompositiontypeField;
            }
            set
            {
                this.decompositiontypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogLinkdef
    {

        private grlcatalogLinkdefDependency[] dependencyField;

        private grlcatalogLinkdefDecomposition[] decompositionField;

        private grlcatalogLinkdefContribution[] contributionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dependency", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public grlcatalogLinkdefDependency[] dependency
        {
            get
            {
                return this.dependencyField;
            }
            set
            {
                this.dependencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("decomposition", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public grlcatalogLinkdefDecomposition[] decomposition
        {
            get
            {
                return this.decompositionField;
            }
            set
            {
                this.decompositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contribution", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public grlcatalogLinkdefContribution[] contribution
        {
            get
            {
                return this.contributionField;
            }
            set
            {
                this.contributionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogLinkdefDependency
    {

        private string dependeeidField;

        private string dependeridField;

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string dependeeid
        {
            get
            {
                return this.dependeeidField;
            }
            set
            {
                this.dependeeidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string dependerid
        {
            get
            {
                return this.dependeridField;
            }
            set
            {
                this.dependeridField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogLinkdefDecomposition
    {

        private string srcidField;

        private string destidField;

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string srcid
        {
            get
            {
                return this.srcidField;
            }
            set
            {
                this.srcidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string destid
        {
            get
            {
                return this.destidField;
            }
            set
            {
                this.destidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogLinkdefContribution
    {

        private string srcidField;

        private string destidField;

        private string contributiontypeField;

        private string quantitativeContributionField;

        private bool correlationField;

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string srcid
        {
            get
            {
                return this.srcidField;
            }
            set
            {
                this.srcidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string destid
        {
            get
            {
                return this.destidField;
            }
            set
            {
                this.destidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contributiontype
        {
            get
            {
                return this.contributiontypeField;
            }
            set
            {
                this.contributiontypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string quantitativeContribution
        {
            get
            {
                return this.quantitativeContributionField;
            }
            set
            {
                this.quantitativeContributionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool correlation
        {
            get
            {
                return this.correlationField;
            }
            set
            {
                this.correlationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogActor
    {

        private string idField;

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class grlcatalogActorContIE
    {

        private string actorField;

        private string ieField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string actor
        {
            get
            {
                return this.actorField;
            }
            set
            {
                this.actorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string ie
        {
            get
            {
                return this.ieField;
            }
            set
            {
                this.ieField = value;
            }
        }
    }
}