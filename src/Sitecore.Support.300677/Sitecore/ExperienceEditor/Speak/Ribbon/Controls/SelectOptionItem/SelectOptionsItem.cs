// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectOptionsItem.cs" company="Sitecore">
//   Copyright (c) Sitecore. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Controls.SelectOptionItem
{
  #region Dependencies

  using System.Web.UI;

  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.ExperienceEditor.Speak.Caches;
  using Sitecore.ExperienceEditor.Speak.Ribbon;
  using Sitecore.Resources;
  using Sitecore.Web.UI;

  #endregion

  /// <summary>
  /// Represents the 'SelectOptionsItem' control class.
  /// </summary>
  public class SelectOptionsItem : RibbonComponentControlBase
  {
    /// <summary>
    /// The default data bind.
    /// </summary>
    protected const string DefaultDataBind = "visible: isVisible, click: click, enabled: isEnabled";

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectOptionsItem"/> class.
    /// </summary>
    /// <param name="dataSourceItem">The data source item.</param>
    /// <param name="click">The click trigger.</param>
    /// <param name="renderAsThumbnail">The value indicates whether the item should be rendered as thumbnail.</param>
    public SelectOptionsItem(Item dataSourceItem, string click, bool renderAsThumbnail)
    {
      Assert.IsNotNull(dataSourceItem, "SelectOptionsItem - dataSourceItem");

      this.SourceItem = dataSourceItem;
      this.Click = click;
      this.RenderAsThumbnail = renderAsThumbnail;
      this.InitializeControl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectOptionsItem"/> class.
    /// Prevents a default instance of the <see cref="SelectOptionsItem"/> class from being created.
    /// </summary>
    protected SelectOptionsItem()
    {
    }

    /// <summary>
    /// Gets or sets the click trigger.
    /// </summary>
    protected string Click { get; set; }

    /// <summary>
    /// Gets or sets the data source item.
    /// </summary>
    protected Item SourceItem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item should be rendered as thumbnail.
    /// </summary>
    protected bool RenderAsThumbnail { get; set; }

    /// <summary>
    /// Initializes the control.
    /// </summary>
    protected void InitializeControl()
    {
      Assert.IsNotNull(this.SourceItem, "SourceItem on {0} is missing", this.ControlId);
      this.Class = "sc-insertItem-container";
      this.DataBind = DefaultDataBind;
      ResourcesCache.RequireJs(this, "ribbon", "SelectOptionsItem.js");
      ResourcesCache.RequireCss(this, "ribbon", "SelectOptionsItem.css");
    }

    /// <summary>
    /// The pre render.
    /// </summary>
    protected override void PreRender()
    {
      base.PreRender();
      this.Attributes["data-sc-click"] = this.Click;
      this.Attributes["data-sc-displayname"] = this.SourceItem.DisplayName;
      this.Attributes["data-sc-itemid"] = this.SourceItem.ID.ToShortID().ToString();
    }

    /// <summary>
    /// Renders the content.
    /// </summary>
    /// <param name="output">The HTML output.</param>
    protected override void Render([NotNull] HtmlTextWriter output)
    {
      Assert.ArgumentNotNull(output, nameof(output));

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

    /// <summary>
    /// Gets the item icon.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The item URL.</returns>
    protected string GetIconPath([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      if (!this.RenderAsThumbnail)
      {
        return Images.GetThemedImageSource(item.Appearance.Icon, ImageDimension.id48x48);
      }

      if (item.Appearance.Thumbnail == Configuration.Settings.DefaultThumbnail)
      {
        return Images.GetThemedImageSource(item.Appearance.Icon, ImageDimension.id48x48);
      }

      return UIUtil.GetThumbnailSrc(item, 128, 128);
    }
  }
}