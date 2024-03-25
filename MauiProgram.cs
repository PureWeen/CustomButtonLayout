using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
namespace CustomButtonLayout;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if MACCATALYST
                ButtonHandler.PlatformViewFactory = (x) =>
                {
                    var button = new UIKit.UIButton(UIKit.UIButtonType.System);

                    // Starting with iOS 15, we can use the button.Configuration and assign future UIButton.Configuration.ContentInsets here instead of the deprecated UIButton.ContentEdgeInsets.
                    // It is important to note that the configuration will change any set style changes so we will do this right after creating the button.
                    if (OperatingSystem.IsIOSVersionAtLeast(15))
                    {
                        var config = UIKit.UIButtonConfiguration.PlainButtonConfiguration;
                        button.Configuration = config;
                    }

		            UIKit.UIControlState[] ControlStates = { UIKit.UIControlState.Normal, UIKit.UIControlState.Highlighted, UIKit.UIControlState.Disabled };
                    foreach (UIKit.UIControlState uiControlState in ControlStates)
                    {
                        button.SetTitleColor(UIKit.UIButton.Appearance.TitleColor(uiControlState), uiControlState); // If new values are null, old values are preserved.
                        button.SetTitleShadowColor(UIKit.UIButton.Appearance.TitleShadowColor(uiControlState), uiControlState);
                        button.SetBackgroundImage(UIKit.UIButton.Appearance.BackgroundImageForState(uiControlState), uiControlState);
                    }

                    return button;

                };
				ButtonHandler.Mapper.ReplaceMapping<Button, IButtonHandler>(nameof(Button.ContentLayout), ButtonExtensions.MapContentLayout);
				ButtonHandler.Mapper.ReplaceMapping<Button, IButtonHandler>(nameof(Button.Text), ButtonExtensions.MapText);
				ButtonHandler.Mapper.ReplaceMapping<Button, IButtonHandler>(nameof(Button.TextTransform), ButtonExtensions.MapText);
				ButtonHandler.Mapper.ReplaceMapping<Button, IButtonHandler>(nameof(Button.Padding), ButtonExtensions.MapPadding);
#elif WINDOWS
                ButtonHandler.Mapper.AppendToMapping<Button, IButtonHandler>(nameof(Button.ContentLayout), CustomButton.MapContentLayout);

                ButtonHandler.PlatformViewFactory = (_) =>
                {
                    return new CustomButton();
                };
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
