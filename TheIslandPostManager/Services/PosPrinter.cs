using BarcodeStandard;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Media;

namespace TheIslandPostManager.Services;
public class POSPrinter
{

    Font myFont = new("Arial", 15, FontStyle.Bold);
    SolidBrush myBrush = new(System.Drawing.Color.Black);
    PointF myPoint = new(0, 0);

    public float CurrentY
    {
        get { return myPoint.Y; }
    }

    internal void SetCurrentX(float amount)
    {
        myPoint.X = amount;
    }


    Single lineHeight { get; set; }


    PrintPageEventArgs ppea;

    public POSPrinter(PrintPageEventArgs setE)
    {
        lineHeight = myFont.Height;
        ppea = setE;
    }

    public void PrintHLine()
    {
        System.Drawing.Pen pen = new(System.Drawing.Color.Black);
        PointF startPoint = new(0, myPoint.Y);
        PointF endPoint = new(ppea.PageBounds.Width, myPoint.Y);
        ppea.Graphics!.DrawLine(pen, startPoint, endPoint);
        myPoint.X = 0;
        myPoint.Y += (float)(lineHeight * 0.8);
    }

    public void EmptyLine()
    {
        myPoint.Y += (float)(lineHeight / 1.1);
        myPoint.X = 0;
    }

    public void NewLine()
    {
        myPoint.Y += (float)(lineHeight);
        myPoint.X = 0;
    }


    public void PrintText(string text, TextAlign align = TextAlign.LEFT)
    {

        PointF textPos;
        switch (align)
        {
            case TextAlign.LEFT:
                textPos = myPoint;
                break;
            case TextAlign.RIGHT:
                textPos = RightPosition(text);
                break;
            case TextAlign.CENTER:
                textPos = CenterPosition(text);
                break;
            default:
                return;
        }

        ppea.Graphics!.DrawString(text, myFont, myBrush, textPos);

        SizeF textSize = ppea.Graphics!.MeasureString(text, myFont);
        myPoint.X += textSize.Width;

    }

    public void PrintTextLn(string text, TextAlign align = TextAlign.LEFT)
    {
        PrintText(text, align);
        myPoint.X = 0;
        myPoint.Y += (float)(lineHeight / 1.1);
    }


    public void PrintBarcode(string value, TextAlign align = TextAlign.LEFT)
    {
        SKFont labelFont = new()
        {
            Size = 10
        };
        Barcode b = new()
        {
            IncludeLabel = true,
            LabelFont = labelFont
        };

        // Encode barcode as SkiaSharp image
        SKImage img = b.Encode(BarcodeStandard.Type.Code128, value,
                               SKColors.Black, SKColors.White, 150, 50);

        SKData data = img.Encode(SKEncodedImageFormat.Png, 50);
        using Stream barcodeImageStream = data.AsStream();
        using Image image = Image.FromStream(barcodeImageStream);

        float x = myPoint.X;

        switch (align)
        {
            case TextAlign.CENTER:
                float printableWidth = ppea.PageSettings.PaperSize.Width
                                       - ppea.PageSettings.Margins.Left
                                       - ppea.PageSettings.Margins.Right;
                x = ppea.PageSettings.Margins.Left + (printableWidth - image.Width) / 2;
                break;

            case TextAlign.RIGHT:
                printableWidth = ppea.PageSettings.PaperSize.Width
                                 - ppea.PageSettings.Margins.Left
                                 - ppea.PageSettings.Margins.Right;
                x = ppea.PageSettings.Margins.Left + (printableWidth - image.Width);
                break;

            case TextAlign.LEFT:
            default:
                x = ppea.PageSettings.Margins.Left;
                break;
        }

        // Draw barcode at calculated position
        ppea.Graphics!.DrawImage(image, x, myPoint.Y, image.Width, image.Height);

        // Advance cursor down after barcode
        myPoint.X = 0;
        myPoint.Y += image.Height + 25; // add padding
    }

    //private PointF CenterPosition(string text)
    //{
    //    SizeF textSize = ppea.Graphics!.MeasureString(text, myFont);
    //    Single freeSpace = ppea.PageSettings.PaperSize.Width - textSize.Width;
    //    Single centerX = (freeSpace / 2) - (ppea.PageSettings.Margins.Left / 6);
    //    return new PointF(centerX, myPoint.Y);
    //}

    //Refactorerd by AI
    private PointF CenterPosition(string text)
    {
        SizeF textSize = ppea.Graphics!.MeasureString(text, myFont);

        // Total printable width = page width minus left/right margins
        float printableWidth = ppea.PageSettings.PaperSize.Width
                               - ppea.PageSettings.Margins.Left
                               - ppea.PageSettings.Margins.Right;

        // Center = left margin + half of free space
        float centerX = ppea.PageSettings.Margins.Left + (printableWidth - textSize.Width) / 2;

        return new PointF(centerX, myPoint.Y);
    }


    //private PointF RightPosition(string text)
    //{
    //    SizeF textSize = ppea.Graphics!.MeasureString(text, myFont);
    //    Single freeSpace = ppea.PageSettings.PaperSize.Width - textSize.Width;
    //    Single positionX = (freeSpace / 2) - (ppea.PageSettings.Margins.Left / 2);
    //    return new PointF(positionX, myPoint.Y);
    //}

    //Refactored By AI
    private PointF RightPosition(string text)
    {
        SizeF textSize = ppea.Graphics!.MeasureString(text, myFont);

        // Total printable width = page width minus left/right margins
        float printableWidth = ppea.PageSettings.PaperSize.Width
                               - ppea.PageSettings.Margins.Left
                               - ppea.PageSettings.Margins.Right;

        // Position X = right edge minus text width
        float positionX = ppea.PageSettings.Margins.Left + (printableWidth - textSize.Width);

        return new PointF(positionX, myPoint.Y);
    }

    public void PrintTwoColumns(string leftText, string rightText, float leftWidthRatio = 0.6f)
    {
        // Calculate printable width
        float printableWidth = ppea.PageSettings.PaperSize.Width
                               - ppea.PageSettings.Margins.Left
                               - ppea.PageSettings.Margins.Right;

        // Left column width (e.g. 60% of total)
        float leftColumnWidth = printableWidth * leftWidthRatio;

        // Draw left text
        PointF leftPos = new(ppea.PageSettings.Margins.Left, myPoint.Y);
        ppea.Graphics!.DrawString(leftText, myFont, myBrush, leftPos);

        // Draw right text aligned to right column
        SizeF rightSize = ppea.Graphics!.MeasureString(rightText, myFont);
        float rightX = ppea.PageSettings.Margins.Left + printableWidth - rightSize.Width;
        PointF rightPos = new(rightX, myPoint.Y);
        ppea.Graphics!.DrawString(rightText, myFont, myBrush, rightPos);

        // Move down one line
        myPoint.X = 0;
        myPoint.Y += (float)(lineHeight / 1.1);
    }

    public void PrintItemWithDotsTruncate(string leftText, string rightText)
    {
        SizeF rightSize = ppea.Graphics!.MeasureString(rightText, myFont);

        float printableWidth = ppea.PageSettings.PaperSize.Width
                               - ppea.PageSettings.Margins.Left
                               - ppea.PageSettings.Margins.Right;

        float leftX = ppea.PageSettings.Margins.Left;
        float rightX = ppea.PageSettings.Margins.Left + printableWidth - rightSize.Width;

        // Max width available for left text (before dots and price)
        float maxLeftWidth = rightX - leftX - 20; // leave padding for dots

        // Truncate left text if too long
        string displayLeft = leftText;
        SizeF leftSize = ppea.Graphics!.MeasureString(displayLeft, myFont);
        while (leftSize.Width > maxLeftWidth && displayLeft.Length > 3)
        {
            displayLeft = displayLeft.Substring(0, displayLeft.Length - 4) + "...";
            leftSize = ppea.Graphics!.MeasureString(displayLeft, myFont);
        }

        // Draw left text
        ppea.Graphics!.DrawString(displayLeft, myFont, myBrush, new PointF(leftX, myPoint.Y));

        // Draw right text
        ppea.Graphics!.DrawString(rightText, myFont, myBrush, new PointF(rightX, myPoint.Y));

        // Fill gap with dots
        float dotStart = leftX + leftSize.Width;
        int dotCount = (int)((rightX - dotStart) / (myFont.Size / 2));
        if (dotCount < 0) dotCount = 0;

        string dots = new string('.', dotCount);
        ppea.Graphics!.DrawString(dots, myFont, myBrush, new PointF(dotStart, myPoint.Y));

        // Move down one line
        myPoint.X = 0;
        myPoint.Y += (float)(lineHeight / 1.1);
    }
    void PrintItemWithDots(string leftText, string rightText)
    {
        SizeF leftSize = ppea.Graphics!.MeasureString(leftText, myFont);
        SizeF rightSize = ppea.Graphics!.MeasureString(rightText, myFont);

        float printableWidth = ppea.PageSettings.PaperSize.Width
                               - ppea.PageSettings.Margins.Left
                               - ppea.PageSettings.Margins.Right;

        float leftX = ppea.PageSettings.Margins.Left;
        float rightX = ppea.PageSettings.Margins.Left + printableWidth - rightSize.Width;

        // If left text overlaps right text, wrap it
        if (leftX + leftSize.Width > rightX)
        {
            // Print left text on its own line
            ppea.Graphics!.DrawString(leftText, myFont, myBrush, new PointF(leftX, myPoint.Y));
            myPoint.Y += (float)(lineHeight / 1.1);

            // Then print right text on the next line
            ppea.Graphics!.DrawString(rightText, myFont, myBrush, new PointF(rightX, myPoint.Y));
        }
        else
        {
            // Normal case: print left, dots, right
            ppea.Graphics!.DrawString(leftText, myFont, myBrush, new PointF(leftX, myPoint.Y));
            ppea.Graphics!.DrawString(rightText, myFont, myBrush, new PointF(rightX, myPoint.Y));

            int dotCount = (int)((rightX - (leftX + leftSize.Width)) / (myFont.Size / 2));
            if (dotCount < 0) dotCount = 0;

            string dots = new string('.', dotCount);
            ppea.Graphics!.DrawString(dots, myFont, myBrush, new PointF(leftX + leftSize.Width, myPoint.Y));
        }

        myPoint.X = 0;
        myPoint.Y += (float)(lineHeight / 1.1);
    }

    public void SetFontSize(float size)
    {
        // Recreate font with same family/style but new size
        myFont = new Font(myFont.FontFamily, size, myFont.Style);
        lineHeight = myFont.Height;
    }

    public void PrintImage(string logoPath, bool center = false, float width = 100, float height = 100)
    {
        if (!File.Exists(logoPath))
            throw new FileNotFoundException("Logo file not found", logoPath);

        using Image logo = Image.FromFile(logoPath);

        float x = myPoint.X;

        if (center)
        {
            // Total printable width = page width minus margins
            float printableWidth = ppea.PageSettings.PaperSize.Width
                                   - ppea.PageSettings.Margins.Left
                                   - ppea.PageSettings.Margins.Right;

            // Center = left margin + half of free space
            x = ppea.PageSettings.Margins.Left + (printableWidth - width) / 2;
        }

        // Draw logo at calculated position
        RectangleF destRect = new RectangleF(x, myPoint.Y, width, height);
        ppea.Graphics!.DrawImage(logo, destRect);

        // Move cursor down after logo
        myPoint.X = 0;
        myPoint.Y += height + 10; // add padding
    }


    public Font Font
    {
        get { return myFont; }
        set { myFont = value; lineHeight = myFont.Height; }
    }

    public SolidBrush Brush
    {
        get { return myBrush; }
        set { myBrush = value; }
    }

}

public enum TextAlign
{
    RIGHT = 0,
    LEFT = 1,
    CENTER = 2
}
