using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hellion.Core.Structures.Dialogs
{
    [DataContract]
    public class DialogData
    {
        /// <summary>
        /// Gets or sets the dialog name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dialog oral text.
        /// </summary>
        [DataMember(Name = "oral")]
        public string OralText { get; set; }

        /// <summary>
        /// Gets or sets the dialog introduction text.
        /// </summary>
        /// <remarks>
        /// This text will appear once the dialog box has been openened.
        /// </remarks>
        [DataMember(Name = "textIntro")]
        public string TextIntro { get; set; }

        /// <summary>
        /// Gets or sets the "goodbye" text.
        /// </summary>
        /// <remarks>
        /// This text will appear once the client closes the dialog box.
        /// The text will be displayed as an oral text.
        /// </remarks>
        [DataMember(Name = "bye")]
        public string Bye { get; set; }

        /// <summary>
        /// Gets or sets the dialog links.
        /// </summary>
        [DataMember(Name = "links")]
        public List<DialogLink> Links { get; set; }

        /// <summary>
        /// Creates an empty <see cref="DialogData"/> instance.
        /// </summary>
        public DialogData()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Creates a <see cref="DialogData"/> instance.
        /// </summary>
        /// <param name="name">Dialog name</param>
        /// <param name="oralText">Dialog oral text</param>
        /// <param name="textIntro">Dialog introduction text</param>
        /// <param name="byeText">Dialog goodbye text</param>
        public DialogData(string name, string oralText, string textIntro, string byeText)
        {
            this.Name = name;
            this.OralText = oralText;
            this.TextIntro = textIntro;
            this.Bye = byeText;
        }
    }
}
