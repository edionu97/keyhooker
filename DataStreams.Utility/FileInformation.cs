namespace DataStreams.Utility
{
    public class FileInformation
    {
        public string Language { get; set; }

        public string FileAff { get; set; }

        public string FileDict { get; set; }

        public void Deconstruct(out string aff, out string dict)
        {
            aff = FileAff;
            dict = FileDict;
        }

        public void Deconstruct(out string aff, out string dict, out  string lang)
        {
            aff = FileAff;
            dict = FileDict;
            lang = Language;
        }
    }
}
