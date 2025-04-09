using System.ComponentModel;

namespace GeminiDotNET.API_Models.Enums
{
    /// <summary>
    /// The media type of the file specified in the Inline Data
    /// </summary>
    public enum MimeType
    {
        /// <summary>
        /// MIME type for PDF documents.
        /// </summary>
        [Description("application/pdf")]
        ApplicationPdf,

        /// <summary>
        /// MIME type for MPEG audio.
        /// </summary>
        [Description("audio/mpeg")]
        AudioMpeg,

        /// <summary>
        /// MIME type for MP3 audio.
        /// </summary>
        [Description("audio/mp3")]
        AudioMp3,

        /// <summary>
        /// MIME type for WAV audio.
        /// </summary>
        [Description("audio/wav")]
        AudioWav,

        /// <summary>
        /// MIME type for PNG images.
        /// </summary>
        [Description("image/png")]
        ImagePng,

        /// <summary>
        /// MIME type for JPEG images.
        /// </summary>
        [Description("image/jpeg")]
        ImageJpeg,

        /// <summary>
        /// MIME type for HEIF images.
        /// </summary>
        [Description("image/heif")]
        ImageHeif,

        /// <summary>
        /// MIME type for HEIC images.
        /// </summary>
        [Description("image/heic")]
        ImageHeic,

        /// <summary>
        /// MIME type for WEBP images.
        /// </summary>
        [Description("image/webp")]
        ImageWebp,

        /// <summary>
        /// MIME type for plain text.
        /// </summary>
        [Description("text/plain")]
        TextPlain,

        /// <summary>
        /// MIME type for MOV video.
        /// </summary>
        [Description("video/mov")]
        VideoMov,

        /// <summary>
        /// MIME type for MPEG video.
        /// </summary>
        [Description("video/mpeg")]
        VideoMpeg,

        /// <summary>
        /// MIME type for MP4 video.
        /// </summary>
        [Description("video/mp4")]
        VideoMp4,

        /// <summary>
        /// MIME type for MPG video.
        /// </summary>
        [Description("video/mpg")]
        VideoMpg,

        /// <summary>
        /// MIME type for AVI video.
        /// </summary>
        [Description("video/avi")]
        VideoAvi,

        /// <summary>
        /// MIME type for WMV video.
        /// </summary>
        [Description("video/wmv")]
        VideoWmv,

        /// <summary>
        /// MIME type for MPEGPS video.
        /// </summary>
        [Description("video/mpegps")]
        VideoMpegps,

        /// <summary>
        /// MIME type for FLV video.
        /// </summary>
        [Description("video/flv")]
        VideoFlv
    }
}
