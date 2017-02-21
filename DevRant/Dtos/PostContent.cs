using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.Dtos
{
    /// <summary>
    /// Post or comment for uploading
    /// </summary>
    public class PostContent
    {
        private static readonly List<string> SupportedTypes = new List<string>
        {
            "png", "jpeg", "jpg", "gif"
        };

        /// <summary>
        /// Returns the supported types
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<string> GetSupportedTypes()
        {
            return SupportedTypes.AsReadOnly();
        }

        /// <summary>
        /// Image
        /// </summary>
        public byte[] Image { get; private set; }

        /// <summary>
        /// Local name of file
        /// </summary>
        public string ImageName { get; private set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Adds an image to the post
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        public void AddImage(byte[] image, string name)
        {
            if (!Supported(name))
                throw new Exception("File not a supported image");

            ImageName = name;
            Image = image;
        }

        private bool Supported(string name)
        {
            FileInfo info = new FileInfo(name);
            string ext = info.Extension.ToLower().Replace(".","");

            return SupportedTypes.Contains(ext);
        }

        /// <summary>
        /// Creates a simple post
        /// </summary>
        /// <param name="text"></param>
        public PostContent(string text)
        {
            this.Text = text;
        }

        internal string GenerateImageName()
        {
            FileInfo info = new FileInfo(ImageName);
            string ext = info.Extension;

            string filename = string.Concat("xax", DateTime.Now.Ticks, "." + ext);
            return filename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void SetTag(string tag)
        {
            Tag = tag;
        }
    }
}
