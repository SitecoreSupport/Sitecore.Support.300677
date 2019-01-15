#region Dependencies

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.Support.ExperienceEditor.Speak.Ribbon.Controls.SelectOptionItem;

#endregion

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Controls.InsertOptions
{
  using Sitecore.ExperienceEditor.Speak.Caches;

  public class InsertOptions : Sitecore.ExperienceEditor.Speak.Ribbon.RibbonComponentControlBase
  {
    protected const string DefaultDataBind = "visible: isVisible";

    private Item _dataSourceItem;

    public InsertOptions()
    {
      InitializeControl(null);
    }

    public InsertOptions(RenderingParametersResolver parametersResolver)
      : base(parametersResolver)
    {
      InitializeControl(parametersResolver);
    }

    protected override Item DataSourceItem
    {
      get { return _dataSourceItem ?? (_dataSourceItem = Context.ContentDatabase.GetItem(DataSource)); }
    }


    public string Headline { get; set; }
    public string Text { get; set; }
    public string IconPath { get; set; }

    protected IEnumerable<SelectOptionsItem> InsertItems { get; set; }

    protected void InitializeControl(RenderingParametersResolver parametersResolver)
    {
      if (parametersResolver != null)
        DataSource = parametersResolver.GetString("DataSource");
      if (string.IsNullOrEmpty(DataSource))
        DataSource = GetDataSourceFromQuery();
      Assert.IsNotNullOrEmpty(DataSource, "DataSource cannot be null or empty on insert options control {0}", ControlId);
      if (string.IsNullOrEmpty(DataSource))
        throw new ArgumentException(string.Concat("Datasource on ", ControlId, " is missing"));
      Class = "sc-insert-items";
      ResourcesCache.RequireJs(this, "ribbon", "InsertOptions.js");
      ResourcesCache.RequireCss(this, "ribbon", "InsertOptions.css");
      DataBind = DefaultDataBind;
      var items = GetInsertOptions(DataSourceItem);
      if (items.Count() == 0)
      {
        InsertItems = new List<SelectOptionsItem> { new SelectOptionsItem(DataSourceItem.Template, "trigger:item:selected", false) };
      }
      else
      {
        InsertItems = items.Select(i => new SelectOptionsItem(i, "trigger:item:selected", true));
      }
      HasNestedComponents = true;
    }

    protected static IEnumerable<Item> GetInsertOptions(Item dataSourceItem)
    {
      return Masters.GetMasters(dataSourceItem);
    }

    protected virtual string GetDataSourceFromQuery()
    {
      var itemId = HttpContext.Current.Request["itemId"];
      if (string.IsNullOrEmpty(itemId))
      {
        itemId = HttpContext.Current.Request["id"];
      }
      return itemId;
    }

    protected override void Render([NotNull] HtmlTextWriter output)
    {
      Assert.ArgumentNotNull(output, nameof(output));

      base.Render(output);

      AddAttributes(output);
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      foreach (var insertItem in InsertItems)
      {
        output.Write(insertItem.Render());
      }
      output.RenderEndTag();
    }
  }
}