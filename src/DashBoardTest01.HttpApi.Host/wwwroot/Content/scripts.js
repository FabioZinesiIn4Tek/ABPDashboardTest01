window.dxDemo = window.dxDemo || {};

dxDemo.colorSchemeIcon = '<svg id="colorSchemeIcon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><defs><style>.dx_gray{fill:#7b7b7b;}</style></defs><title>Themes copy</title><path class="dx_gray" d="M12,3a9,9,0,0,0,0,18c7,0,1.35-3.13,3-5,1.4-1.59,6,4,6-4A9,9,0,0,0,12,3ZM5,10a2,2,0,1,1,2,2A2,2,0,0,1,5,10Zm3,7a2,2,0,1,1,2-2A2,2,0,0,1,8,17Zm3-8a2,2,0,1,1,2-2A2,2,0,0,1,11,9Zm5,1a2,2,0,1,1,2-2A2,2,0,0,1,16,10Z" /></svg>';
dxDemo.colorSchemaList = [{
    key: 'Material Compact',
    items: [
        { data: "material.blue.light.compact", text: "Blue Light" },
        { data: "material.lime.light.compact", text: "Lime Light" },
        { data: "material.orange.light.compact", text: "Orange Light" },
        { data: "material.purple.light.compact", text: "Purple Light" },
        { data: "material.teal.light.compact", text: "Teal Light" },
        { data: "material.blue.dark.compact", text: "Blue Dark" },
        { data: "material.lime.dark.compact", text: "Lime Dark" },
        { data: "material.orange.dark.compact", text: "Orange Dark" },
        { data: "material.purple.dark.compact", text: "Purple Dark" },
        { data: "material.teal.dark.compact", text: "Teal Dark" },
    ]
}, {
    key: 'Generic',
    items: [
        { data: "light", text: "Light" },
        { data: "dark", text: "Dark" },
        { data: "carmine", text: "Carmine" },
        { data: "darkmoon", text: "Dark Moon" },
        { data: "greenmist", text: "Green Mist" },
        { data: "darkviolet", text: "Dark Violet" },
        { data: "softblue", text: "Soft Blue" },
    ]
}, {
    key: 'Generic Compact',
    items: [
        { data: "light.compact", text: "Light" },
        { data: "dark.compact", text: "Dark" },
        //"carmine.compact": "Carmine Compact",
        //"darkmoon.compact": "Dark Moon Compact",
        //"greenmist.compact": "Green Mist Compact",
        //"darkviolet.compact": "Dark Violet Compact",
        //"softblue.compact": "Soft Blue Compact"
    ]
}, {
    key: 'Custom themes',
    items: [
        { data: "light-blue", text: "Light Blue" },
        { data: "dark-blue", text: "Dark Blue" },
    ]
}];

dxDemo.State = {
    isMobileView: false,
    isDesignerMode: false,
    getColorSchema: function () {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for(var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if(pair[0] === "colorSchema") { return pair[1]; }
        }
        return "light";
    }
};

dxDemo.Navigation = {
    replaceUrlValue: function(uri, key, value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        var newParameterValue = value ? key + "=" + encodeURIComponent(value) : "";
        var newUrl;
        if(uri.match(re)) {
            var separator = !!newParameterValue ? '$1' : "";
            newUrl = uri.replace(re, separator + newParameterValue + '$2');
        }
        else if(!!newParameterValue) {
            newUrl = uri + separator + newParameterValue;
        }
        return newUrl;
    },
    saveToUrl: function (key, value) {
        var uri = location.href;
        var newUrl = this.replaceUrlValue(uri, key, value);
        if(newUrl) {
            if(newUrl.length > 2000) {
                newUrl = this.replaceUrlValue(uri, key, null);
            }
            history.replaceState({}, "", newUrl);
        }
    },
    navigate: function (baseLink) {
        window.location = baseLink + window.location.search;
        window.event.preventDefault ? window.event.preventDefault() : (window.event.returnValue = false);
        return false;
    }
};

dxDemo.UI = {
    setButtonCaption: function () {
        var buttonTextElement = document.getElementById("designer-mode-button-text");
        var modeTitle = document.getElementById("working-mode-title");
        var text = "", title = "";
        if(!buttonTextElement) {
            return;
        }
        if(dxDemo.State.isMobileView) {
            return;
        }
        var dashboardControl = getDashboardControl();
        if(dashboardControl) {
            text = dashboardControl.isDesignMode() ? "Switch to Viewer" : "Edit in Designer";
            title = dashboardControl.isDesignMode() ? "Designer Mode" : "Viewer Mode";
        }
        buttonTextElement.innerText = text;
        modeTitle.innerText = title;
    }
};

dxDemo.themeChooser = {
    listItemTemplate: itemData => {
        let container = document.createElement('div');
        container.classList.add('dx-dashboard-flex-parent');
        container.classList.add('dx-dashboard-themechooser-item');

        let imageDiv = document.createElement('div');
        imageDiv.classList.add('dx-dashboard-fixed');
        imageDiv.classList.add('dx-dashboard-circle');

        const themeParts = itemData.data.split(".");
        if (themeParts[themeParts.length - 1] === "compact") {
            themeParts.pop();
            imageDiv.classList.add("dx-dashboard-compact");
        }
        imageDiv.classList.add('dx-dashboard-' + themeParts.join("-"));

        container.appendChild(imageDiv);

        let textDiv = document.createElement('div');
        textDiv.innerText = itemData.text;
        container.appendChild(textDiv);

        return container;
    },
    popoverContentTemplate: contentElement => {
        const selectedItem = dxDemo.colorSchemaList.reduce((acc, val) => {
            if (acc)
                return acc;
            const item = val.items.filter(item => item.data === dxDemo.State.getColorSchema());
            return item.length ? item : null;
        }, null);

        const listOptions = {
            grouped: true,
            dataSource: dxDemo.colorSchemaList,
            itemTemplate: dxDemo.themeChooser.listItemTemplate,
            selectionMode: 'single',
            selectedItems: selectedItem,
            onItemClick: args => {
                dxDemo.Navigation.saveToUrl("colorSchema", args.itemData.data);
                location.reload();
            }
        };

        return new DevExpress.ui.dxList(document.createElement('div'), listOptions).element();
    },
    popoverInstance: null,
    clickHandler: ($element, dashboardControlContainer) => {
        const element = $element[0];

        let popoverContainer = element.querySelector('.menu-popover-container');
        if (!popoverContainer) {
            dxDemo.themeChooser.popoverInstance?.dispose();
            dxDemo.themeChooser.popoverInstance = null;

            popoverContainer = document.createElement('div');
            popoverContainer.classList.add('menu-popover-container');
            element.appendChild(popoverContainer);
        }

        const popoverOptions = {
            maxHeight: '50%',
            target: element,
            animation: {
                show: { type: 'pop', from: { opacity: 1, scale: 0 }, to: { scale: 1 } },
                hide: { type: 'pop', from: { scale: 1 }, to: { scale: 0 } }
            },
            position: {
                my: 'top center',
                at: 'bottom center',
                collision: 'fit flip',
                boundary: dashboardControlContainer
            },
            container: dashboardControlContainer,
            wrapperAttr: { class: 'dx-dashboard-list-popover-wrapper' },
            contentTemplate: dxDemo.themeChooser.popoverContentTemplate
        };

        if (!dxDemo.themeChooser.popoverInstance)
            dxDemo.themeChooser.popoverInstance = new DevExpress.ui.dxPopover(popoverContainer, popoverOptions);

        const popoverInstance = dxDemo.themeChooser.popoverInstance;
        popoverInstance.toggle(!popoverInstance.option('visible'));
    }
};

function onDashboardTitleToolbarUpdated(args) {
    if(dxDemo.Sidebar && DevExpress.devices.real().phone) {        
        args.options.actionItems.unshift(dxDemo.Sidebar.getToolbarItem(args.component));
    } 

    args.options.actionItems.unshift({
        type: "button",
        icon: "colorSchemeIcon",
        hint: "Theme",
        click: element => dxDemo.themeChooser.clickHandler(element, args.component.getWidgetContainer())
    });
}

function onBeforeRender(dashboardControl) {
    dashboardControl.on('dashboardInitialized', onDashboardChanged);
    const viewerApi = dashboardControl.findExtension('viewerApi');
    viewerApi?.on('dashboardTitleToolbarUpdated', args => onDashboardTitleToolbarUpdated({ component: dashboardControl, ...args }));

    DevExpress.Dashboard.ResourceManager.registerIcon(dxDemo.colorSchemeIcon);
    dxDemo.UI.setButtonCaption();
    dxDemo.Navigation.saveToUrl("mode", dashboardControl.isDesignMode() ? "designer" : "viewer");

    dashboardControl.isDesignMode.subscribe(function (isDesignValue) {
        dxDemo.Navigation.saveToUrl("mode", isDesignValue ? "designer" : "viewer");
        dxDemo.UI.setButtonCaption();
        dxDemo.State.isDesignerMode = isDesignValue;
    });

    const panelExtension = new DevExpress.Dashboard.DashboardPanelExtension(dashboardControl, { dashboardThumbnail: "./Content/DashboardThumbnail/{0}.png" });
    dashboardControl.registerExtension(panelExtension);
    panelExtension.allowSwitchToDesigner(false);

    if(!dashboardControl.findExtension("textBoxItemEditor")) {
        dashboardControl.registerExtension(new DevExpress.Dashboard.Designer.TextBoxItemEditorExtension(dashboardControl))
    }
    /* Custom Properties Extension */
    dashboardControl.registerExtension(new ChartLineOptionsExtension(dashboardControl));
    /*          Custom Item Extensions         */
    dashboardControl.registerExtension(new FunnelD3CustomItem(dashboardControl));
    dashboardControl.registerExtension(new WebPageCustomItem(dashboardControl));
    dashboardControl.registerExtension(new OnlineMapCustomItem(dashboardControl));
}



function onDashboardChanged(args) {
    var dashboardControl = args.component,
        dashboardId = args.dashboardId;
    if(dashboardId === "CustomItemExtensions") {
        !dashboardControl.findExtension("saveAs") && dashboardControl.registerExtension(new SaveAsDashboardExtension(dashboardControl));
    } else {
        dashboardControl.unregisterExtension("saveAs");
    }

    dxDemo.Sidebar && dxDemo.Sidebar.viewModel && dxDemo.Sidebar.viewModel.feedback && dxDemo.Sidebar.viewModel.feedback.init(dashboardId);
}


document.addEventListener("DOMContentLoaded", function (event) {
    var designModeButton = document.getElementById("designer-mode-button"),
        hasDemoToolbar = designModeButton;    

    dxDemo.State.isMobileView = document.querySelector(".phone-wrapper") !== null;

    if(hasDemoToolbar) {
        designModeButton.addEventListener("click", function (event) {
            var dashboardControl = getDashboardControl();
            if(dashboardControl.isDesignMode()) {
                dashboardControl.switchToViewer();
            } else {
                dashboardControl.switchToDesigner();
            }
            event.preventDefault();
        });

        document.getElementById("info-button").addEventListener("click", function(event) {
            var dashboardControl = getDashboardControl();
            dxDemo.Sidebar.showDemoPopup(dashboardControl, dashboardControl.dashboard(), "header");
            event.preventDefault();
        });
    }
    var demoName = window.dxDemoName || "WebDashboardDemo",
        platform = window.dxDemoPlatform || "a Web application";
    dxDemo.Sidebar && dxDemo.Sidebar.initPopup(demoName, platform);

    dxDemo.Feedback && dxDemo.Feedback.init("https://services.devexpress.com/customerfeedback", demoName);
    
    if(dxDemo.State.isMobileView) {
        var className = "dx-state-selected";
        document.getElementById("desktop-button").classList.remove(className);
        document.getElementById("mobile-button").classList.add(className);
        designModeButton.classList.add("dx-state-disabled");
    }
});