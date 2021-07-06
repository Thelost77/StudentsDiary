using System.IO;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class FileHelper<T> where T : new()
    {
        private string _filePath;

        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }
        public void SerializeToFile(T objects)
        {
            using (var streamWriter = new StreamWriter(_filePath))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(streamWriter, objects);
                streamWriter.Close();
            }
        }

        public T DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
                return new T();

            var serializer = new XmlSerializer(typeof(T));
            using (var streamReader = new StreamReader(_filePath))
            {
                var objects = (T)serializer.Deserialize(streamReader);
                streamReader.Close();
                return objects;
            }
        }
    }
}
