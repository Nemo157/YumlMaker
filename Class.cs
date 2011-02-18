using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YumlMaker
{
    class Class
    {
        string name;
        IDictionary<string, string> styles;
        IList<string> bases;
        IList<string> properties;
        IList<string> methods;
        IList<Relationship> relationships;

        public Class(IDictionary klassYaml)
        {
            this.name = ((string)klassYaml["name"]).Escape();

            var yamlStyles = klassYaml["styles"] as IDictionary;
            if (yamlStyles != null)
            {
                this.styles = new Dictionary<string, string>();
                foreach (DictionaryEntry pair in yamlStyles)
                {
                    this.styles.Add(((string)pair.Key).Escape(), ((string)pair.Value).Escape());
                }

            }

            var yamlBases = klassYaml["bases"] as IList;
            if (yamlBases != null)
            {
                this.bases = new List<string>();
                foreach (string s in yamlBases)
                {
                    this.bases.Add(s.Escape());
                }
            }

            var yamlProps = klassYaml["properties"] as IList;
            if (yamlProps != null)
            {
                this.properties = new List<string>();
                foreach (string s in yamlProps)
                {
                    this.properties.Add(s.Escape());
                }
            }

            var yamlMethods = klassYaml["methods"] as IList;
            if (yamlMethods != null)
            {
                this.methods = new List<string>();
                foreach (string s in yamlMethods)
                {
                    this.methods.Add(s.Escape());
                }
            }

            this.relationships = new List<Relationship>();

            var yamlCompositions = klassYaml["compositions"] as IList;
            if (yamlCompositions != null)
            {
                foreach (IDictionary compositionYaml in yamlCompositions)
                {
                    this.relationships.Add(new Composition(compositionYaml));
                }
            }

            var yamlAggregations = klassYaml["aggregations"] as IList;
            if (yamlAggregations != null)
            {
                foreach (IDictionary aggregationYaml in yamlAggregations)
                {
                    this.relationships.Add(new Aggregation(aggregationYaml));
                }
            }

            var yamlUses = klassYaml["uses"] as IList;
            if (yamlUses != null)
            {
                foreach (IDictionary useYaml in yamlUses)
                {
                    this.relationships.Add(new Use(useYaml));
                }
            }
        }

        public string ToMainPart()
        {
            return this.GenMainPart();
        }

        public string ToRestPart()
        {
            var s = new StringBuilder();
            var basesPart = this.GenBasesPart();
            var relationshipsPart = this.GenRelationshipPart();

            if (!String.IsNullOrEmpty(basesPart))
            {
                s.AppendFormat("{0}", basesPart);
            }

            if (!String.IsNullOrEmpty(basesPart) && !String.IsNullOrEmpty(relationshipsPart))
            {
                s.Append(",");
            }

            if (!String.IsNullOrEmpty(relationshipsPart))
            {
                s.AppendFormat("{0}", relationshipsPart);
            }

            return s.ToString();
        }

        private string GenRelationshipPart()
        {
            if (this.relationships != null && this.relationships.Count > 0)
            {
                return this.relationships
                    .Select(relationship => relationship.ToUrlPart(this.name))
                    .Aggregate((acc, next) => String.Format("{0},{1}", acc, next));
            }
            else
            {
                return string.Empty;
            }
        }

        private string GenBasesPart()
        {
            if (this.bases != null && this.bases.Count > 0)
            {
                return this.bases
                    .Select(s => String.Format("[{0}]^[{1}]", s, this.name))
                    .Aggregate((acc, next) => String.Format("{0},{1}", acc, next));
            }
            else
            {
                return string.Empty;
            }
        }

        private string GenMainPart()
        {
            string props = string.Empty;
            string methods = string.Empty;
            string styles = string.Empty;

            if (this.properties != null && this.properties.Count > 0)
            {
                props = this.properties
                    .Aggregate((acc, next) => String.Format("{0};{1}", acc, next));
            }

            if (this.methods != null && this.methods.Count > 0)
            {
                methods = this.methods
                    .Aggregate((acc, next) => String.Format("{0};{1}", acc, next));
            }

            if (this.styles != null && this.styles.Count > 0)
            {
                styles = String.Format("{{{0}}}",
                    this.styles
                        .Select(pair => String.Format("{0}:{1}", pair.Key, pair.Value))
                        .Aggregate((acc, next) => String.Format("{0},{1}", acc, next))
                );
            }

            if (String.IsNullOrEmpty(props))
            {
                if (String.IsNullOrEmpty(methods))
                {
                    return String.Format("[{0}{1}]", this.name, styles);
                }
                else
                {
                    return String.Format("[{0}||{1}{2}]", this.name, methods, styles);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(methods))
                {
                    return String.Format("[{0}|{1}{2}]", this.name, props, styles);
                }
                else
                {
                    return String.Format("[{0}|{1}|{2}{3}]", this.name, props, methods, styles);
                }
            }
        }
    }

    abstract class Relationship
    {
        protected string name;
        protected string num;
        protected string title;

        public Relationship(IDictionary yaml)
        {
            this.name = ((string)yaml["name"]).Escape();

            this.num = ((string)yaml["num"]).Escape();

            this.title = ((string)yaml["title"]).Escape();
        }

        internal virtual string ToUrlPart(string baseName)
        {
            return string.Format("[{0}]{1}-{2}{3}>[{4}]", baseName, this.StartString(), this.num, String.IsNullOrEmpty(this.num) || String.IsNullOrEmpty(this.title) ? this.title : " " + this.title, this.name);
        }

        protected abstract string StartString();
    }

    class Use : Relationship
    {
        public Use(IDictionary yaml)
            : base(yaml)
        {
        }

        protected override string StartString()
        {
            return "";
        }
    }

    class Aggregation : Relationship
    {
        public Aggregation(IDictionary yaml)
            : base(yaml)
        {
        }

        protected override string StartString()
        {
            return "+";
        }
    }

    class Composition : Relationship
    {
        public Composition(IDictionary yaml)
            : base(yaml)
        {
        }

        protected override string StartString()
        {
            return "++";
        }
    }

    public static class Extensions
    {
        public static string Escape(this string p)
        {
            return p == null ? null
                 : p.Replace('(', '（')
                    .Replace(')', '）')
                    .Replace('<', '＜')
                    .Replace('>', '＞')
                    .Replace(',', '，');
        }
    }
}
