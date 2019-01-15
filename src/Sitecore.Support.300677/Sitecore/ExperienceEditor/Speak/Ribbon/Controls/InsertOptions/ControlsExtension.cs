namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Controls.InsertOptions
{
  using Sitecore.Diagnostics;
  using Sitecore.Mvc;
  using Sitecore.Mvc.Presentation;
  using System;
  using System.Runtime.CompilerServices;
  using System.Web;

  public static class ControlsExtension
  {
    public static HtmlString InsertOptions(this Controls controls, Rendering rendering)
    {
      Assert.ArgumentNotNull(controls, "controls");
      Assert.ArgumentNotNull(rendering, "rendering");
      return new HtmlString(new Sitecore.Support.ExperienceEditor.Speak.Ribbon.Controls.InsertOptions.InsertOptions(controls.GetParametersResolver(rendering)).Render());
    }
  }
}
