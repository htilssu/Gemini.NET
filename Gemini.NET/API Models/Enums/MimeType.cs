using System.ComponentModel;

namespace GeminiDotNET.ApiModels.Enums
{
    /// <summary>
    /// The type of the file specified in the Inline Data
    /// </summary>
    public enum MimeType
    {
        #region Document
        [Description("application/pdf")]
        PDF,

        [Description("text/plain")]
        TXT,

        [Description("text/html")]
        HTML,

        [Description("text/css")]
        CSS,

        [Description("text/md")]
        MD,

        [Description("text/csv")]
        CSV,

        [Description("text/xml")]
        XML,

        [Description("text/rtf")]
        RTF,
        #endregion

        #region Image
        [Description("image/png")]
        PNG,

        [Description("image/jpeg")]
        JPEG,

        [Description("image/heif")]
        HEIF,

        [Description("image/heic")]
        HEIC,

        [Description("image/webp")]
        WEBP,
        #endregion

        #region Video
        [Description("video/mp4")]
        MP4,

        [Description("video/mpeg")]
        MPEG,

        [Description("video/mov")]
        MOV,

        [Description("video/avi")]
        AVI,

        [Description("video/x-flv")]
        FLV,

        [Description("video/mpg")]
        MPG,

        [Description("video/webm")]
        WEBM,

        [Description("video/wmv")]
        WMV,

        [Description("video/3gpp")]
        GPP,
        #endregion

        #region Audio
        [Description("audio/wav")]
        WAV,

        [Description("audio/mp3")]
        MP3,

        [Description("audio/aiff")]
        AIFF,

        [Description("audio/aac")]
        AAC,

        [Description("audio/ogg")]
        OGG,

        [Description("audio/flac")]
        FLAC,
        #endregion
    }
}
