using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.ExceptionServices;

namespace ElvisPhotoUploader.test;

internal sealed class PhotoBatch
{
    private const int width = 800;
    private const int height = 600;
    private const int maxJpegBytes = 200_000;

    private static readonly HashSet<string> Extensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".jfif", ".png", ".bmp", ".tif", ".tiff"
    };

    public static string StagingRoot { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ElvisPhotoUploader", "Staged");
}