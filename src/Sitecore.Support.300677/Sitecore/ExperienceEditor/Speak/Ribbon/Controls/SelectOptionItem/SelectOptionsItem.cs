namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Controls.SelectOptionItem
{
  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.ExperienceEditor.Speak.Caches;
  using Sitecore.ExperienceEditor.Speak.Ribbon;
  using Sitecore.Resources;
  using Sitecore.Web.UI;
  using System;
  using System.Runtime.CompilerServices;
  using System.Web.UI;

  public class SelectOptionsItem : RibbonComponentControlBase
  {
    protected const string DefaultDataBind = "visible: isVisible, click: click, enabled: isEnabled";

    protected SelectOptionsItem()
    {
    }

    public SelectOptionsItem(Item dataSourceItem, string click, bool renderAsThumbnail)
    {
      Assert.IsNotNull(dataSourceItem, "SelectOptionsItem - dataSourceItem");
      this.SourceItem = dataSourceItem;
      this.Click = click;
      this.RenderAsThumbnail = renderAsThumbnail;
      this.InitializeControl();
    }

    protected string GetIconPath(Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      if (!this.RenderAsThumbnail)
      {
        return Images.GetThemedImageSource(item.Appearance.Icon, ImageDimension.id48x48);
      }
      if (item.Appearance.Thumbnail == Settings.DefaultThumbnail)
      {
        return Images.GetThemedImageSource(item.Appearance.Icon, ImageDimension.id48x48);
      }
      return UIUtil.GetThumbnailSrc(item, 0x80, 0x80);
    }

    protected void InitializeControl()
    {
      object[] args = new object[] { base.ControlId };
      Assert.IsNotNull(this.SourceItem, "SourceItem on {0} is missing", args);
      base.Class = "sc-insertItem-container";
      base.DataBind = "visible: isVisible, click: click, enabled: isEnabled";
      ResourcesCache.RequireJs(this, "ribbon", "SelectOptionsItem.js");
      ResourcesCache.RequireCss(this, "ribbon", "SelectOptionsItem.css");
    }

    protected override void PreRender()
    {
      base.PreRender();
      base.Attributes["data-sc-click"] = this.Click;
      base.Attributes["data-sc-displayname"] = this.SourceItem.DisplayName;
      base.Attributes["data-sc-itemid"] = this.SourceItem.ID.ToShortID().ToString();
    }

    protected override void Render(HtmlTextWriter output)
    {
      if (this.RenderAsThumbnail)
      {
        DoRender(output);
      }
      else
      {
        RenderDummy(output);
      }
    }

    private void DoRender(HtmlTextWriter output)
    {
      Assert.ArgumentNotNull(output, "output");
      base.Render(output);
      this.AddAttributes(output);
      output.AddAttribute(HtmlTextWriterAttribute.Class, "sc-insertItem-container");
      output.AddAttribute(HtmlTextWriterAttribute.Href, "#");
      output.RenderBeginTag(HtmlTextWriterTag.A);
      output.AddAttribute(HtmlTextWriterAttribute.Class, "sc-insertItem-imageContainer");
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      output.AddAttribute(HtmlTextWriterAttribute.Alt, this.SourceItem.DisplayName);
      output.AddAttribute(HtmlTextWriterAttribute.Src, this.GetIconPath(this.SourceItem));
      output.RenderBeginTag(HtmlTextWriterTag.Img);
      output.RenderEndTag();
      output.RenderEndTag();
      output.AddAttribute(HtmlTextWriterAttribute.Class, "sc-insertItem-displayNameContainer");
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      output.AddAttribute(HtmlTextWriterAttribute.Class, "sc-display-name");
      output.RenderBeginTag(HtmlTextWriterTag.Span);
      output.Write(this.SourceItem.DisplayName);
      output.RenderEndTag();
      output.RenderEndTag();
      output.RenderEndTag();
    }
    private void RenderDummy(HtmlTextWriter output)
    {
      Assert.ArgumentNotNull(output, "output");
      base.Render(output);
      this.AddAttributes(output);
      output.AddAttribute(HtmlTextWriterAttribute.Class, "sc-insertItem-container");
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      output.RenderEndTag();
    }
    protected string Click { get; set; }

    protected bool RenderAsThumbnail { get; set; }

    protected Item SourceItem { get; set; }
  }
}
