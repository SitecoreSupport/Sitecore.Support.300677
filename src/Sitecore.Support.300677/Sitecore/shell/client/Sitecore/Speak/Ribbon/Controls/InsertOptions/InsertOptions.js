﻿define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Factories.createBaseComponent({
        name: "InsertOptions",
        base: "ControlBase",
        selector: ".sc-insert-items",
        attributes: [
            { name: "selectedItemId", value: "" },
            { name: "selectedDisplayName", value: "" }
        ],
        initialize: function () {
        },
        refreshInsertOptions: function (itemId) {
            var context = ExperienceEditor.RibbonApp.getAppContext();
            context.currentContext.itemId = itemId;
            ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.Insert.GetInsertOptions", function (response) {
                if (response.responseValue.value.length > 0) {
                    var insertOptionsContainer = $(this.document).find("[data-sc-id='InsertOptions']");
                    insertOptionsContainer.empty();
                    jQuery.each(response.responseValue.value, function () {
                        insertOptionsContainer.append(this.toString());
                        $(insertOptionsContainer).trigger("insertOptionAdded", insertOptionsContainer[0].lastChild);
                    });
                }
                else {
                    var dialogWindow = document.getElementsByClassName("sc-dialogWindow-buttons")[0];
                    dialogWindow.children[0].setAttribute('disabled', "disabled");
                }
            }).execute(context);
        },
        disableInsertAbility: function () {
            $(this.el).empty();
        }
    });
});