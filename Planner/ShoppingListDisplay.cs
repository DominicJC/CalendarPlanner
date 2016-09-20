/*
 * Class stores and retrieves shopping list from file.
 * List is serialized to an xml file.
 */ 

using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

using Planner.Objects;

namespace Planner
{
    internal class ShoppingListDisplay
    {
        private string _shoppingListsDirectory;

        public ShoppingList ShoppingListObject;

        public ShoppingListDisplay(DateTime listDate)
        {
            GetPathForShoppingListFile();
            RetrieveShoppingList(listDate);
        }

        public ShoppingListDisplay(DateTime listDate, string shopList)
        {
            GetPathForShoppingListFile();
            NewShoppingList(listDate, shopList);
        }

        private void GetPathForShoppingListFile()
        {
            string path = Path.GetFullPath("ShoppingList.txt");
#if DEBUG
            path = path.Remove(path.Length - 34);
#else
            path = path.Remove(path.Length - 36);
#endif
            path = path + @"ShoppingLists\";
            _shoppingListsDirectory = path;
        }

        private void NewShoppingList(DateTime listDate, string shopList)
        {
            ShoppingListObject = new ShoppingList();
            ShoppingListObject.WeekEnd = listDate.Date;
            ShoppingListObject.shoppingList = shopList;
            SaveShoppingList();
        }

        private void RetrieveShoppingList(DateTime listDate)
        {
            string fileName = listDate.Year.ToString() + listDate.Month.ToString() + listDate.Day.ToString();
            var path = _shoppingListsDirectory + fileName + ".xml";
            XmlSerializer deSerialize = new XmlSerializer(typeof(ShoppingList));
            if (File.Exists(path))
            {
                FileStream file = new FileStream(path, FileMode.Open);
                ShoppingListObject = (ShoppingList)deSerialize.Deserialize(file);
                file.Close();
            }
        }

        private void SaveShoppingList()
        {
            string fileName = ShoppingListObject.WeekEnd.Year + ShoppingListObject.WeekEnd.Month.ToString() + ShoppingListObject.WeekEnd.Day.ToString();
            var path = _shoppingListsDirectory + fileName + ".xml";

            XmlWriterSettings writersettings = new XmlWriterSettings();
            writersettings.NewLineHandling = NewLineHandling.Entitize;

            XmlSerializer serialize = new XmlSerializer(ShoppingListObject.GetType());

            using (XmlWriter writer = XmlWriter.Create(path, writersettings))
            {
                serialize.Serialize(writer, ShoppingListObject);
            }

        }
    }
}
