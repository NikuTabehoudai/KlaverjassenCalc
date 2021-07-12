using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using KlaverjassenCalc.Model;

namespace KlaverjassenCalc
{
    public static class SaveState
    {


        public static void XmlSave(ObservableCollection<InputDataList> InputDataList)
        {

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fileName = "/SaveState.xml";
            string fullpath = path + fileName;
            XmlTextWriter write = new XmlTextWriter(fullpath, System.Text.Encoding.UTF8);
            write.Formatting = Formatting.Indented;

            write.WriteStartDocument();
            write.WriteStartElement("KlaverjassenCalc");
            write.WriteStartElement("InpuDataLis");

            foreach (var item in InputDataList)
            {
                write.WriteStartElement("Rondes");
                write.WriteElementString("Ronde", item.Ronde);
                write.WriteElementString("PuntenWij", item.PuntenWij);
                write.WriteElementString("RoemWij", item.RoemWij.ToString());
                write.WriteElementString("PuntenZij", item.PuntenZij); ;
                write.WriteElementString("RoemZij", item.RoemZij.ToString());
                write.WriteElementString("RoemWijSave", item.RoemWijSave.ToString());
                write.WriteElementString("RoemZijSave", item.RoemZijSave.ToString());
                write.WriteEndElement();

            }
            write.WriteEndElement();
            write.WriteEndElement();
            write.WriteEndDocument();
            write.Flush();
            write.Close();

        }

        public static ObservableCollection<InputDataList> XmlLoad()
        {
            ObservableCollection<InputDataList> InputList = new ObservableCollection<InputDataList>();

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fileName = "/SaveState.xml";
            string fullpath = path + fileName;
            XmlDocument xdoc = new XmlDocument();

            try
            {
                xdoc.Load(fullpath);

            XmlNodeList list = xdoc.GetElementsByTagName("Rondes");
            List<string> L = new List<string>();
            //List<string> Lpw = new List<string>();
            //List<string> Lpz = new List<string>();
            //List<string> Lrw = new List<string>();
            //List<string> Lrz = new List<string>();
            //List<string> Lrws = new List<string>();
            //List<string> Lrzs = new List<string>();



            foreach (XmlNode i in list)
            {
                foreach (XmlNode ii in i)
                {
                    switch (ii.Name)
                    {
                        case "Ronde":
                            L.Add(ii.InnerText);
                            break;

                        case "PuntenWij":
                            L.Add(ii.InnerText);
                            break;

                        case "PuntenZij":
                            L.Add(ii.InnerText);
                            break;

                        case "RoemWij":
                            L.Add(ii.InnerText);
                            break;

                        case "RoemZij":
                            L.Add(ii.InnerText);
                            break;

                        case "RoemWijSave":
                            L.Add(ii.InnerText);
                            break;

                        case "RoemZijSave":
                            L.Add(ii.InnerText);
                            break;
                    }



                }
                InputList.Add(new InputDataList() { Ronde = L[0], PuntenWij = L[1], RoemWij = Int16.Parse(L[2]), PuntenZij = L[3], RoemZij = Int16.Parse(L[4]), RoemWijSave = Int16.Parse(L[5]), RoemZijSave = Int16.Parse(L[6]) });
                L.Clear();

            }
        }
            catch (Exception)
            {
                return InputList;

            }
            return InputList;
        }

        public static void RemoveXml()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fileName = "/SaveState.xml";
            string fullpath = path + fileName;

            File.Delete(fullpath);

        }
       
    }
}
