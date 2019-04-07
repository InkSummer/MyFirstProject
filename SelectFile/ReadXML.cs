using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SelectFile
{
    class ReadXML
    {
        private string Path { get; set; }

        public List<Rule> Rules { get; set; }

        private XmlDocument RuleXML { get; set; }

        public ReadXML()
        {
            Path = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FolderChange.xml";
            ReadFile();
            Rules = ReadXMLRules();
        }


        /// <summary>
        /// 读取RuleXML
        /// </summary>
        /// <returns></returns>
        private List<Rule> ReadXMLRules()
        {
            List<Rule> list = new List<Rule>();
            if (RuleXML == null) return list;
            XmlNode node = RuleXML.SelectSingleNode("\root");
            XmlNodeList childList = node.ChildNodes;

            foreach (XmlNode item in childList)
            {
                var rule = new Rule();
                rule.Type = Convert.ToInt32(item.Attributes["Type"].Value.ToString());
                rule.Checked = Convert.ToBoolean(item.Attributes["Checked"].Value.ToString());
                rule.RegexRule = Convert.ToString(item.Attributes["RegexRule"].Value.ToString());
                rule.Str = Convert.ToString(item.Attributes["Str"].Value.ToString());
                rule.RepalceStr = Convert.ToString(item.Attributes["RepalceStr"].Value.ToString());
                list.Add(rule);
            }

            return list;
        }

        public bool SaveXMLRulesToFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement Root = doc.CreateElement("Root");
                foreach (var item in Rules)
                {
                    XmlElement node = doc.CreateElement("Rule");

                    XmlAttribute type = doc.CreateAttribute("Type");
                    type.Value = item.Type.ToString();
                    node.SetAttributeNode(type);

                    XmlAttribute Checked = doc.CreateAttribute("Checked");
                    Checked.Value = item.Checked.ToString();
                    node.SetAttributeNode(Checked);

                    XmlAttribute RegexRule = doc.CreateAttribute("RegexRule");
                    RegexRule.Value = item.RegexRule.ToString();
                    node.SetAttributeNode(RegexRule);

                    XmlAttribute RepalceStr = doc.CreateAttribute("RepalceStr");
                    RepalceStr.Value = item.RepalceStr.ToString();
                    node.SetAttributeNode(RepalceStr);

                    XmlAttribute Str = doc.CreateAttribute("Str");
                    Str.Value = item.Str.ToString();
                    node.SetAttributeNode(Str);

                    Root.AppendChild(node);
                }

                doc.Save(Path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ReadFile()
        {
            if (Path == string.Empty) throw new Exception("未找到路径");
            if (Directory.Exists(Path))
            {
                RuleXML = new XmlDocument();
                RuleXML.Load(Path);
            }
            else
            {
                File.Create(Path);
            }
        }
    }
}
