using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YumlMaker
{
    class Class
    {
        string name;
        IDictionary<string, string> styles;
        IList<string> bases;
        IList<string> properties;
        IList<Relationship> relationships;

        public Class(IDictionary klassYaml)
        {
            this.name = (string)klassYaml["name"];

            var yamlStyles = klassYaml["styles"] as IDictionary;
            if (yamlStyles != null)
            {
                this.styles = new Dictionary<string, string>();
                foreach (DictionaryEntry pair in yamlStyles)
                {
                    this.styles.Add((string)pair.Key, (string)pair.Value);
                }

            }

            var yamlBases = klassYaml["bases"] as IList;
            if (yamlBases != null)
            {
                this.bases = new List<string>();
                foreach (string s in yamlBases)
                {
                    this.bases.Add(s);
                }
            }

            var yamlProps = klassYaml["properties"] as IList;
            if (yamlProps != null)
            {
                this.properties = new List<string>();
                foreach (string s in yamlProps)
                {
                    this.properties.Add(s);
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
        }

        public string ToUrlPart()
        {
            var s = new StringBuilder();
            var mainPart = this.GenMainPart();
            var basesPart = this.GenBasesPart();
            var compositionsPart = this.GenRelationshipPart();

            s.AppendFormat("{0}", mainPart);

            if (!String.IsNullOrEmpty(basesPart))
            {
                s.AppendFormat(",{0}", basesPart);
            }

            if (!String.IsNullOrEmpty(compositionsPart))
            {
                s.AppendFormat(",{0}", compositionsPart);
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
            this.name = (string)yaml["name"];

            this.num = (string)yaml["num"];

            this.title = (string)yaml["title"];
        }

        internal virtual string ToUrlPart(string baseName)
        {
            return string.Format("[{0}]{1}-{2}{3}>[{4}]", baseName, this.StartString(), this.num, String.IsNullOrEmpty(num) ? this.title : " " + this.title, this.name);
        }

        protected abstract string StartString();
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
}
