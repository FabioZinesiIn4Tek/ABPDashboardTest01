class SaveAsDashboardExtension {
    constructor(dashboardControl) {
        this._dashboardControl = dashboardControl;

        this.name = "saveAs";
        this.newName = "New Dashboard Name";

        this._menuSaveAsItem = new DevExpress.Dashboard.Designer.DashboardMenuItem("save-as", "Save As...", 120, 0, this.showPopup.bind(this));
        this._menuSaveAsItem.hasSeparator = true;
        this._menuSaveAsItem.data = this;

        var saveIcon = '<svg version="1.1" id="dx-dashboard-toolbar-save" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 24 24" style="enable-background:new 0 0 24 24;" xml:space="preserve"><style type="text/css">.dx_darkgray{fill:#414141;}.dx_green{fill:#39A866;}</style><path class="dx_green" d="M21,2H3C2.4,2,2,2.4,2,3v18c0,0.6,0.4,1,1,1h3v-6h12v6h3c0.6,0,1-0.4,1-1V3C22,2.4,21.6,2,21,2z"/><rect x="14" y="18" class="dx_darkgray" width="2" height="4"/></svg>';
        var saveAsIcon = '<svg version="1.1" id="dx-dashboard-toolbar-save-as" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 24 24" style="enable-background:new 0 0 24 24;" xml:space="preserve"><style type="text/css">.dx_white{fill:#FFFFFF;}.dx_darkgray{fill:#414141;}.dx_green{fill:#39A866;}</style><path class="dx_green" d="M21,2H3C2.4,2,2,2.4,2,3v18c0,0.6,0.4,1,1,1h3v-6h12v6h3c0.6,0,1-0.4,1-1V3C22,2.4,21.6,2,21,2z"/><rect x="14" y="18" class="dx_darkgray" width="2" height="4"/><path class="dx_darkgray" d="M15,13h8c0.6,0,1-0.4,1-1V4.4c0-0.3-0.1-0.5-0.3-0.7l-2.4-2.4C21.1,1.1,20.9,1,20.6,1H15c-0.6,0-1,0.4-1,1v10C14,12.6,14.4,13,15,13z"/><polygon class="dx_white" points="16,11 22,11 22,5 20,5 20,3 16,3 "/></svg>'

        this._dashboardControl.registerIcon(saveIcon);
        this._dashboardControl.registerIcon(saveAsIcon);

        var toolbarSaveItem = new DevExpress.Dashboard.Designer.DashboardToolbarItem("save", this.save.bind(this), "dx-dashboard-toolbar-save", "Save (Custom Extension)");
        var toolbarSaveAsItem = new DevExpress.Dashboard.Designer.DashboardToolbarItem("save-as", this.showPopup.bind(this), "dx-dashboard-toolbar-save-as", "Save As... (Custom Extension)");
        this._toolbarSaveGroup = new DevExpress.Dashboard.Designer.DashboardToolbarGroup("save", "Save", 60, toolbarSaveItem, toolbarSaveAsItem);
    }
    start() {
        var toolbox = this._dashboardControl.findExtension("toolbox");
        if(toolbox) {
            toolbox.menuItems.push(this._menuSaveAsItem);
            toolbox.toolbarGroups.push(this._toolbarSaveGroup);
            toolbox.menuItems().filter(function (item) { return item.id === "save" })[0].hasSeparator = false;

            this._popupContainer = document.createElement('div');
            document.body.appendChild(this._popupContainer);
            this._popup = this.createPopup(
                function () {
                    this.saveAs();
                    this.hidePopup();
                },
                function () {
                    this.hidePopup();
                }
            );
        }
    }
    stop() {
        var toolbox = this._dashboardControl.findExtension("toolbox");
        if(toolbox) {
            toolbox.menuItems.remove(this._menuSaveAsItem);
            toolbox.toolbarGroups.remove(this._toolbarSaveGroup);

            document.body.removeChild(this._popupContainer);
        }
    }
    save () {
        var saveExtension = this._dashboardControl.findExtension("saveDashboard");
        saveExtension && saveExtension.saveDashboard();
    }
    saveAs () {
        var newExtension = this._dashboardControl.findExtension("createDashboard");
        newExtension && newExtension.performCreateDashboard(this.newName, this._dashboardControl.dashboard().getJSON());
    }
    createPopup (saveButtonAction, cancelButtonAction) {
        var _this = this;
        return new DevExpress.ui.dxPopup(this._popupContainer, {
            title: "Save As... (Custom Extension)",
            width: 400,
            height: 185,
            toolbarItems: [{
                toolbar: "bottom",
                widget: "dxButton",
                location: "after",
                options: {
                    text: "Save",
                    onClick: saveButtonAction.bind(this)
                }
            }, {
                toolbar: "bottom",
                widget: "dxButton",
                location: "after",
                options: {
                    text: "Cancel",
                    onClick: cancelButtonAction.bind(this)
                }
            }],
            contentTemplate: function () {
                var div = document.createElement('div');
                new DevExpress.ui.dxTextBox(div, {
                    value: _this.newName,
                    onValueChanged: function (e) { _this.newName = e.value; }
                });
                return div;
            }
        });
    }
    showPopup () {
        this._popup.show();
    }
    hidePopup () {
        this._dashboardControl.findExtension("toolbox").menuVisible(false);
        this._popup.hide();
    }
}