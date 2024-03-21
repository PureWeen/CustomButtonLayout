using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Security.Credentials.UI;
using WThickness = Microsoft.UI.Xaml.Thickness;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;
using WImage = Microsoft.UI.Xaml.Controls.Image;
using WButton = Microsoft.UI.Xaml.Controls.Button;
using WRowDefinition = Microsoft.UI.Xaml.Controls.RowDefinition;
using WColumnDefinition = Microsoft.UI.Xaml.Controls.ColumnDefinition;
using WVerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;
using WHorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using Microsoft.Maui.Platform;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace CustomButtonLayout
{
    public class MauiButtonAutomationPeer : ButtonAutomationPeer
    {
        public MauiButtonAutomationPeer(WButton owner) : base(owner)
        {
        }

        protected override IList<AutomationPeer>? GetChildrenCore()
        {
            return null;
        }

        protected override AutomationPeer? GetLabeledByCore()
        {
            foreach (var item in base.GetChildrenCore())
            {
                if (item is TextBlockAutomationPeer tba)
                    return tba;
            }

            return null;
        }
    }

    public class CustomButton : Microsoft.Maui.Platform.MauiButton
    {
        public static void MapContentLayout(IButtonHandler handler, Microsoft.Maui.Controls.Button button)
        {
           
        }

        public CustomButton()
        {
            Content = new DefaultMauiButtonContent();

            VerticalAlignment = WVerticalAlignment.Stretch;
            HorizontalAlignment = WHorizontalAlignment.Stretch;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new MauiButtonAutomationPeer(this);
        }
    }

    internal class DefaultMauiButtonContent : WGrid
    {
        readonly WImage _image;
        readonly TextBlock _textBlock;

        public DefaultMauiButtonContent()
        {
            RowDefinitions.Add(new WRowDefinition { Height = Microsoft.UI.Xaml.GridLength.Auto });
            RowDefinitions.Add(new WRowDefinition { Height = Microsoft.UI.Xaml.GridLength.Auto });

            ColumnDefinitions.Add(new WColumnDefinition { Width = Microsoft.UI.Xaml.GridLength.Auto });
            ColumnDefinitions.Add(new WColumnDefinition { Width = Microsoft.UI.Xaml.GridLength.Auto });

            HorizontalAlignment = WHorizontalAlignment.Center;
            VerticalAlignment = WVerticalAlignment.Center;
            Margin = new WThickness(0);

            _image = new WImage
            {
                VerticalAlignment = WVerticalAlignment.Center,
                HorizontalAlignment = WHorizontalAlignment.Center,
                Stretch = Microsoft.UI.Xaml.Media.Stretch.None,
                Margin = new WThickness(0),
                Visibility = Microsoft.UI.Xaml.Visibility.Collapsed,
            };

            _image.ImageOpened += (x, y) =>
            {
                _image.Stretch = Microsoft.UI.Xaml.Media.Stretch.None;
            };

            _image.LayoutUpdated += (x, y) =>
            {
                _image.Stretch = Microsoft.UI.Xaml.Media.Stretch.None;
            };

            _textBlock = new TextBlock
            {
                VerticalAlignment = WVerticalAlignment.Center,
                HorizontalAlignment = WHorizontalAlignment.Center,
                Margin = new WThickness(0),
                Visibility = Microsoft.UI.Xaml.Visibility.Collapsed,
            };

            Children.Add(_image);
            Children.Add(_textBlock);

            LayoutImageLeft(0);
        }

        public void LayoutImageLeft(double spacing)
        {
            SetupHorizontalLayout(spacing);

            WGrid.SetColumn(_image, 0);
            WGrid.SetColumn(_textBlock, 1);

            ColumnDefinitions[0].Width = Microsoft.UI.Xaml.GridLength.Auto;
            ColumnDefinitions[1].Width = new Microsoft.UI.Xaml.GridLength(1, Microsoft.UI.Xaml.GridUnitType.Star);
        }

        public void LayoutImageRight(double spacing)
        {
            SetupHorizontalLayout(spacing);

            WGrid.SetColumn(_image, 1);
            WGrid.SetColumn(_textBlock, 0);

            ColumnDefinitions[0].Width = new Microsoft.UI.Xaml.GridLength(1, Microsoft.UI.Xaml.GridUnitType.Star);
            ColumnDefinitions[1].Width = Microsoft.UI.Xaml.GridLength.Auto;
        }

        public void LayoutImageTop(double spacing)
        {
            SetupVerticalLayout(spacing);

            WGrid.SetRow(_image, 0);
            WGrid.SetRow(_textBlock, 1);
        }

        public void LayoutImageBottom(double spacing)
        {
            SetupVerticalLayout(spacing);

            WGrid.SetRow(_image, 1);
            WGrid.SetRow(_textBlock, 0);
        }

        double AdjustSpacing(double spacing)
        {
            if (_image.Visibility == Microsoft.UI.Xaml.Visibility.Collapsed
                || _textBlock.Visibility == Microsoft.UI.Xaml.Visibility.Collapsed)
            {
                return 0;
            }

            return spacing;
        }

        void SetupHorizontalLayout(double spacing)
        {
            RowSpacing = 0;
            ColumnSpacing = AdjustSpacing(spacing);

            RowDefinitions[0].Height = new Microsoft.UI.Xaml.GridLength(1, Microsoft.UI.Xaml.GridUnitType.Star);
            RowDefinitions[1].Height = new Microsoft.UI.Xaml.GridLength(1, Microsoft.UI.Xaml.GridUnitType.Star);

            WGrid.SetRow(_image, 0);
            WGrid.SetRowSpan(_image, 2);
            WGrid.SetColumnSpan(_image, 1);

            WGrid.SetRow(_textBlock, 0);
            WGrid.SetRowSpan(_textBlock, 2);
            WGrid.SetColumnSpan(_textBlock, 1);

        }

        void SetupVerticalLayout(double spacing)
        {
            ColumnSpacing = 0;
            RowSpacing = AdjustSpacing(spacing);

            RowDefinitions[0].Height = Microsoft.UI.Xaml.GridLength.Auto;
            RowDefinitions[1].Height = Microsoft.UI.Xaml.GridLength.Auto;

            ColumnDefinitions[0].Width = new Microsoft.UI.Xaml.GridLength(1, Microsoft.UI.Xaml.GridUnitType.Star);
            ColumnDefinitions[1].Width = new Microsoft.UI.Xaml.GridLength(1, Microsoft.UI.Xaml.GridUnitType.Star);

            WGrid.SetRowSpan(_image, 1);
            WGrid.SetColumn(_image, 0);
            WGrid.SetColumnSpan(_image, 2);

            WGrid.SetRowSpan(_textBlock, 1);
            WGrid.SetColumn(_textBlock, 0);
            WGrid.SetColumnSpan(_textBlock, 2);
        }

        public static void UpdateContentLayout(Microsoft.UI.Xaml.Controls.Button mauiButton, Microsoft.Maui.Controls.Button button)
        {
            if (mauiButton.Content is not DefaultMauiButtonContent content)
            {
                // If the content is the default for Maui.Core, then
                // The user has set a custom Content or the content isn't a mix of text/images
                return;
            }

            var layout = button.ContentLayout;
            var spacing = layout.Spacing;

            switch (layout.Position)
            {
                case Microsoft.Maui.Controls.Button.ButtonContentLayout.ImagePosition.Top:
                    content.LayoutImageTop(spacing);
                    break;
                case Microsoft.Maui.Controls.Button.ButtonContentLayout.ImagePosition.Bottom:
                    content.LayoutImageBottom(spacing);
                    break;
                case Microsoft.Maui.Controls.Button.ButtonContentLayout.ImagePosition.Right:
                    content.LayoutImageRight(spacing);
                    break;
                default:
                    // Defaults to image on the left
                    content.LayoutImageLeft(spacing);
                    break;
            }
        }

        public static void UpdateLineBreakMode(WButton platformButton, Microsoft.Maui.Controls.Button button)
        {
            if (platformButton.GetContent<TextBlock>() is TextBlock textBlock)
            {
                SetLineBreakMode(textBlock, button.LineBreakMode, null);
            }
        }

        static void SetLineBreakMode(TextBlock textBlock, LineBreakMode lineBreakMode, int? maxLines = null)
        {
            if (maxLines.HasValue && maxLines >= 0)
                textBlock.MaxLines = maxLines.Value;
            else
                textBlock.MaxLines = 0;

            switch (lineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    textBlock.TextTrimming = TextTrimming.Clip;
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.WordWrap:
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.CharacterWrap:
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    // TODO: This truncates at the end.
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    DetermineTruncatedTextWrapping(textBlock);
                    break;
                case LineBreakMode.TailTruncation:
                    textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                    DetermineTruncatedTextWrapping(textBlock);
                    break;
                case LineBreakMode.MiddleTruncation:
                    // TODO: This truncates at the end.
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    DetermineTruncatedTextWrapping(textBlock);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static void DetermineTruncatedTextWrapping(TextBlock textBlock) =>
            textBlock.TextWrapping = textBlock.MaxLines > 1 ? TextWrapping.Wrap : TextWrapping.NoWrap;
    }
}